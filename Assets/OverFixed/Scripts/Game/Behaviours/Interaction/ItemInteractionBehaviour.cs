using System;
using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Character.Input;
using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Interaction
{
    [Serializable] public class ItemEvent : UnityEvent<Type>{}
    
    [RequireComponent(typeof(IInteractionInput))]
    public class ItemInteractionBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _itemContainer;
        
        [SerializeField] private ItemEvent _onPickUp;
        [SerializeField] private ItemEvent _onDrop;
        [SerializeField] private ItemEvent _onUseBegin;
        [SerializeField] private ItemEvent _onUseEnd;
            
        private IInteractionInput _interactionInput;
        private IInteractionInput InteractionInput
        {
            get
            {
                if (_interactionInput == null) _interactionInput = GetComponent<IInteractionInput>();
                return _interactionInput;
            }
        }

        private bool _hasItem;
        private bool _shouldPickItem;
        private ItemBehaviourBase _currentItemBehaviourBase;
        private IList<ItemBehaviourBase> _interactableItemBehaviours;
        private IList<ItemBehaviourBase> _accessibleItemBehaviours;

        [Inject]
        public void Initialize(ItemBehaviourBase[] items)
        {
            _interactableItemBehaviours = items;
        }
        
        private void Awake()
        {
            _accessibleItemBehaviours = new List<ItemBehaviourBase>(); 
            
            InteractionInput.OnPicked += InteractionInput_OnPick;
            InteractionInput.OnUseStarted += InteractionInput_OnUseDown;
            InteractionInput.OnUseEnded += InteractionInput_OnUseUp;
        }

        private void OnDestroy()
        {   
            InteractionInput.OnPicked -= InteractionInput_OnPick;
            InteractionInput.OnUseStarted -= InteractionInput_OnUseDown;
            InteractionInput.OnUseEnded -= InteractionInput_OnUseUp;            
        }
        
        private void Pick(ItemBehaviourBase item)
        {
            _hasItem = true;
            _currentItemBehaviourBase = item;
            _currentItemBehaviourBase.BoundItem.Equipped = true;
            
            _currentItemBehaviourBase.transform.SetParent(_itemContainer);
            _currentItemBehaviourBase.transform.localPosition = Vector3.zero;
            _currentItemBehaviourBase.transform.localEulerAngles = Vector3.zero;
        }

        private void BeginUse()
        {
            _currentItemBehaviourBase.BoundItem.Using = true;
            
            _onUseBegin?.Invoke(_currentItemBehaviourBase.GetType());
        }

        private void EndUse()
        {
            _currentItemBehaviourBase.BoundItem.Using = false;
         
            _onUseEnd?.Invoke(_currentItemBehaviourBase.GetType());
        }

        private void Drop()
        {
            _hasItem = false;
            _currentItemBehaviourBase.BoundItem.Equipped = false;
            _currentItemBehaviourBase.BoundItem.Using = false;
            
            _currentItemBehaviourBase.transform.SetParent(null);
            
            _onDrop.Invoke(_currentItemBehaviourBase.GetType());
            _currentItemBehaviourBase = null;
        }

        private ItemBehaviourBase GetClosestItem()
        {
            ItemBehaviourBase closest = _accessibleItemBehaviours.First();
            for (int i = 0; i < _accessibleItemBehaviours.Count; i++)
            {
                if (Vector3.Distance(_accessibleItemBehaviours[i].transform.position, transform.position) <
                    Vector3.Distance(closest.transform.position, transform.position))
                {
                    closest = _accessibleItemBehaviours[i];
                }
            }
            
            return closest;
        }
        
        #region Event Listeners

        private void InteractionInput_OnPick()
        {
            if (_hasItem)
            {
                Drop();
            }
            else if(_accessibleItemBehaviours.Count > 0)
            {
                Pick(GetClosestItem());
                _onPickUp?.Invoke(_currentItemBehaviourBase.GetType());
            }
        }
        
        private void InteractionInput_OnUseDown()
        {
            if (!_hasItem) return;
            
            BeginUse();
        }

        private void InteractionInput_OnUseUp()
        {
            if (!_hasItem) return;
            
            EndUse();
        }
        
        public void OnInteractionTriggerEnter(Collider other)
        {
            ItemBehaviourBase itemBehaviour = other.GetComponent<ItemBehaviourBase>();
            if (itemBehaviour != null && !_accessibleItemBehaviours.Contains(itemBehaviour))
            {
                _accessibleItemBehaviours.Add(itemBehaviour);
            }
        }

        public void OnInteractionTriggerExit(Collider other)
        {
            ItemBehaviourBase itemBehaviour = other.GetComponent<ItemBehaviourBase>();
            if (itemBehaviour != null && _accessibleItemBehaviours.Contains(itemBehaviour))
            {
                _accessibleItemBehaviours.Remove(itemBehaviour);
            }
        }

        #endregion
    }
}
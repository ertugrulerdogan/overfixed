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
        public static event Action OnItemPick;
        public static event Action OnItemDrop;
        public static event Action<Type> OnItemUseBegin;
        public static event Action<Type> OnItemUseEnd;
        
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private Transform _wrenchContainer;
        
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
        private IList<ItemBehaviourBase> _accessibleItemBehaviours;
        
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
            
            OnItemPick?.Invoke();
            _onPickUp?.Invoke(_currentItemBehaviourBase.GetType());
        }

        private void BeginUse()
        {
            _currentItemBehaviourBase.BoundItem.Using = true;

            if (_currentItemBehaviourBase.GetType() == typeof(WrenchBehaviour))
            {
                WrenchBehaviour behaviour = ((WrenchBehaviour) _currentItemBehaviourBase);
                behaviour.Visuals.SetParent(_wrenchContainer);
                behaviour.Visuals.localPosition = Vector3.zero;
                behaviour.Visuals.localEulerAngles = Vector3.zero;
                behaviour.Visuals.localScale = Vector3.one;
            }
            
            OnItemUseBegin?.Invoke(_currentItemBehaviourBase.GetType());
            _onUseBegin?.Invoke(_currentItemBehaviourBase.GetType());
        }

        private void EndUse()
        {
            _currentItemBehaviourBase.BoundItem.Using = false;
                        
            if (_currentItemBehaviourBase.GetType() == typeof(WrenchBehaviour))
            {
                WrenchBehaviour behaviour = ((WrenchBehaviour) _currentItemBehaviourBase);
                behaviour.SetVisualsAsChild();
            }
            
            OnItemUseEnd?.Invoke(_currentItemBehaviourBase.GetType());
           _onUseEnd?.Invoke(_currentItemBehaviourBase.GetType());
        }

        private void Drop()
        {
            _hasItem = false;
            _currentItemBehaviourBase.BoundItem.Equipped = false;
            _currentItemBehaviourBase.BoundItem.Using = false;
            
            _currentItemBehaviourBase.transform.SetParent(null);            
            _currentItemBehaviourBase.Drop();
            
            OnItemDrop?.Invoke();
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
            if(_accessibleItemBehaviours.Count > 1 || (_accessibleItemBehaviours.Count > 0 && !_hasItem))
            {
                if (_hasItem)
                {
                    _accessibleItemBehaviours.Remove(_currentItemBehaviourBase);
                    Drop();
                }
                
                Pick(GetClosestItem());
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
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
    [RequireComponent(typeof(IInteractionInput))]
    public class ItemInteractionBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private UnityEvent _onPickUp;
        [SerializeField] private UnityEvent _onUsing;
            
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

        [Inject]
        public void Initialize(ItemBehaviourBase[] items)
        {
            _interactableItemBehaviours = items;
        }
        
        private void Awake()
        {
            InteractionInput.OnPick += InteractionInput_OnPick;
            InteractionInput.OnUseDown += InteractionInput_OnUseDown;
            InteractionInput.OnUseUp += InteractionInput_OnUseUp;
        }

        private void OnDestroy()
        {   
            InteractionInput.OnPick -= InteractionInput_OnPick;
            InteractionInput.OnUseDown -= InteractionInput_OnUseDown;
            InteractionInput.OnUseUp -= InteractionInput_OnUseUp;            
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
        }

        private void EndUse()
        {
            _currentItemBehaviourBase.BoundItem.Using = false;
        }

        private void Drop()
        {
            _hasItem = false;
            _currentItemBehaviourBase.BoundItem.Equipped = false;
            _currentItemBehaviourBase.BoundItem.Using = false;
            
            _currentItemBehaviourBase.transform.SetParent(null);
            _currentItemBehaviourBase = null;
        }

        private ItemBehaviourBase GetClosestItem()
        {
            ItemBehaviourBase closest = _interactableItemBehaviours.First();
            for (int i = 0; i < _interactableItemBehaviours.Count; i++)
            {
                if (Vector3.Distance(_interactableItemBehaviours[i].transform.position, transform.position) <
                    Vector3.Distance(closest.transform.position, transform.position))
                {
                    closest = _interactableItemBehaviours[i];
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
            else if(_interactableItemBehaviours.Count > 0)
            {
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
            ItemBehaviour<Item> itemBehaviour = other.GetComponent<ItemBehaviour<Item>>();
            if (itemBehaviour != null && !_interactableItemBehaviours.Contains(itemBehaviour))
            {
                _interactableItemBehaviours.Add(itemBehaviour);
            }
        }

        public void OnInteractionTriggerExit(Collider other)
        {
            ItemBehaviour<Item> itemBehaviour = other.GetComponent<ItemBehaviour<Item>>();
            if (itemBehaviour != null && _interactableItemBehaviours.Contains(itemBehaviour))
            {
                _interactableItemBehaviours.Remove(itemBehaviour);
            }
        }

        #endregion
    }
}
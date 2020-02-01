using System.Collections.Generic;
using OverFixed.Scripts.Game.Behaviours.Character.Input;
using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Interaction
{
    [RequireComponent(typeof(IInteractionInput))]
    public class ItemInteractionBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _itemContainer;

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
        private ItemBehaviour<Item> _currentItemBehaviour;
        private List<ItemBehaviour<Item>> _interactableItemBehaviours;
        
        private void Awake()
        {
            _interactableItemBehaviours = new List<ItemBehaviour<Item>>();
            
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
        
        private void Pick(ItemBehaviour<Item> item)
        {
            _hasItem = true;
            _currentItemBehaviour = item;
            _currentItemBehaviour.Item.Equipped = true;
            
            _currentItemBehaviour.transform.SetParent(_itemContainer);
            _currentItemBehaviour.transform.localPosition = Vector3.zero;
            _currentItemBehaviour.transform.localEulerAngles = Vector3.zero;
        }

        private void BeginUse()
        {
            _currentItemBehaviour.Item.Using = true;
        }

        private void EndUse()
        {
            _currentItemBehaviour.Item.Using = false;
        }

        private void Drop()
        {
            _hasItem = false;
            _currentItemBehaviour.Item.Equipped = false;
            _currentItemBehaviour.Item.Using = false;
            
            _currentItemBehaviour.transform.SetParent(null);
            _currentItemBehaviour = null;
        }

        private ItemBehaviour<Item> GetClosestItem()
        {
            return null;
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
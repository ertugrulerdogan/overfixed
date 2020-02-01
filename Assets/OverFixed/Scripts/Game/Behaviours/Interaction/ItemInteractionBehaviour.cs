using OverFixed.Scripts.Game.Behaviours.Character.Input;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Interaction
{
    [RequireComponent(typeof(IInteractionInput))]
    public class ItemInteractionBehaviour : MonoBehaviour
    {
        public bool HasItem { get; private set; }
        public Item CurrentItem { get; private set; }
        
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
        
        private void Awake()
        {
            InteractionInput.OnPick += InteractionInput_OnPick;
            InteractionInput.OnUse += InteractionInput_OnUse;
        }
 
        private void Pick(){}
        
        private void Use(){}
        
        private void Drop(){}
        
        private void InteractionInput_OnPick()
        {
            if (HasItem) return;
        }
        
        private void InteractionInput_OnUse()
        {
            if (!HasItem) return;
            
            throw new System.NotImplementedException();
        }

        public void OnInteractionTriggerEnter(Collider other)
        {
            if (other.GetComponent<Item>() != null)
            {
                
            }
        }

        public void OnInteractionTriggerExit(Collider other)
        {
            
        }
    }
}
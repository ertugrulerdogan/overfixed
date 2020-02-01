using System;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    using Input = UnityEngine.Input;
    
    public class DefaultInteractionInput : MonoBehaviour, IInteractionInput
    {
        public event Action OnPicked;
        public event Action OnUseStarted;
        public event Action OnUseEnded;

        [SerializeField] private KeyCode _pickKey;
        [SerializeField] private KeyCode _useKey;

        private bool _useToggle;
                
        public void OnPick()
        {
            OnPicked?.Invoke();
        }

        public void OnUse()
        {            
            _useToggle = !_useToggle;
            
            if(_useToggle) OnUseStarted?.Invoke();
            else OnUseEnded?.Invoke();
        }
    }
}
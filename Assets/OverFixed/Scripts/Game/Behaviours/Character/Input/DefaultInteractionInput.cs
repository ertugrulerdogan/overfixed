using System;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    using Input = UnityEngine.Input;
    
    public class DefaultInteractionInput : MonoBehaviour, IInteractionInput
    {
        public event Action OnPick;
        public event Action OnUseDown;
        public event Action OnUseUp;

        [SerializeField] private KeyCode _pickKey;
        [SerializeField] private KeyCode _useKey;

        public void Update()
        {
            if(Input.GetKeyDown(_pickKey)) OnPick?.Invoke();
            if(Input.GetKeyDown(_useKey)) OnUseDown?.Invoke();
            if(Input.GetKeyUp(_useKey)) OnUseUp?.Invoke();
        }
    }
}
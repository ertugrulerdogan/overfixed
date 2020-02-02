using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    public class DefaultDirectionalInput : MonoBehaviour, IDirectionalInput
    {
        public event Action OnChanged;
        
        public Quaternion LookRotation { get; private set; }

        public float Vertical { get; private set; }
        public float Horizontal { get; private set; }

        private Camera _mainCamera;

        private bool _isGettingMoveInput;
        
        [Inject]
        private void Initialize(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }
        
        private float GetAngle(Vector3 from, Vector3 to)
        {
            return 360f - Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
        }

        public void OnMove(InputValue inputValue)
        {
            Horizontal = inputValue.Get<Vector2>().x;
            Vertical = inputValue.Get<Vector2>().y;
            
            OnChanged?.Invoke();
        }
        
        public void OnMoveEnd()
        {
            Horizontal = 0f;
            Vertical = 0f;
        }
    }
}
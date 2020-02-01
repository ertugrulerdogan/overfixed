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
        
        public void Update()
        {
//            LookRotation = Quaternion.Euler(0f, GetAngle(_mainCamera.WorldToScreenPoint(transform.position), Input.mousePosition), 0f);
//            
//            Vertical = Input.GetAxis("Vertical");
//            Horizontal = Input.GetAxis("Horizontal");
        }

        private float GetAngle(Vector3 from, Vector3 to)
        {
            return 360f - Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
        }

        public void OnMove(InputValue inputValue)
        {
            Horizontal = inputValue.Get<Vector2>().x;
            Vertical = inputValue.Get<Vector2>().y;
        }
        
        public void OnMoveEnd()
        {
            Horizontal = 0f;
            Vertical = 0f;
        }
        
        public void OnKeyboardMove(InputValue inputValue)
        {
//            Horizontal = inputValue.Get<Keyboard>().;
//            Vertical = inputValue.Get<Vector2>().y;
        }
    }
}
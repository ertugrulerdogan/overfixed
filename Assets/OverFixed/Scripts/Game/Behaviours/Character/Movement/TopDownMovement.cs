using System.IO;
using OverFixed.Scripts.Game.Behaviours.Character.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Character.Movement
{
    [RequireComponent(typeof(IDirectionalInput), typeof(Rigidbody))]
    public class TopDownMovement : MonoBehaviour, IMovement
    {
        public Vector3 Velocity { get; private set; }
        
        [SerializeField] private float _speed;
        
        private Camera _mainCamera;
        
        private IDirectionalInput _directionalInput;
        private IDirectionalInput DirectionalInput
        {
            get
            {
                if (_directionalInput == null) _directionalInput = GetComponent<IDirectionalInput>();
                return _directionalInput;
            }
        }

        private Rigidbody _rigidbody;
        private Rigidbody Rigidbody
        {
            get
            {
                if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        private bool _canMove;
        private Vector3 _movementAmount;
        private Vector3 _lastPosition;
        
        [Inject]
        private void Initialize(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }

        private void Awake()
        {
            _lastPosition = transform.position;
        }

        private void FixedUpdate()
        {
            Move();
            
            Velocity = transform.position - _lastPosition;
            _lastPosition = transform.position;

            if (Velocity.magnitude> 0.1f)
            {
                transform.forward = Velocity.normalized;                
            }
        }

        public void Move()
        {
            Rigidbody.MovePosition(Rigidbody.position + GetMovementAmount(Time.fixedDeltaTime));
        }
        
        private Vector3 GetMovementAmount(float deltaTime)
        {
            _movementAmount.x = DirectionalInput.Horizontal * _speed * deltaTime;
            _movementAmount.y = 0f;
            _movementAmount.z = DirectionalInput.Vertical * _speed * deltaTime;
            
            return Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * _movementAmount;
        }
    }
}
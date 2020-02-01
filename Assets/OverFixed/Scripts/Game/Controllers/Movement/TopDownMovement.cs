using OverFixed.Scripts.Game.Controllers.Camera;
using OverFixed.Scripts.Game.Controllers.Input;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers.Movement
{
    [RequireComponent(typeof(IDirectionalInput), typeof(Rigidbody))]
    public class TopDownMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private CameraController _cameraController;
        
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

        private Vector3 _movementAmount;
        
        [Inject]
        private void Initialize(CameraController cameraController)
        {
            _cameraController = cameraController;
        }
        
        private void FixedUpdate()
        {
            Rigidbody.MovePosition(Rigidbody.position + GetMovementAmount(Time.fixedDeltaTime));
            Rigidbody.MoveRotation(DirectionalInput.LookRotation.normalized);
        }

        private Vector3 GetMovementAmount(float deltaTime)
        {
            _movementAmount.x = DirectionalInput.Horizontal * _speed * deltaTime;
            _movementAmount.y = 0f;
            _movementAmount.z = DirectionalInput.Vertical * _speed * deltaTime;
            
            return Quaternion.Euler(0f, _cameraController.transform.eulerAngles.y, 0f) * _movementAmount;
        }
        
    }
}
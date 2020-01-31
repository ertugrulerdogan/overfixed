using OverFixed.Scripts.Game.Controllers.Camera;
using OverFixed.Scripts.Game.Controllers.Input;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers.Movement
{
    [RequireComponent(typeof(IDirectionalInput), typeof(Rigidbody))]
    public class TopDownMovement : MonoBehaviour
    {
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

        [Inject]
        private void Initialize(CameraController cameraController)
        {
            _cameraController = cameraController;
        }
        
        private void Update()
        {
            
        }
    }
}
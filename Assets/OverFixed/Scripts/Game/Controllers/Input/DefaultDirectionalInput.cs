using OverFixed.Scripts.Game.Controllers.Camera;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers.Input
{
    using Input = UnityEngine.Input;
    
    public class DefaultDirectionalInput : MonoBehaviour, IDirectionalInput
    {
        public Quaternion LookRotation { get; private set; }
        public float Vertical { get; private set; }
        public float Horizontal { get; private set; }

        private CameraController _cameraController;
        
        [Inject]
        private void Initialize(CameraController cameraController)
        {
            _cameraController = cameraController;
        }
        
        public void Update()
        {
            LookRotation = Quaternion.Euler(0f, GetAngle(_cameraController.GetScreenPosition(transform.position), Input.mousePosition), 0f);
            
            Vertical = Input.GetAxis("Vertical");
            Horizontal = Input.GetAxis("Horizontal");
        }

        private float GetAngle(Vector3 from, Vector3 to)
        {
            return 360f - Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
        }
    }
}
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    using Input = UnityEngine.Input;
    
    public class DefaultDirectionalInput : MonoBehaviour, IDirectionalInput
    {
        public Quaternion LookRotation { get; private set; }
        public float Vertical { get; private set; }
        public float Horizontal { get; private set; }

        private Camera _mainCamera;
        
        [Inject]
        private void Initialize(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }
        
        public void Update()
        {
            LookRotation = Quaternion.Euler(0f, GetAngle(_mainCamera.WorldToScreenPoint(transform.position), Input.mousePosition), 0f);
            
            Vertical = Input.GetAxis("Vertical");
            Horizontal = Input.GetAxis("Horizontal");
        }

        private float GetAngle(Vector3 from, Vector3 to)
        {
            return 360f - Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
        }
    }
}
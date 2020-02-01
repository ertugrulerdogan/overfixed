using UnityEngine;

namespace OverFixed.Scripts.Game.Controllers.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraController : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        private UnityEngine.Camera Camera
        {
            get
            {
                if (_camera == null) _camera = GetComponent<UnityEngine.Camera>();
                return _camera;
            }
        }

        public Vector3 GetScreenPosition(Vector3 worldPosition)
        {
            return Camera.WorldToScreenPoint(worldPosition);
        }
    }
}
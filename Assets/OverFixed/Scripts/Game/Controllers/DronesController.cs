using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Drones;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers
{
    public class DronesController : MonoBehaviour
    {
        private const float DroneFrequency = 20f;

        private DroneBehaviour.Pool _pool;
        private Tween _tween;
        
        [Inject]
        public void Initialize(DroneBehaviour.Pool pool)
        {
            _pool = pool;
            // _tween = DOVirtual.DelayedCall(20, () =>
            // {
            //     var drone = _pool.Spawn();
            //     drone.transform.position = 
            // }).SetLoops(-1);
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}
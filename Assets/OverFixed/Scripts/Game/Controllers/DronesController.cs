using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Drones;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers
{
    public class DronesController : MonoBehaviour
    {
        private const float DroneFrequency = 20f;

        [SerializeField] private Transform[] _linesA;
        [SerializeField] private Transform[] _linesB;
        private DroneBehaviour.Pool _pool;
        private Tween _tween;
        
        [Inject]
        public void Initialize(DroneBehaviour.Pool pool)
        {
            _pool = pool;
            _tween = DOVirtual.DelayedCall(DroneFrequency, () =>
            {
                Transform[] points;
                if (Random.value > 0.5f)
                {
                    points = _linesA;
                }
                else
                {
                    points = _linesB;
                }

                var point = Vector3.Lerp(points[0].position, points[1].position, Random.value);
                var drone = _pool.Spawn();
                drone.BeginMovement(point + Vector3.forward * 10f, point);
            }).SetLoops(-1);
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}
using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Drones;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers
{
    public class DronesController : MonoBehaviour
    {
        private const float DroneFrequency = 20f;

        [SerializeField] private Transform _followPoint;
        [SerializeField] private float _spawnRadius;
        private DroneBehaviour.Pool _pool;
        private Tween _tween;
        
        [Inject]
        public void Initialize(DroneBehaviour.Pool pool)
        {
            _pool = pool;
            _tween = DOVirtual.DelayedCall(DroneFrequency, () =>
            {
                var drone = _pool.Spawn();
                var angle = Random.Range(-Mathf.PI, Mathf.PI);
                drone.transform.position = _followPoint.position +
                                           new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _spawnRadius;
                drone.transform.LookAt(_followPoint);
                drone.BeginMovement();
            }).SetLoops(-1);
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Bullets;
using OverFixed.Scripts.Game.Behaviours.Hittables;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Models.Bullets;
using OverFixed.Scripts.Game.Models.Drones;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace OverFixed.Scripts.Game.Behaviours.Drones
{
    public class DroneBehaviour : MonoBehaviour, IHittable
    {
        public event Action Die;
        
        private Drone _drone;
        private Pool _pool;
        private BulletBehaviour.Pool _bulletPool;
        private IList<PlatformBehaviour> _platformBehaviours;
        private Tween _movementTween;
        private Tween _fireTween;

        [Inject]
        public void Initialize(Pool pool, BulletBehaviour.Pool bulletPool, IList<PlatformBehaviour> platformBehaviours)
        {
            _pool = pool;
            _bulletPool = bulletPool;
            _platformBehaviours = platformBehaviours;
        }
        
        private void OnEnable()
        {
            _drone = new Drone();
            _fireTween?.Kill();
            _fireTween = DOVirtual.DelayedCall(_drone.FirePeriod, () =>
            {
                var platforms = _platformBehaviours.Where(platform => platform.Platform.IsPlatformOccupied).ToList();
                if (platforms.Count > 0)
                {
                    var platform = platforms[Random.Range(0, platforms.Count)];
                    var bullet = _bulletPool.Spawn();
                    bullet.Bind(new Bullet(_drone.Damage, true));
                    bullet.transform.position = transform.position;
                    bullet.transform.LookAt(platform.transform);
                }
            });
        }

        public void BeginMovement()
        {
            _movementTween?.Kill();
            _movementTween = transform.DOMove(transform.forward, 5f).SetRelative(true);
        }

        private void OnDestroy()
        {
            _movementTween?.Kill();
        }

        public void Hit(float damage)
        {
            _drone.Health -= damage;
            if (_drone.Health < 0.0001f)
            {
                Die?.Invoke();
                _pool.Despawn(this);
            }
        }
        
        public class Pool : MonoMemoryPool<DroneBehaviour> { }
    }
}
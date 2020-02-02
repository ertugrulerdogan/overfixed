using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Bullets;
using OverFixed.Scripts.Game.Behaviours.Explosion;
using OverFixed.Scripts.Game.Behaviours.Hittables;
using OverFixed.Scripts.Game.Behaviours.Scraps;
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
        private ExplosionBehaviour.Pool _explosionPool;
        private ScrapSpawner _scrapSpawner;
        private IList<PlatformBehaviour> _platformBehaviours;
        private Tween _movementTween;
        private Tween _fireTween;

        [Inject]
        public void Initialize(Pool pool, BulletBehaviour.Pool bulletPool, IList<PlatformBehaviour> platformBehaviours, ExplosionBehaviour.Pool explosionPool, ScrapSpawner scrapSpawner)
        {
            _pool = pool;
            _bulletPool = bulletPool;
            _platformBehaviours = platformBehaviours;
            _explosionPool = explosionPool;
            _scrapSpawner = scrapSpawner;
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
                    bullet.Fire(transform.position, Quaternion.LookRotation(platform.transform.position - transform.position));
                }
            });
        }

        public void BeginMovement(Vector3 from, Vector3 to)
        {
            _movementTween?.Kill();
            transform.position = from;
            transform.rotation = Quaternion.FromToRotation(from, to);
            _movementTween = transform.DOMove(to, 5f);
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
                var explosion = _explosionPool.Spawn();
                explosion.transform.localScale = 0.2f * Vector3.one;
                explosion.transform.position = transform.position;
                _scrapSpawner.Scatter(transform.position, 4);
                _pool.Despawn(this);
            }
        }
        
        public class Pool : MonoMemoryPool<DroneBehaviour> { }
    }
}
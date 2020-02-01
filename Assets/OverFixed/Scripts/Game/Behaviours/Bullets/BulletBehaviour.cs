using System;
using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Hittables;
using OverFixed.Scripts.Game.Models.Bullets;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Bullets
{
    public class BulletBehaviour : MonoBehaviour
    {
        private const string BulletLayer = "Bullet";
        private const string ShipBulletLayer = "ShipBullet";
        
        public delegate void HitEvent(Vector3 position, Vector3 direction);

        public event HitEvent OnHit;

        private Pool _pool;
        private Bullet _bullet;
        private Tween _timeoutTween;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }

        public void Bind(Bullet bullet)
        {
            _bullet = bullet;
            gameObject.layer = LayerMask.NameToLayer(_bullet.IsShipBullet ? ShipBulletLayer : BulletLayer);
            _timeoutTween?.Kill();
            _timeoutTween = DOVirtual.DelayedCall(3f, () => _pool.Despawn(this));
        }

        private void OnDestroy()
        {
            _timeoutTween?.Kill();
        }

        private void Update()
        {
            transform.Translate(0f, 0f, Time.deltaTime * _bullet.Speed);
        }

        private void OnCollisionEnter(Collision other)
        {
            OnHit?.Invoke(other.GetContact(0).point, transform.forward);
            other.gameObject.GetComponent<IHittable>()?.Hit(_bullet.Damage);
            _pool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<BulletBehaviour> { }
    }
}
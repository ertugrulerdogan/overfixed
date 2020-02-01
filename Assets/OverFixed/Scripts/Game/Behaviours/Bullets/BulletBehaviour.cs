using System;
using OverFixed.Scripts.Game.Behaviours.Hittables;
using OverFixed.Scripts.Game.Models.Bullets;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Bullets
{
    public class BulletBehaviour : MonoBehaviour
    {
        public delegate void HitEvent(Vector3 position, Vector3 direction);

        public event HitEvent OnHit;

        private Pool _pool;
        private Bullet _bullet;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }

        public void Bind(Bullet bullet)
        {
            _bullet = bullet;
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
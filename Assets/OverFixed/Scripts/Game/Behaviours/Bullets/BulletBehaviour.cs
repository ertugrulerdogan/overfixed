using OverFixed.Scripts.Game.Behaviours.Hittables;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Bullets
{
    public class BulletBehaviour : MonoBehaviour
    {
        private const float Speed = 20f;
        
        public float Damage { get; set; }

        private Pool _pool;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }
        
        private void Update()
        {
            transform.Translate(0f, 0f, Time.deltaTime * Speed);
        }

        private void OnCollisionEnter(Collision other)
        {
            _pool.Despawn(this);
            other.gameObject.GetComponent<IHittable>()?.Hit(Damage);
        }

        public class Pool : MonoMemoryPool<BulletBehaviour> { }
    }
}
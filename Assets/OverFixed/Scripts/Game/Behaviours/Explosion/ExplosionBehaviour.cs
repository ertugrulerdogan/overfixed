using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Explosion
{
    public class ExplosionBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float _despawnDuration;
        private float _timer;
        private Pool _pool;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > _despawnDuration)
            {
                _pool.Despawn(this);
            }
        }

        private void OnEnable()
        {
            _timer = 0f;
        }

        public class Pool : MonoMemoryPool<ExplosionBehaviour> { }

    }
}
using System;
using OverFixed.Scripts.Game.Behaviours.Hittables;
using OverFixed.Scripts.Game.Models.Drones;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Drones
{
    public class DroneBehaviour : MonoBehaviour, IHittable
    {
        public event Action Die;
        
        private Drone _drone;
        private Pool _pool;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }
        
        private void OnEnable()
        {
            _drone = new Drone();
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
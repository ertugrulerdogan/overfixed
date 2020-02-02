using System;
using OverFixed.Scripts.Game.Behaviours.Bullets;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Views.Bullets
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        private PlaceholderFactory<ParticleSystem> _scatterFactory;
        
        [Inject]
        public void Initialize(PlaceholderFactory<ParticleSystem> scatterFactory)
        {
            _scatterFactory = scatterFactory;
            var bullet = GetComponent<BulletBehaviour>();
            bullet.OnHit += OnHit;
            bullet.OnFire += OnFire;
        }

        private void OnFire()
        {
            _trailRenderer.Clear();
        }

        private void OnHit(Vector3 position, Vector3 direction)
        {
            var scatter = _scatterFactory.Create();
            scatter.transform.position = position;
            scatter.transform.LookAt(transform.position - direction);
        }
    }
}
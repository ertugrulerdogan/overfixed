using System;
using System.Linq;
using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Models.Data;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class WrenchBehaviour : SpherecastItemBehaviour<Wrench>
    {
        public Transform Visuals => _visuals;
        
        [SerializeField] private Transform _visuals;
        
        private TeamData _teamData;
        private PlaceholderFactory<ParticleSystem> _scatterParticlePool;
        private Tween _particleTween;

        private Vector3 _initialVisualPosition;
        private Vector3 _initialVisualRotation;
        
        [Inject]
        public void Initialize(TeamData teamData, PlaceholderFactory<ParticleSystem> scatterPool)
        {
            _teamData = teamData;
            _scatterParticlePool = scatterPool;
        }

        protected override void Start()
        {
            base.Start();
            _initialVisualPosition = Visuals.localPosition;
            _initialVisualRotation = Visuals.localEulerAngles;
        }
        
        protected override void OnHit(ShipBehaviour shipBehaviour)
        {
            if (_teamData.Scrap > 0.0001f)
            {
                var usedScrap = shipBehaviour.Repair(Item.Strength);
                _teamData.Scrap = Mathf.Max(_teamData.Scrap - usedScrap, 0f);
            }
        }

        public void SetVisualsAsChild()
        {
            Visuals.SetParent(transform);
            Visuals.localPosition = _initialVisualPosition;
            Visuals.localEulerAngles = _initialVisualRotation;
        }

        private void OnDisable()
        {
            _particleTween?.Kill();
            _particleTween = null;
        }

        private void LateUpdate()
        {
            if (Item.Using && _particleTween == null)
            {
                _particleTween = DOVirtual.DelayedCall(0.8f, () =>
                {
                    var scatter = _scatterParticlePool.Create();
                    scatter.transform.position = transform.position;
                    scatter.transform.rotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
                }).SetLoops(-1);
            }
            else if (!Item.Using && _particleTween != null)
            {
                _particleTween.Kill();
                _particleTween = null;
            }
        }
    }
}

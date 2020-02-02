using System;
using System.Linq;
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
        private Vector3 _initialVisualPosition;
        private Vector3 _initialVisualRotation;
        
        [Inject]
        public void Initialize(TeamData teamData)
        {
            _teamData = teamData;
        }

        private void Start()
        {
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
    }
}
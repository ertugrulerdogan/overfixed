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
        private TeamData _teamData;
        
        [Inject]
        public void Initialize(TeamData teamData)
        {
            _teamData = teamData;
        }
        
        protected override void OnHit(ShipBehaviour shipBehaviour)
        {
            if (_teamData.Scrap > 0.0001f)
            {
                var usedScrap = shipBehaviour.Repair(Item.Strength);
                _teamData.Scrap = Mathf.Max(_teamData.Scrap - usedScrap, 0f);
            }
        }
    }
}
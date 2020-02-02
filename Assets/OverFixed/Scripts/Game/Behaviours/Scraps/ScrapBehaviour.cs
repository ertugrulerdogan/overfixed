using System;
using OverFixed.Scripts.Game.Behaviours.Pickupables;
using OverFixed.Scripts.Game.Models.Data;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Scraps
{
    public class ScrapBehaviour : MonoBehaviour, IPickupable
    {
        public const float ScrapContribution = 0.5f;
        
        public Rigidbody Rigidbody { get; private set; }
        
        private TeamData _teamData;
        private Pool _pool;
        
        [Inject]
        public void Initialize(TeamData teamData, Pool pool)
        {
            _teamData = teamData;
            _pool = pool;
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Pickup()
        {
            _teamData.Scrap = Mathf.Min(_teamData.Scrap + ScrapContribution);
            Despawn();
        }

        private void Despawn()
        {
            _pool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<ScrapBehaviour> { }
    }
}
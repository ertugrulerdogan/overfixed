using System.Collections.Generic;
using OverFixed.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours
{
    public class HangarBehaviour : MonoBehaviour
    {
        public Hangar Hangar;

        public List<Transform> LandingPositions;
        public List<Transform> SpawnPositions;

        [Inject]
        private void Construct(Hangar hangar)
        {
            Hangar = hangar;
        }
    }
}
using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Ships
{
    public class ShipSectionBehaviour : MonoBehaviour
    {
        private Ship.Section _section;
        [HideInInspector]
        public ShipBehaviour ShipBehaviour;

        public void Init(ShipBehaviour shipBehaviour, Ship.Section section)
        {
            _section = section;
            ShipBehaviour = shipBehaviour;
        }

        public Ship.Section GetSection()
        {
            return _section;
        }
    }
}
using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;

namespace OverFixed.Scripts.Game.Views.Ships
{
    public class ShipSectionBehaviour : MonoBehaviour
    {
        private Ship.Section _section;

        public void Init(Ship.Section section)
        {
            _section = section;
        }

        public Ship.Section GetSection()
        {
            return _section;
        }
    }
}
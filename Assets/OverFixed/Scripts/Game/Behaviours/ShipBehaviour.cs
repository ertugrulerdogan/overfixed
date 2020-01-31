using OverFixed.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Ship Ship;

        [Inject]
        private void Construct(Ship ship)
        {
            Ship = ship;
        }



    }
}

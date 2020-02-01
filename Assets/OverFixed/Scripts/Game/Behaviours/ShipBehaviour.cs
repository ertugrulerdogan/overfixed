using DG.Tweening;
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

        public void Land(Vector3 position)
        {
            transform.DOMove(position, 5f);
        }

        public void TakeOff()
        {
            
        }

        public class Pool : MemoryPool<ShipBehaviour>
        {

        }
    }
}

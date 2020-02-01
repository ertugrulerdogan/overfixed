using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Scraps;
using OverFixed.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Ship Ship;

        public void Land()
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition, 5f));
        }

        public void TakeOff()
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DORotateQuaternion(Quaternion.LookRotation(Ship.Platform.SpawnPosition - Ship.Platform.LandingPosition), 2f));
            seq.Append(transform.DOMove(Ship.Platform.SpawnPosition, 5f));
            
        }

        public class Pool : MonoMemoryPool<ShipBehaviour>
        {

        }
    }
}

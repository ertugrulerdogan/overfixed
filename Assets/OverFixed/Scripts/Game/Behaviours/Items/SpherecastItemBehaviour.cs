using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Ship;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public abstract class SpherecastItemBehaviour<TItem> : ItemBehaviour<TItem> where TItem : Item
    {
        [SerializeField] private float _range;
        private LayerMask _shipMask;

        private void Start()
        {
            _shipMask = LayerMask.GetMask("Ship");
        }
        
        protected override void UseTick()
        {
            var shipInRange = Physics.OverlapSphere(transform.position, _range, _shipMask)
                .OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault();
            var ship = shipInRange?.GetComponent<ShipBehaviour>();
            if (ship != null)
            {
                OnHit(ship);
            }
        }

        protected abstract void OnHit(ShipBehaviour shipBehaviour);
    }
}
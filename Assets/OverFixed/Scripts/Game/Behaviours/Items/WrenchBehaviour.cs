using System;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Ship;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class WrenchBehaviour : ItemBehaviour<Wrench>
    {
        private LayerMask _shipMask;

        private void Start()
        {
            _shipMask = LayerMask.GetMask("Ship");
        }

        protected override void UseTick()
        {
            var shipInRange = Physics.OverlapSphere(transform.position, 3f, _shipMask)
                .OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault();
            if (shipInRange != null)
            {
                shipInRange.GetComponent<ShipBehaviour>()?.Repair(Item.Strength);
            }
        }
    }
}
using System;
using OverFixed.Scripts.Game.Behaviours.Ship;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class ExtinguisherBehaviour : ItemBehaviour<Extinguisher>
    {
        private LayerMask _shipMask;

        private void Start()
        {
            _shipMask = LayerMask.GetMask("Ship");
        }

        protected override void UseTick()
        {
            if (Physics.Raycast(transform.position - transform.forward, transform.forward, out var hit, 6f, _shipMask))
            {
                hit.collider.GetComponent<ShipBehaviour>()?.Extinguish(Item.Strength);
            }
        }
    }
}
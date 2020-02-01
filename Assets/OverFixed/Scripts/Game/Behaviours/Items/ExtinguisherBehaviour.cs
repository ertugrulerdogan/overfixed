using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class ExtinguisherBehaviour : ItemBehaviour<Extinguisher>
    {
        private LayerMask _shipMask;

        private void Start()
        {
            _shipMask = LayerMask.GetMask("ShipSection");
        }

        protected override void UseTick()
        {
            if (Physics.Raycast(transform.position - transform.forward, transform.forward, out var hit, 6f, _shipMask))
            {
                var section = hit.collider.GetComponent<ShipSectionBehaviour>();
                section.ShipBehaviour.Extinguish(section.GetSection(),Item.Strength);
            }
        }
    }
}
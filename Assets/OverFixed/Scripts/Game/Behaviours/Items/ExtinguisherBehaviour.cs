using OverFixed.Scripts.Game.Behaviours.Ship;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class ExtinguisherBehaviour : ItemBehaviour<Extinguisher>
    {
        private static readonly LayerMask ShipMask = LayerMask.GetMask("Ship");
        
        protected override void UseTick()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 3f, ShipMask))
            {
                hit.collider.GetComponent<ShipBehaviour>()?.Extinguish(Item.Strength);
            }
        }
    }
}
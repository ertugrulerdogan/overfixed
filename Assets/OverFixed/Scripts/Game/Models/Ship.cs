using System.Collections.Generic;

namespace OverFixed.Scripts.Game.Models
{
    public enum ShipState
    {
        None,
        OnFire,
        Damaged,
    }
    public class Ship
    {
        public float MaxHealth;
        public float CurrentHealth;

        public ShipState State;
    }
}
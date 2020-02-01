using System.Collections.Generic;

namespace OverFixed.Scripts.Game.Models.Ships
{
    public enum ShipType
    {
        Small,
        Medium,
        Large,
    }

    public enum ShipState
    {
        OnFire,
        Smoking,
        Damaged,
        Healthy,
    }

    public class Ship
    {
        public float MaxHealth;
        public float CurrentHealth;
        public ShipState State;

        public List<Section> ShipParts = new List<Section>();
        public Platform Platform;

        public Ship(float maxHealth, float currentHealth, Platform platform)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            Platform = platform;

            for (int i = 0; i < 3; i++) //Hardcoded 3 parts for now
            {
                ShipParts.Add(new Section());
            }
        }

        public class Section
        {
            public float SmokeAmount;
            public float FireAmount;
        }
    }
}
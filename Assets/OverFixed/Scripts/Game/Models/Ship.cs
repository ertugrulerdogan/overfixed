using System.Collections.Generic;

namespace OverFixed.Scripts.Game.Models
{
    public enum ShipType
    {
        Small,
        Medium,
        Large,
    }

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
        public int ScrapAmount;
        public float SmokeTimer;
        public ShipState State;

        public Platform Platform;

        public Ship(float maxHealth, float currentHealth, ShipState state, int scrapAmount, float smokeTimer, Platform platform)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            State = state;
            ScrapAmount = scrapAmount;
            SmokeTimer = smokeTimer;
            Platform = platform;

        }
    }
}
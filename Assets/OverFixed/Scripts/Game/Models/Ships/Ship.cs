using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
        public ShipInfo Info;

        public float CurrentHealth;
        public ShipState State;

        public float SmokeRepairDuration;
        public float FireExtinguishDuration;

        [FormerlySerializedAs("ShipParts")]
        public List<Section> ShipSections = new List<Section>();
        public Platform Platform;

        public Ship(ShipInfo info, Platform platform)
        {
            Info = info;
            CurrentHealth = Random.Range(info.CurrentHealthMin, info.CurrentHealthMax);
            Platform = platform;

            for (int i = 0; i < 3; i++) //Hardcoded 3 parts for now
            {
                ShipSections.Add(new Section());
            }
        }

        public class Section
        {
            public float SmokeAmount;
            public float FireAmount;
        }
    }

    [Serializable]
    public struct ShipInfo
    {
        public ShipType ShipType;

        [Header("Health")]
        public float MaxHealth;
        public float CurrentHealthMin;
        public float CurrentHealthMax;

        [Header("ActionRelated")]
        public float SmokeDuration;
        public float BurnDamage;
        public float GrindAmount;
        public int GrindHitAmount;
        public float ExtinguisherDuration;
        public ShipState State;

        [Header("GameRelated")]
        public float DifficultyPoint;
        public float WarPointPositive;
        public float WarPointNegative;
    }
}
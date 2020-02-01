using UnityEngine;

namespace OverFixed.Scripts.Game.Models
{
    public class Platform
    {
        public bool IsPlatformOccupied;
        public Vector3 LandingPosition;
        public Vector3 SpawnPosition;

        public Platform(Vector3 landingPos, Vector3 spawnPos)
        {
            LandingPosition = landingPos;
            SpawnPosition = spawnPos;
        }

    }
}
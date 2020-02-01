using UnityEngine;

namespace OverFixed.Scripts.Game.Models.Ship
{
    public class Platform
    {
        public bool IsPlatformOccupied;
        public Vector3 LandingPosition;
        public Vector3 SpawnPosition;
        public Quaternion SpawnRotation;

        public Platform(Vector3 landingPos, Vector3 spawnPos, Quaternion spawnRotation)
        {
            LandingPosition = landingPos;
            SpawnPosition = spawnPos;
            SpawnRotation = spawnRotation;
        }

    }
}
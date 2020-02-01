using System.Collections.Generic;
using OverFixed.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours
{
    public class PlatformBehaviour : MonoBehaviour
    {
        public Platform Platform;

        public Transform LandingTransform;
        public Transform SpawnTransform;

    }
}
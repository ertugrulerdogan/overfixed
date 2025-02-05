﻿using System;
using UnityEngine;

namespace OverFixed.Scripts.Game.Views.Ships
{
    public class ShipSectionView : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _fireParticles;
        [SerializeField]
        private ParticleSystem _smokeParticles;

        private void Awake()
        {
            var emission = _fireParticles.emission;
            emission.rateOverTimeMultiplier = 0f;
        }

        public void SetFireStrength(float strength)
        {
            var emission = _fireParticles.emission;
            emission.rateOverTimeMultiplier = strength;
        }

        public void SetSmoke(bool emit)
        {
            var emission = _smokeParticles.emission;
            emission.enabled = emit;
        }
    }
}
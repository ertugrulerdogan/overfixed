﻿using System;
using DG.Tweening;
using OverFixed.Scripts.Game.Models;
using OverFixed.Scripts.Game.Models.Ship;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Ship
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Models.Ship.Ship Ship;

        public void Land()
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition + Vector3.one * 3f, 5f));
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition, 2f));
        }

        public void TakeOff(Action onComplete)
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition + Vector3.one, 3f));
            seq.Append(transform.DORotateQuaternion(Quaternion.LookRotation(Ship.Platform.SpawnPosition - Ship.Platform.LandingPosition), 2f));
            seq.Append(transform.DOMove(Ship.Platform.SpawnPosition, 5f));
            seq.OnComplete(() =>
            {
                Ship.Platform.IsPlatformOccupied = false;
                onComplete?.Invoke();
            });
        }

        public void Repair(float amount)
        {
            Ship.CurrentHealth = Mathf.Clamp(Ship.CurrentHealth + amount * Time.deltaTime, 0, Ship.MaxHealth);
        }

        public float Scrap(float amount)
        {
            var initialAmount = Ship.CurrentHealth;
            Ship.CurrentHealth = Mathf.Clamp(Ship.CurrentHealth - amount * Time.deltaTime, 0f, Ship.MaxHealth);
            return initialAmount - Ship.CurrentHealth;
        }

        public void Extinguish(float amount)
        {
            //extinguish logic here
        }

        private void Update()
        {
            switch (Ship.State)
            {
                case ShipState.None:
                    break;
                case ShipState.OnFire:
                    break;
                case ShipState.Smoking:
                    break;
                case ShipState.Damaged:
                    break;
                case ShipState.Healthy:
                    break;
            }
        }

        public class Pool : MonoMemoryPool<ShipBehaviour>
        {

        }
    }
}

using System;
using System.Linq;
using DG.Tweening;
using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace OverFixed.Scripts.Game.Behaviours.Ship
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Models.Ships.Ship Ship;
        private Pool _pool;


        private bool _isMoving;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }

        public void Init(ShipState state)
        {
            Ship.State = state;

            foreach (var part in Ship.ShipParts)
            {
                part.FireAmount = 0f;
                part.SmokeAmount = 0f;
            }

            var selectedPart = Ship.ShipParts[Random.Range(0, Ship.ShipParts.Count)];

            switch (state)
            {
                case ShipState.OnFire:
                    StartFire(selectedPart);
                    break;
                case ShipState.Smoking:
                    StartSmoke(selectedPart);
                    break;
                case ShipState.Damaged:
                    break;
                case ShipState.Healthy:
                    break;
            }
        }

        #region Movement

        public void Land()
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition + Vector3.up * 3f, 5f));
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition, 2f));
            seq.OnComplete(() => { _isMoving = false; });
            _isMoving = true;
        }

        private void TakeOff(Action onComplete)
        {
            if (_isMoving)
            {
                return;
            }

            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition + Vector3.up * 3f, 3f));
            seq.Append(transform.DORotateQuaternion(Quaternion.LookRotation(Ship.Platform.SpawnPosition - Ship.Platform.LandingPosition), 3f));
            seq.AppendInterval(1f);
            seq.Append(transform.DOMove(Ship.Platform.SpawnPosition, 2f));
            seq.OnComplete(() =>
            {
                Ship.Platform.IsPlatformOccupied = false;
                _isMoving = false;
                onComplete?.Invoke();
            });

            _isMoving = true;
        }

        #endregion

        #region Actions

        public void Repair(float amount)
        {
            if (_isMoving)
            {
                return;
            }

            if (Ship.ShipParts.Any(x => x.FireAmount > 0))
            {
                Hurt(1f);
            }
            else if (Ship.ShipParts.Any(x => x.SmokeAmount > 0))
            {
                foreach (var part in Ship.ShipParts)
                {
                    part.SmokeAmount -= Time.deltaTime;
                }

                if (Ship.ShipParts.All(p => p.SmokeAmount < 0))
                {
                    Ship.State = ShipState.Damaged;
                }
                
                Heal(amount);
            }
            else
            {
                Heal(amount);
            }
        }

        public float Scrap(float amount)
        {
            var initialAmount = Ship.CurrentHealth;
            
            if (!_isMoving)
            {
                Hurt(amount);
            }

            return initialAmount - Ship.CurrentHealth;
        }

        public void Extinguish(float amount)
        {
            if(!_isMoving)
            {
                //Ship.CurrentHealth = Mathf.Clamp(Ship.CurrentHealth - amount * Time.deltaTime, 0f, Ship.MaxHealth);
            }
            //extinguish logic here
        }

        #endregion

        private void Update()
        {
            switch (Ship.State)
            {
                case ShipState.Damaged:
                    break;
                case ShipState.OnFire:
                    ProcessFire();
                    break;
                case ShipState.Smoking:
                    ProcessSmoke();
                    break;
                case ShipState.Healthy:
                    TakeOff(Destruct);
                    break;
            }

            if (Ship.ShipParts.Any(p => p.FireAmount > 0f))
            {
                Hurt(1f); //change later
            }

            if (Ship.CurrentHealth < 0.1f)
            {
                Destruct();
            }
        }

        private void Hurt(float amount)
        {
            Ship.CurrentHealth = Mathf.Clamp(Ship.CurrentHealth - amount * Time.deltaTime, 0f, Ship.MaxHealth);
        }

        private void Heal(float amount)
        {
            Ship.CurrentHealth = Mathf.Clamp(Ship.CurrentHealth + amount * Time.deltaTime, 0, Ship.MaxHealth);

            if (Ship.CurrentHealth >= Ship.MaxHealth)
            {
                Ship.State = ShipState.Healthy;
            }
        }

        private void ProcessSmoke()
        {
            for (var i = 0; i < Ship.ShipParts.Count; i++)
            {
                var part = Ship.ShipParts[i];
                if (part.SmokeAmount > 0.1f)
                {
                    part.SmokeAmount += Time.deltaTime;
                }

                if (part.SmokeAmount >= 10f)
                {
                    StartFire(part);
                    Ship.State = ShipState.OnFire;
                    part.SmokeAmount = 0f;
                }
            }
        }

        private void ProcessFire()
        {
            foreach (var part in Ship.ShipParts)
            {
                if (part.FireAmount > 0)
                {
                    part.FireAmount =  Mathf.Clamp(part.FireAmount + 0.25f * Time.deltaTime, 0f, 10f); //TODO change multiplier later 
                }         
            }
        }

        private void StartFire(Models.Ships.Ship.Section section)
        {
            section.FireAmount = 1f; // change later
        }

        private void StartSmoke(Models.Ships.Ship.Section section)
        {
           section.SmokeAmount = 1f; //change later
        }

        private void Destruct()
        {
            Ship.Platform.IsPlatformOccupied = false;
            _isMoving = false;
            _pool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<ShipBehaviour>
        {

        }
    }
}

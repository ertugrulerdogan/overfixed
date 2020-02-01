using System;
using System.Linq;
using DG.Tweening;
using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace OverFixed.Scripts.Game.Behaviours.Ships
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Ship Ship;
        private Pool _pool;
        private bool _isMoving;
        public float AfterburnerAmount;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }

        public void Init(ShipState state)
        {
            Ship.State = state;

            foreach (var part in Ship.ShipSections)
            {
                part.FireAmount = 0f;
                part.SmokeAmount = 0f;
            }

            var selectedPart = Ship.ShipSections[Random.Range(0, Ship.ShipSections.Count)];

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
            AfterburnerAmount = 0.5f;
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition + Vector3.up * 3f, 5f));
            seq.Append(transform.DOMove(Ship.Platform.LandingPosition, 2f));
            seq.Join(DOTween.To(() => AfterburnerAmount, x => AfterburnerAmount = x, 0, 2));
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
            seq.AppendInterval(0.5f);
            seq.Join(DOTween.To(() => AfterburnerAmount, x => AfterburnerAmount = x, 1, 2));
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

        public float Repair(float amount)
        {
            var initialHealth = Ship.CurrentHealth;

            if (_isMoving)
            {
                return 0f;
            }

            if (Ship.ShipSections.Any(x => x.FireAmount > 0))
            {
                Hurt(1f);
            }
            else if (Ship.ShipSections.Any(x => x.SmokeAmount > 0))
            {
                foreach (var part in Ship.ShipSections)
                {
                    part.SmokeAmount -= Time.deltaTime;
                }

                if (Ship.ShipSections.All(p => p.SmokeAmount < 0) && Ship.State == ShipState.Smoking)
                {
                    Ship.State = ShipState.Damaged;
                }
            }
            else
            {
                Heal(amount);
            }

            return Ship.CurrentHealth - initialHealth;
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

        public void Extinguish(Ship.Section section, float amount)
        {
            if(!_isMoving)
            {
                section.FireAmount = Mathf.Clamp(section.FireAmount - amount * Time.deltaTime, 0f, float.MaxValue);

                if (Ship.ShipSections.All(s => s.FireAmount <= 0 && s.SmokeAmount <= 0 && Ship.State == ShipState.OnFire))
                {
                    Ship.State = ShipState.Damaged;
                }
            }
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

            if (Ship.ShipSections.Any(p => p.FireAmount > 0f))
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
            for (var i = 0; i < Ship.ShipSections.Count; i++)
            {
                var part = Ship.ShipSections[i];
                if (part.SmokeAmount > 0.1f)
                {
                    part.SmokeAmount += Time.deltaTime * 0.25f;
                }

                if (part.SmokeAmount >= 5f)
                {
                    StartFire(part);
                    Ship.State = ShipState.OnFire;
                    part.SmokeAmount = 0f;
                }
            }
        }

        private void ProcessFire()
        {
            foreach (var part in Ship.ShipSections)
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
           section.SmokeAmount = 0.5f; //change later
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

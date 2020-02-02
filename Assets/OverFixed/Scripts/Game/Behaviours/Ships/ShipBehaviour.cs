using System;
using System.Linq;
using DG.Tweening;
using OverFixed.Scripts.Game.Behaviours.Explosion;
using OverFixed.Scripts.Game.Behaviours.Hittables;
using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace OverFixed.Scripts.Game.Behaviours.Ships
{
    public class ShipBehaviour : MonoBehaviour, IHittable
    {
        public static event Action<ShipBehaviour> OnBurn;
        public static event Action<ShipBehaviour> OnSmoke;
        public static event Action<ShipBehaviour> OnExploded;
        public static event Action<ShipBehaviour> OnTakeOff;
        public static event Action<ShipBehaviour> OnLand;
        public static event Action<ShipBehaviour> OnDespawned;
        
        public Ship Ship;
        private Pool _pool;
        private ExplosionBehaviour.Pool _explosionPool;
        private bool _isMoving;

        private Action<float> _resultAction;

        public float AfterburnerAmount;

        [Inject]
        public void Initialize(ExplosionBehaviour.Pool explosionPool, Pool pool)
        {
            _pool = pool;
            _explosionPool = explosionPool;
        }

        public void Init(ShipState state, Action<float> resultAction)
        {
            Ship.State = state;

            _resultAction = resultAction;
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
            
            OnLand?.Invoke(this);            
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
                _resultAction?.Invoke(Ship.Info.WarPointPositive);
                OnTakeOff?.Invoke(this);
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
                    Ship.SmokeRepairDuration += Time.deltaTime;
                }

                if (Ship.SmokeRepairDuration >= Ship.Info.SmokeDuration && Ship.State == ShipState.Smoking)
                {
                    foreach (var section in Ship.ShipSections)
                    {
                        section.SmokeAmount = 0;
                    }

                    Ship.State = ShipState.Damaged;
                }
            }
            else
            {
                Heal(30); //hardcoded for now
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

            return (initialAmount - Ship.CurrentHealth) * 5f;
        }

        public void Extinguish(Ship.Section section, float amount)
        {
            if(!_isMoving)
            {
                if (section.FireAmount > 0)
                {
                    Ship.FireExtinguishDuration += Time.deltaTime;
                }

                if (Ship.FireExtinguishDuration >= Ship.Info.ExtinguisherDuration && Ship.State == ShipState.OnFire)
                {
                    foreach (var s in Ship.ShipSections)
                    {
                        s.FireAmount = 0;
                    }

                    Ship.State = ShipState.Damaged;
                }
            }
        }

        #endregion
         
        private void LateUpdate()
        {
            if (_isMoving)
            {
                return;
            }

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
                Hurt(Ship.Info.BurnDamage); //change later
            }

            if (Ship.CurrentHealth < 0.1f)
            {
                Destruct();
                
                SpawnExplosion();

                _resultAction?.Invoke(-Ship.Info.WarPointNegative);
            }
        }

        private void SpawnExplosion()
        {
            var explosion = _explosionPool.Spawn();
            explosion.transform.localScale = Vector3.one * 3f;
            explosion.transform.position = transform.position;
        }

        private void Hurt(float amount)
        {
            Ship.CurrentHealth = Mathf.Clamp(Ship.CurrentHealth - amount * Time.deltaTime, 0f, Ship.Info.MaxHealth);
        }

        private void Heal(float amount)
        {
            Ship.CurrentHealth = Mathf.Clamp(Ship.CurrentHealth + amount * Time.deltaTime, 0, Ship.Info.MaxHealth);

            if (Ship.CurrentHealth >= Ship.Info.MaxHealth)
            {
                Ship.State = ShipState.Healthy;
            }
        }

        private void ProcessSmoke()
        {
            if (_isMoving)
            {
                return;
            }

            for (var i = 0; i < Ship.ShipSections.Count; i++)
            {
                var part = Ship.ShipSections[i];
                if (part.SmokeAmount > 0f)
                {
                    part.SmokeAmount += Time.deltaTime;
                }

                if (part.SmokeAmount >= Ship.Info.SmokeDuration)
                {
                    StartFire(part);
                    Ship.State = ShipState.OnFire;
                    part.SmokeAmount = 0f;
                }
            }
        }

        private void ProcessFire()
        {
            if (_isMoving)
            {
                return;
            }

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
            
            OnBurn?.Invoke(this);
        }

        private void StartSmoke(Models.Ships.Ship.Section section)
        {
           section.SmokeAmount = 0.1f; //change later
            
            OnSmoke?.Invoke(this);
        }

        private void Destruct()
        {
            Ship.Platform.IsPlatformOccupied = false;
            _isMoving = false;
            
            OnExploded?.Invoke(this);
            
            _pool.Despawn(this);
        }

        public void Hit(float damage)
        {
            Ship.CurrentHealth -= damage;
        }

        public class Pool : MonoMemoryPool<ShipBehaviour>
        {
            protected override void OnDespawned(ShipBehaviour item)
            {
                base.OnDespawned(item);
                
                ShipBehaviour.OnDespawned?.Invoke(item);
            }
        }
    }
}


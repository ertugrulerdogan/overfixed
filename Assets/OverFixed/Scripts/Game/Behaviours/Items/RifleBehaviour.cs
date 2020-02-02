using OverFixed.Scripts.Game.Behaviours.Bullets;
using OverFixed.Scripts.Game.Models.Bullets;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class RifleBehaviour : ItemBehaviour<Rifle>
    {
        private BulletBehaviour.Pool _bulletPool;
        private float _lastFireTime;

        [Inject]
        public void Initialize(BulletBehaviour.Pool bulletPool)
        {
            _bulletPool = bulletPool;
        }
        
        protected override void UseTick()
        {
            var time = Time.time;
            if (time - _lastFireTime > Item.FirePeriod)
            {
                _lastFireTime = time;
                Item.Ammo--;
                var bullet = _bulletPool.Spawn();
                bullet.Bind(new Bullet(Item.Damage));
                bullet.Fire(transform.position + transform.forward, transform.rotation);
            }
        }
    }
}
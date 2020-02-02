using System;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public abstract class ItemBehaviour<TItem> : ItemBehaviourBase where TItem : Item
    {
        public TItem Item { get; private set; }
        public override Item BoundItem => Item;
        public Transform Transform => transform;

        private Pool _pool;

        [Inject]
        public void Initialize(Pool pool)
        {
            _pool = pool;
        }

        public void Bind(TItem item)
        {
            Item = item;
        }

        private void Update()
        {
            if (Item != null && Item.Using)
            {
                UseTick();
            }
        }

        public override void Drop()
        {
            _pool.Despawn(this);
        }

        protected abstract void UseTick();

        public class Pool : MonoMemoryPool<ItemBehaviour<TItem>>
        {
            protected override void OnCreated(ItemBehaviour<TItem> item)
            {
                base.OnCreated(item);
                item.Bind(Activator.CreateInstance<TItem>());
            }
        }
    }
}
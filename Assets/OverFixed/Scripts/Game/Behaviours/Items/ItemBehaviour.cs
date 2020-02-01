using System;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public abstract class ItemBehaviour<TItem> : MonoBehaviour where TItem : Item
    {
        public TItem Item { get; private set; }

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

        protected abstract void UseTick();
    }
}
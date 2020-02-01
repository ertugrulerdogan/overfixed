using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Views.Items
{
    public abstract class ItemView<TItem> : MonoBehaviour where TItem : Item
    {
        public TItem Item { get; private set; }
        
        public void Start()
        {
            Item = GetComponent<ItemBehaviour<TItem>>().Item;
        }
    }
}
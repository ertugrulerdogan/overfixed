using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class ItemView<TItem> : MonoBehaviour where TItem : Item
    {
        public TItem Item { get; private set; }

        public void Bind(TItem item)
        {
            
        }
    }
}
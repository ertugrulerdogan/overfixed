using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public abstract class ItemBehaviourBase : MonoBehaviour
    {
        public abstract Item BoundItem { get; }
        public virtual void Drop() { }
    }
}
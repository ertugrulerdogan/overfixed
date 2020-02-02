using System;
using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.ItemDock
{
    public class ItemDockBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform[] _slots;
        private Item[] _items;
        private Func<ItemBehaviourBase>[] _spawnFunctions;
        
        [Inject]
        public void Initialize(ItemBehaviour<Cutter>.Pool cutterPool, ItemBehaviour<Wrench>.Pool wrenchPool,
            ItemBehaviour<Rifle>.Pool riflePool, ItemBehaviour<Extinguisher>.Pool extinguisherPool)
        {
            _spawnFunctions = new Func<ItemBehaviourBase>[]
            {
                cutterPool.Spawn,
                wrenchPool.Spawn,
                extinguisherPool.Spawn,
                riflePool.Spawn
            };

            _items = new Item[_slots.Length];
        }

        private void Update()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                if (_items[i]?.Equipped ?? true)
                {
                    var item = _spawnFunctions[i]();
                    item.transform.position = _slots[i].position;
                    item.transform.rotation = _slots[i].rotation;
                    _items[i] = item.BoundItem;
                }
            }
        }
    }
}
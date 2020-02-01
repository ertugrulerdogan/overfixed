using OverFixed.Scripts.Game.Behaviours.Scraps;
using OverFixed.Scripts.Game.Behaviours.Ship;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Items
{
    public class CutterBehaviour : SpherecastItemBehaviour<Cutter>
    {
        private ScrapSpawner _scrapSpawner;
        
        [Inject]
        public void Initialize(ScrapSpawner scrapSpawner)
        {
            _scrapSpawner = scrapSpawner;
        }
        
        protected override void OnHit(ShipBehaviour shipBehaviour)
        {
            var scrap = Mathf.RoundToInt(shipBehaviour.Scrap(Item.Strength));
            _scrapSpawner.Scatter(transform.position, scrap);
        }
    }
}
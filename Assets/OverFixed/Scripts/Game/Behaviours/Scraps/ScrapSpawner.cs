using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Scraps
{
    public class ScrapSpawner
    {
        private ScrapBehaviour.Pool _scrapPool;
        
        public ScrapSpawner(ScrapBehaviour.Pool scrapPool)
        {
            _scrapPool = scrapPool;
        }

        public void Scatter(Vector3 position, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var scrap = _scrapPool.Spawn();
                scrap.Rigidbody.velocity += Vector3.up + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            }
        }
    }
}
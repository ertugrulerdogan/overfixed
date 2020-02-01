using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Scraps
{
    public class ScrapSpawner
    {
        private ScrapBehaviour.Pool _scrapPool;
        private float _accumulated;
        
        public ScrapSpawner(ScrapBehaviour.Pool scrapPool)
        {
            _scrapPool = scrapPool;
        }

        public void Scatter(Vector3 position, float amount)
        {
            _accumulated += amount;
            var curRelease = Mathf.FloorToInt(_accumulated / ScrapBehaviour.ScrapContribution);
            for (int i = 0; i < curRelease; i++)
            {
                var scrap = _scrapPool.Spawn();
                scrap.transform.position = position;
                scrap.Rigidbody.velocity += Vector3.up + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            }

            _accumulated -= curRelease * ScrapBehaviour.ScrapContribution;
        }
    }
}
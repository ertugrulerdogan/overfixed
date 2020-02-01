using OverFixed.Scripts.Game.Behaviours.Scraps;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers
{
    public class TestController : ITickable
    {
        private readonly ScrapSpawner _scrapSpawner;

        public TestController(ScrapSpawner scrapSpawner)
        {
            _scrapSpawner = scrapSpawner;
        }
        
        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                _scrapSpawner.Scatter(Vector3.zero, 10);
            }
        }
    }
}
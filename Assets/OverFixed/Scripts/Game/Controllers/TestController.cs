using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Behaviours.Scraps;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers
{
    public class TestController : ITickable
    {
        private readonly ScrapSpawner _scrapSpawner;
        private Rifle _rifle;

        public TestController(ScrapSpawner scrapSpawner)
        {
            _scrapSpawner = scrapSpawner;
            _rifle = new Rifle();
            // Object.FindObjectOfType<RifleBehaviour>().Bind(_rifle);
        }
        
        public void Tick()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                _rifle.Using = true;
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.T))
            {
                _rifle.Using = false;
            }
        }
    }
}
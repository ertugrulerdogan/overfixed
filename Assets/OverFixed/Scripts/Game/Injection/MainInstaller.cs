using OverFixed.Scripts.Game.Behaviours;
using OverFixed.Scripts.Game.Behaviours.Bullets;
using OverFixed.Scripts.Game.Behaviours.Scraps;
using OverFixed.Scripts.Game.Controllers;
using OverFixed.Scripts.Game.Models;
using OverFixed.Scripts.Game.Models.Data;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Injection
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameObject _scrap;
        [SerializeField] private GameObject _ship;
        [SerializeField] private GameObject _bullet;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_mainCamera).AsSingle().NonLazy();
            
            Container.Bind<TeamData>().AsSingle();
            Container.BindMemoryPool<ScrapBehaviour, ScrapBehaviour.Pool>().WithInitialSize(100).FromComponentInNewPrefab(_scrap);
            Container.Bind<ScrapSpawner>().AsSingle();

            Container.BindMemoryPool<ShipBehaviour, ShipBehaviour.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_ship);
            Container.BindInterfacesAndSelfTo<TestController>().AsSingle();
            Container.BindMemoryPool<BulletBehaviour, BulletBehaviour.Pool>().WithInitialSize(100).FromComponentInNewPrefab(_bullet);
        }
    }
}

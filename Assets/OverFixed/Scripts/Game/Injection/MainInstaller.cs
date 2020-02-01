using OverFixed.Scripts.Game.Behaviours;
using OverFixed.Scripts.Game.Controllers.Camera;
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
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private GameObject _scrap;
        [SerializeField] private GameObject _ship;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_cameraController).AsSingle().NonLazy();
            Container.Bind<TeamData>().AsSingle();
            Container.BindMemoryPool<ScrapBehaviour, ScrapBehaviour.Pool>().WithInitialSize(100).FromComponentInNewPrefab(_scrap);
            Container.Bind<ScrapSpawner>().AsSingle();
            Container.Bind<Hangar>().AsSingle();
            Container.Bind<Ship>().AsSingle();
            Container.Bind<HangarBehaviour>().AsSingle();
            Container.BindMemoryPool<ShipBehaviour, ShipBehaviour.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_ship);
            Container.BindInterfacesAndSelfTo<TestController>().AsSingle();

        }
    }
}

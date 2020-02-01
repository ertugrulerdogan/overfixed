using OverFixed.Scripts.Game.Behaviours.Bullets;
using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Behaviours.Scraps;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Controllers;
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
        [SerializeField] private UIManager _uiManager;
        
        [SerializeField] private RifleBehaviour _rifleBehaviour;
        [SerializeField] private ExtinguisherBehaviour _extinguisherBehaviour;
        [SerializeField] private WrenchBehaviour _wrenchBehaviour;
        [SerializeField] private CutterBehaviour _cutterBehaviour;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainCamera).AsSingle().NonLazy();
            
            Container.Bind<TeamData>().AsSingle();
            Container.BindMemoryPool<ScrapBehaviour, ScrapBehaviour.Pool>().WithInitialSize(100).FromComponentInNewPrefab(_scrap);
            Container.Bind<ScrapSpawner>().AsSingle();
            Container.BindInstance(_uiManager).AsSingle();

            Container.BindMemoryPool<ShipBehaviour, ShipBehaviour.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_ship);
            Container.BindInterfacesAndSelfTo<TestController>().AsSingle();
            Container.BindMemoryPool<BulletBehaviour, BulletBehaviour.Pool>().WithInitialSize(100).FromComponentInNewPrefab(_bullet);
            Container.BindInstance(_rifleBehaviour).AsSingle();
            Container.BindInstance(_extinguisherBehaviour).AsSingle();
            Container.BindInstance(_wrenchBehaviour).AsSingle();
            Container.BindInstance(_cutterBehaviour).AsSingle();
            Container.Bind<ItemController>().AsSingle().NonLazy();
            Container.BindInstance(new ItemBehaviourBase[] {_wrenchBehaviour, _extinguisherBehaviour, _rifleBehaviour, _cutterBehaviour});
        }
    }
}

using System.Collections.Generic;
using OverFixed.Scripts.Game.Behaviours.Bullets;
using OverFixed.Scripts.Game.Behaviours.Drones;
using OverFixed.Scripts.Game.Behaviours.Explosion;
using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Behaviours.Scraps;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Controllers;
using OverFixed.Scripts.Game.Models.Data;
using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Injection
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private AudioController _audioController;
        
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameObject _scrap;
        [SerializeField] private GameObject _ship;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private GameObject _explosion;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private PlatformBehaviour[] _platformBehaviours;
        
        [SerializeField] private GameObject _rifle;
        [SerializeField] private GameObject _extinguisher;
        [SerializeField] private GameObject _wrench;
        [SerializeField] private GameObject _cutter;
        [SerializeField] private GameObject _scatter;

        [SerializeField] private DroneBehaviour _drone;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_audioController).AsSingle().NonLazy();
            Container.BindInstance(_mainCamera).AsSingle().NonLazy();
            
            Container.Bind<TeamData>().AsSingle();
            Container.BindMemoryPool<ScrapBehaviour, ScrapBehaviour.Pool>().WithInitialSize(100).FromComponentInNewPrefab(_scrap);
            Container.Bind<ScrapSpawner>().AsSingle();
            Container.BindInstance(_uiManager).AsSingle();

            Container.BindMemoryPool<ShipBehaviour, ShipBehaviour.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_ship);
            Container.BindMemoryPool<ExplosionBehaviour, ExplosionBehaviour.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_explosion);
            Container.BindInterfacesAndSelfTo<TestController>().AsSingle();
            Container.BindMemoryPool<BulletBehaviour, BulletBehaviour.Pool>().WithInitialSize(100).FromComponentInNewPrefab(_bullet);
            Container.BindMemoryPool<DroneBehaviour, DroneBehaviour.Pool>().WithInitialSize(20).FromComponentInNewPrefab(_drone);
            Container.BindInstance<IList<PlatformBehaviour>>(_platformBehaviours).AsSingle();
            Container.BindMemoryPool<ItemBehaviour<Extinguisher>, ItemBehaviour<Extinguisher>.Pool>().WithInitialSize(20).FromComponentInNewPrefab(_extinguisher);
            Container.BindMemoryPool<ItemBehaviour<Wrench>, ItemBehaviour<Wrench>.Pool>().WithInitialSize(4).FromComponentInNewPrefab(_wrench);
            Container.BindMemoryPool<ItemBehaviour<Rifle>, ItemBehaviour<Rifle>.Pool>().WithInitialSize(4).FromComponentInNewPrefab(_rifle);
            Container.BindMemoryPool<ItemBehaviour<Cutter>, ItemBehaviour<Cutter>.Pool>().WithInitialSize(4).FromComponentInNewPrefab(_cutter);
            Container.BindFactory<ParticleSystem, PlaceholderFactory<ParticleSystem>>().FromComponentInNewPrefab(_scatter);
        }
    }
}

using OverFixed.Scripts.Game.Controllers.Camera;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Injection
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private CameraController _cameraController;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_cameraController).AsSingle().NonLazy();
        }
    }
}
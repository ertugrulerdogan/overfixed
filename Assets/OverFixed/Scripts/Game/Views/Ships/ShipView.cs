using System;
using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace OverFixed.Scripts.Game.Views.Ships
{
    public class ShipView : MonoBehaviour
    {
        private Ship _ship;

        private UIManager _uiManager;

        [SerializeField]
        private List<ShipMeshData> _meshData;
        [SerializeField]
        private List<ShipTextureData> _textureData;

        [SerializeField]
        private List<ShipSectionView> _shipPartViews;

        private ShipBehaviour _shipBehaviour;

        public AfterBurner AfterBurner;

        private float _timer;

        private NewStatusBar _statusBar;

        [Inject]
        public void Initialize(UIManager manager)
        {
            _uiManager = manager;
        }

        public void Awake()
        {
            _shipBehaviour = GetComponent<ShipBehaviour>();
            var statusPrefab = Resources.Load<GameObject>("UI/ShipStatsContainer");
            if (statusPrefab != null)
            {
                var go = Instantiate(statusPrefab, _uiManager.BarParent);
                if (go != null)
                {
                    _statusBar = go.GetComponent<NewStatusBar>();
                    _statusBar.Bind(_shipBehaviour);
                    _statusBar.gameObject.SetActive(false);
                }
            }
        }
        
        public void Bind(Ship ship)
        {
            _ship = ship;

            SetShipMeshAndTexture(ship);
        }

        private void SetShipMeshAndTexture(Ship ship)
        {
            foreach (var data in _meshData)
            {
                data.Renderer.gameObject.SetActive(false);
            }

            var meshData = _meshData.FirstOrDefault(m => m.Type == ship.Info.ShipType);
            if (meshData != null)
            {
                meshData.Renderer.gameObject.SetActive(true);

                var material = _textureData.FirstOrDefault(t => t.Type == ship.Info.ShipType);
                if (material != null)
                {
                    meshData.Renderer.material = material.Materials[Random.Range(0, material.Materials.Count)];
                }
            }
        }

        private void OnEnable()
        {
            _statusBar.gameObject.SetActive(true);
        }

        private float _initialExtinguishDuration;
        private float _initialSmokeDuration;

        private void Update()
        {
            _timer -= Time.deltaTime;

            AfterBurner.SetThrustAmount(_shipBehaviour.AfterburnerAmount);
            // _bar.UpdateHealth(_ship.CurrentHealth, _ship.Info.MaxHealth);

            for (var i = 0; i < _ship.ShipSections.Count; i++)
            {
                _shipPartViews[i].SetFireStrength(_ship.ShipSections[i].FireAmount);
                _shipPartViews[i].SetSmoke(_ship.ShipSections[i].SmokeAmount > 0f);
            }

            if (Math.Abs(_initialExtinguishDuration - _ship.FireExtinguishDuration) > 0.1f &&_ship.FireExtinguishDuration > 0) //TODO refactor later
            {
                _timer = 2f;
                _initialExtinguishDuration = _ship.FireExtinguishDuration;
                // _bar.UpdateFireExtinguish(_ship.FireExtinguishDuration / _ship.Info.ExtinguisherDuration);
            }

            if (Math.Abs(_initialSmokeDuration - _ship.SmokeRepairDuration) > 0.1f && _ship.SmokeRepairDuration > 0)
            {
                _timer = 2f;
                _initialSmokeDuration = _ship.SmokeRepairDuration;
                // _bar.UpdateSmokeRepair(_ship.SmokeRepairDuration / _ship.Info.SmokeDuration);
            }

            if (_timer < 0f)
            {
                // _bar.HideFireAndSmoke();
            }
        }

        private void OnDisable()
        {
            // if (_bar != null)
            // {
            //     _bar.gameObject.SetActive(false);
            // }
            if (_statusBar) _statusBar.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class ShipMeshData
    {
        public ShipType Type;
        public MeshRenderer Renderer;
    }

    [Serializable]
    public class ShipTextureData
    {
        public ShipType Type;
        public List<Material> Materials;
    }

}

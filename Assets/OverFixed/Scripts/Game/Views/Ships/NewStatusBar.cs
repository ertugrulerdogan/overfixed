using System;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;
using UnityEngine.UI;

namespace OverFixed.Scripts.Game.Views.Ships
{
    public class NewStatusBar : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _smokeSlider;
        [SerializeField] private Image[] _stateImages;
        private ShipBehaviour _ship;
        private RectTransform _rectTransform;
        private Camera _camera;

        private void Awake()
        {
            _rectTransform = (RectTransform) transform;
            _camera = Camera.main;
        }

        public void Bind(ShipBehaviour ship)
        {
            _ship = ship;
        }

        private void Update()
        {
            _healthSlider.value = _ship.Ship.CurrentHealth / _ship.Ship.Info.MaxHealth;
            for (int i = 0; i < _stateImages.Length; i++)
            {
                _stateImages[i].gameObject.SetActive(false);
            }
            
            _stateImages[(int) _ship.Ship.State].gameObject.SetActive(true);

            float smokeFireFill;
            switch (_ship.Ship.State)
            {
                case ShipState.OnFire:
                    smokeFireFill = 1 - _ship.Ship.FireExtinguishDuration / _ship.Ship.Info.ExtinguisherDuration;
                    break;
                case ShipState.Smoking:
                    smokeFireFill = 1 - _ship.Ship.SmokeRepairDuration / _ship.Ship.Info.SmokeDuration;
                    break;
                default:
                    smokeFireFill = 0f;
                    break;
            }

            _smokeSlider.value = smokeFireFill;
            _rectTransform.anchoredPosition = _camera.WorldToScreenPoint(_ship.transform.position  - _ship.transform.forward * 5f + Vector3.up * 2f);
        }
    }
}
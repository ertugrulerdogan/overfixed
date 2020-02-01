using System;
using UnityEngine;
using UnityEngine.UI;

namespace OverFixed.Scripts.Game.Views.Ships
{
    public class StatusBar : MonoBehaviour
    {
        [SerializeField]
        private Image _healthFill;
        [SerializeField]
        private Text _healthText;

        private ShipView _shipView;
        private Camera _camera;

        private RectTransform _rect;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Init(ShipView view)
        {
            _shipView = view;
            _rect = (RectTransform) transform;
        }

        private void Update()
        {
            SetScreenPosition();
        }

        public void UpdateHealth(float current, float max)
        {
            _healthFill.fillAmount = current / max;
            _healthText.text = $"{current:N0}/{max:N0}";
        }

        private void OnEnable()
        {
            SetScreenPosition();
        }

        private void SetScreenPosition()
        {
            if (_rect != null)
            {
                _rect.anchoredPosition = _camera.WorldToScreenPoint(_shipView.transform.position);
            }
        }
    }
}
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
        private Image _fireExtinguishFill;
        [SerializeField]
        private Image _smokeRepairFill;

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

        public void UpdateSmokeRepair(float amount)
        {
            _smokeRepairFill.transform.parent.gameObject.SetActive(true);
            _smokeRepairFill.fillAmount = amount;
        }

        public void UpdateFireExtinguish(float amount)
        {
            _fireExtinguishFill.transform.parent.gameObject.SetActive(true);
            _fireExtinguishFill.fillAmount = amount;
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

        public void HideFireAndSmoke()
        {
            _smokeRepairFill.transform.parent.gameObject.SetActive(false);
            _fireExtinguishFill.transform.parent.gameObject.SetActive(false);
        }
    }
}
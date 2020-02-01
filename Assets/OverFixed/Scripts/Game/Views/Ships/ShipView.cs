using OverFixed.Scripts.Game.Models.Ships;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Views.Ships
{
    public class ShipView : MonoBehaviour
    {
        private Ship _ship;
        private StatusBar _bar;

        private UIManager _uiManager;

        [Inject]
        public void Initialize(UIManager manager)
        {
            _uiManager = manager;
        }

        public void Awake()
        {
            var statusPrefab = Resources.Load<GameObject>("UI/StatusBar");
            if (statusPrefab != null)
            {
                var go = Instantiate(statusPrefab, _uiManager.BarParent);
                if (go != null)
                {
                    _bar = go.GetComponentInChildren<StatusBar>(true);
                    _bar.Init(this);
                }
            }
        }
        
        public void Bind(Ship ship) 
        { 
           _ship = ship;
            if (_bar != null)
            {
                _bar.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            _bar.UpdateHealth(_ship.CurrentHealth, _ship.MaxHealth);
        }

        private void OnDisable()
        {
            if (_bar != null)
            {
                _bar.gameObject.SetActive(false);
            }
        }
    }
}

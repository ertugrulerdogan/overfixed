using System;
using OverFixed.Scripts.Game.Models.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace OverFixed.Scripts.Game.Views
{
    public class GameView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _friendlyFillImage;
        [SerializeField]
        private RectTransform _enemyFillImage;
        [SerializeField]
        private RectTransform _midBarImage;
        [SerializeField] 
        private Text _friendlyStatusText;
        [SerializeField]
        private Text _enemyStatusText;

        private TeamData _teamData;

        [Inject]
        public void Initialize(TeamData teamData)
        {
            _teamData = teamData;
        }

        private void Update()
        {
            var winRatio = _teamData.WarStatus / TeamData.WarWinTarget;

            _friendlyFillImage.anchorMin = new Vector2(0,0);
            _friendlyFillImage.anchorMax = new Vector2(Mathf.Lerp(_friendlyFillImage.anchorMax.x, winRatio,Time.deltaTime), 1);

            _enemyFillImage.anchorMin = new Vector2(Mathf.Lerp(_enemyFillImage.anchorMin.x, winRatio, Time.deltaTime), 0);
            _enemyFillImage.anchorMax = new Vector2(1, 1);

            _midBarImage.anchorMin = new Vector2(Mathf.Lerp(_midBarImage.anchorMin.x, winRatio, Time.deltaTime), 0f);
            _midBarImage.anchorMax = new Vector2(Mathf.Lerp(_midBarImage.anchorMax.x, winRatio, Time.deltaTime), 1f);

            _friendlyStatusText.text =  $"{winRatio * 100f:N1}";
            _enemyStatusText.text = $"{(1- winRatio) * 100f:N1}";
        }
    }
}

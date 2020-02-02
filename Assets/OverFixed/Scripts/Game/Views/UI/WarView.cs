using System;
using OverFixed.Scripts.Game.Models.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace OverFixed.Scripts.Game.Views.UI
{
    public class WarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _friendlyRatioText;
        [SerializeField] private Text _enemyRatioText;
        private TeamData _teamData;

        [Inject]
        public void Initialize(TeamData teamData)
        {
            _teamData = teamData;
        }

        private void Update()
        {
            var ratio = _teamData.WarStatus / TeamData.WarWinTarget;
            _slider.value = ratio;
            var ratioInt = Mathf.RoundToInt(ratio * 100f);
            _friendlyRatioText.text = ratioInt.ToString();
            _enemyRatioText.text = (100 - ratioInt).ToString();
        }
    }
}
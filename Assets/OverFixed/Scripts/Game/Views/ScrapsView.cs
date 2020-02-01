using System;
using OverFixed.Scripts.Game.Models;
using OverFixed.Scripts.Game.Models.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace OverFixed.Scripts.Game.Views
{
    public class ScrapsView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private TeamData _teamData;
        
        [Inject]
        public void Initialize(TeamData teamData)
        {
            _teamData = teamData;
        }

        private void Update()
        {
            _image.fillAmount = _teamData.Scrap / GameRules.MaxScrapAmount;
        }
    }
}
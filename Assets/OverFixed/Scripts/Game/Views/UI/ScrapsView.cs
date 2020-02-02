using OverFixed.Scripts.Game.Models.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace OverFixed.Scripts.Game.Views.UI
{
    public class ScrapsView : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private TeamData _teamData;
        
        [Inject]
        public void Initialize(TeamData teamData)
        {
            _teamData = teamData;
        }

        private void Update()
        {
            _text.text = Mathf.RoundToInt(_teamData.Scrap).ToString();
        }
    }
}
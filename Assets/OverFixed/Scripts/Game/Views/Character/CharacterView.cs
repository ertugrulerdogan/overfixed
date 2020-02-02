using System;
using System.Collections.Generic;
using OverFixed.Scripts.Game.Models.Data;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Views.Character
{
    public class CharacterView : MonoBehaviour
    {
        private TeamData _teamData;
    
        [SerializeField]
        private SkinnedMeshRenderer _renderer;
        [SerializeField]
        private List<CharacterTextureData> _textureData;

        [Inject]
        public void Initialize(TeamData teamData)
        {
            _teamData = teamData;
        }

        private void Start()
        {
            _renderer.materials = _textureData[_teamData.NextCharacterTextureIndex].Materials.ToArray();
            _teamData.NextCharacterTextureIndex = (_teamData.NextCharacterTextureIndex + 1) % _textureData.Count;
        }

        [Serializable]
        private class CharacterTextureData
        {
            public List<Material> Materials;
        }
    }
}

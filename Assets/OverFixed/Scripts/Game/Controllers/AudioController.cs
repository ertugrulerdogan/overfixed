using System;
using System.Collections.Generic;
using OverFixed.Scripts.Helpers.AudioSystem.Scripts;
using UnityEngine;

namespace OverFixed.Scripts.Game.Controllers
{
    [Serializable]
    public class SoundEffectPair
    {
        public SoundEffectType Type;
        public SoundEffect SoundEffect;
    }

    [Serializable]
    public class PlaylistPair
    {
        public PlaylistType Type;
        public Playlist Playlist;
    }
    
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private List<SoundEffectPair> _soundEffects;
        [SerializeField] private List<PlaylistPair> _playlists;
        
        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = new AudioManager(transform);
        }
    }
    
    public enum SoundEffectType
    {
        None = 0,
        Test01 = 1,
        Test02 = 2,
        Test03 = 3,
        Test04 = 4,
        Test05 = 5,
    }

    public enum PlaylistType
    {
        None = 0,
        Test01 = 1,
        Test02 = 2,
        Test03 = 3,
        Test04 = 4,
        Test05 = 5,        
    }
}
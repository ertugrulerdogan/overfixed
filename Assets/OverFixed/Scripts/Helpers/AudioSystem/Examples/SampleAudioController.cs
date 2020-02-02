using System.Collections.Generic;
using OverFixed.Scripts.Helpers.AudioSystem.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace OverFixed.Scripts.Helpers.AudioSystem.Examples
{
    public class SampleAudioController : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private List<Toggle> _backgroundMusicToggles;
        [SerializeField] private List<Toggle> _soundEffectToggles;
        [SerializeField] private List<Slider> _backgroundMusicSliders;
        [SerializeField] private List<Slider> _soundEffectSliders;
        
        [Header("Background Music Settings")]
        [SerializeField]
        private Dictionary<SamplePlaylistType, Playlist> _playlistDict;
        
        [Header("Sound Effect Settings")]
        [SerializeField]
        private Dictionary<SampleSoundEffectType, SoundEffect> _soundEffectDict;

        private AudioManager _audioManager;
        
        private void Start()
        {
            _audioManager = new AudioManager(transform);
            
            _backgroundMusicToggles.ForEach(_audioManager.BindToggleToBackgroundMusic);
            _soundEffectToggles.ForEach(_audioManager.BindToggleToSoundEffect);

            _backgroundMusicSliders.ForEach(_audioManager.BindSliderToBackgroundMusic);
            _soundEffectSliders.ForEach(_audioManager.BindSliderToSoundEffect);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _audioManager.PlaySoundEffect(_soundEffectDict[SampleSoundEffectType.Test01]);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                _audioManager.PlayPlaylist(_playlistDict[SamplePlaylistType.Test01]);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                
            }            
            if (Input.GetKeyDown(KeyCode.R))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                
            }
        }
    }

    public enum SampleSoundEffectType
    {
        None = 0,
        Test01 = 1,
        Test02 = 2,
        Test03 = 3,
        Test04 = 4,
        Test05 = 5,
    }

    public enum SamplePlaylistType
    {
        None = 0,
        Test01 = 1,
        Test02 = 2,
        Test03 = 3,
        Test04 = 4,
        Test05 = 5,        
    }
}


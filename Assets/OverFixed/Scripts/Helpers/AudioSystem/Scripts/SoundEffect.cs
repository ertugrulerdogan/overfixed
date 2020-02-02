using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace OverFixed.Scripts.Helpers.AudioSystem.Scripts
{
    [Serializable]
    public class SoundEffect
    {
        [HideInInspector] public AudioSource AudioSource;

        [Range(0f, 1f)]
        [SerializeField] 
        private float _originalVolume = 1f;
        [SerializeField] 
        private bool _playRandom;
        [SerializeField] 
        private bool _useAudioMixerGroup;
        [SerializeField]
        private AudioMixerGroup _audioMixerGroup;
        [SerializeField] 
        private List<AudioClip> _audioClipList;
                        
        private Action _onComplete;
        private int _currentAudioClipIndex;
        private int _previousAudioClipIndex;
        
        public AudioClip Clip
        {
            get
            {
                if (!_playRandom || _audioClipList.Count <= 1) return _audioClipList[0];
                
                _currentAudioClipIndex = UnityEngine.Random.Range(0, _audioClipList.Count);
                while (_currentAudioClipIndex == _previousAudioClipIndex)
                {
                    _currentAudioClipIndex = UnityEngine.Random.Range(0, _audioClipList.Count);
                }
                
                _previousAudioClipIndex = _currentAudioClipIndex;
                return _audioClipList[_currentAudioClipIndex];
            }
        }
        
        public float OriginalVolume => _originalVolume;

        public bool UseAudioMixerGroup => _useAudioMixerGroup;

        public AudioMixerGroup AudioMixerGroup => _audioMixerGroup;
    }
}
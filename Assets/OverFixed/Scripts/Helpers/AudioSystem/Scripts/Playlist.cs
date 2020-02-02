using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OverFixed.Scripts.Helpers.AudioSystem.Scripts
{
    [Serializable]
    public class Playlist
    {
        public event Action OnClipChanged;
        [HideInInspector] public bool Paused;
        [HideInInspector] public AudioSource AudioSource;
        
        #region Inspector
        
        [Range(0f, 1f)]
        [SerializeField] 
        private float _originalVolume = 1f;
        [SerializeField] 
        private float _transitionDuration = 0.25f;
        [SerializeField] 
        private MusicTransitionType _transitionType = MusicTransitionType.None;
        [SerializeField] 
        private bool _playRandom;
        [SerializeField] 
        private List<AudioClip> _audioClipList;
        
        #endregion
        
        private int _currentAudioClipIndex;
        private int _previousAudioClipIndex;

        public float OriginalVolume => _originalVolume;
        public float TransitionDuration => _transitionDuration;
        public MusicTransitionType TransitionType => _transitionType;

        public AudioClip CurrentClip => _audioClipList[_currentAudioClipIndex];
        public AudioClip NextClip => _audioClipList[(_currentAudioClipIndex + 1) % _audioClipList.Count];

        public void SetStartClip(int startClipIndex)
        {
            _currentAudioClipIndex = startClipIndex;
        }

        public void MoveToNext()
        {
            OnClipChanged?.Invoke();
            
            if(_playRandom)
            {
                _currentAudioClipIndex = Random.Range(0, _audioClipList.Count);
                while(_audioClipList.Count > 1 && _previousAudioClipIndex ==_currentAudioClipIndex)
                {
                    _currentAudioClipIndex = Random.Range(0, _audioClipList.Count);
                }
                _previousAudioClipIndex = _currentAudioClipIndex;
            }
            else
            {
                _currentAudioClipIndex = (_currentAudioClipIndex + 1) % _audioClipList.Count;
            }
        }
    }
}    
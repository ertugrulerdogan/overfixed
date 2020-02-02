using System;
using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Interaction;
using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Behaviours.Scraps;
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

        private void Awake()
        {
            _audioManager = new AudioManager(transform);
        }

        private void Start()
        {
            ScrapBehaviour.OnPickUp += ScrapBehaviour_OnPickUp;
            ItemInteractionBehaviour.OnItemUseBegin += ItemInteractionBehaviour_OnItemUseBegin;
            ItemInteractionBehaviour.OnItemUseEnd += ItemInteractionBehaviour_OnItemUseEnd;
        }

        private void OnDestroy()
        {
            
        }

        public AudioSource PlaySoundEffect(SoundEffectType type, bool shouldRecycle = true)
        {
            SoundEffect soundEffect = _soundEffects.First(item => item.Type == type).SoundEffect;
            return _audioManager.PlaySoundEffect(soundEffect, shouldRecycle);
        }

        public Playlist PlayPlaylist(PlaylistType type)
        {
            return _audioManager.PlayPlaylist(_playlists.First(item => item.Type == type).Playlist);
        }
        
        #region Event Listeners
        
        private void ScrapBehaviour_OnPickUp()
        {
            PlaySoundEffect(SoundEffectType.ScrapCollected);
        }

        private AudioSource _extinguisherEffect;
        private void ItemInteractionBehaviour_OnItemUseBegin(Type obj)
        {
            if (obj == typeof(ExtinguisherBehaviour))
            {
                _extinguisherEffect = PlaySoundEffect(SoundEffectType.FireExtinguish, false).SetLoop(true);
            }
        }
        
        private void ItemInteractionBehaviour_OnItemUseEnd(Type obj)
        {            
            if (obj == typeof(ExtinguisherBehaviour))
            {
                _audioManager.StopSoundEffect(_extinguisherEffect);
            }     
        }
        
        #endregion
    }
    
    public enum SoundEffectType
    {
        None = 0,
        ShipSmoke = 1,
        ShipBurn = 2,
        ShipExplode = 3,
        ShipScrapping = 4,
        ShipScrapped = 4,
        ShipRepairing = 5,
        ShipRepaired = 6,
        ShipLanding = 7,
        ShipFlying = 8,
        FireExtinguish = 9,
        
        ToolTake = 10,
        ScrapCollected = 11,
        CharacterFootStep = 12,
        Gun = 13,
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
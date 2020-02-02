using System;
using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Interaction;
using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Behaviours.Scraps;
using OverFixed.Scripts.Helpers.AudioSystem.Scripts;
using OverFixed.Scripts.Helpers.CoroutineSystem;
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
            ItemInteractionBehaviour.OnItemPick += ItemInteractionBehaviour_OnItemPick;
            ItemInteractionBehaviour.OnItemDrop += ItemInteractionBehaviour_OnItemDrop;
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

        private bool _canPlayScrapCollectedSound = true;
        private void ScrapBehaviour_OnPickUp()
        {
            if (_canPlayScrapCollectedSound)
            {
                PlaySoundEffect(SoundEffectType.ScrapCollected);

                _canPlayScrapCollectedSound = false;
                CoroutineManager.DoAfterGivenTime(0.2f, () =>
                {
                    _canPlayScrapCollectedSound = true; 
                    
                });
            }
        }

        private AudioSource _itemUseEffect;
        private void ItemInteractionBehaviour_OnItemUseBegin(Type obj)
        {
            if (obj == typeof(ExtinguisherBehaviour))
            {
                _itemUseEffect = PlaySoundEffect(SoundEffectType.FireExtinguish, false).SetLoop(true);
            }
            if (obj == typeof(WrenchBehaviour))
            {
                _itemUseEffect = PlaySoundEffect(SoundEffectType.WrenchUse, false).SetLoop(true);
            }
            if (obj == typeof(CutterBehaviour))
            {
                _itemUseEffect = PlaySoundEffect(SoundEffectType.CutterUse, false).SetLoop(true);
            }
            if (obj == typeof(RifleBehaviour))
            {
                _itemUseEffect = PlaySoundEffect(SoundEffectType.RifleUse, false).SetLoop(true);
            }
        }
        
        private void ItemInteractionBehaviour_OnItemUseEnd(Type obj)
        {            
            if (obj == typeof(ExtinguisherBehaviour) || obj == typeof(WrenchBehaviour) || obj == typeof(CutterBehaviour) || obj == typeof(RifleBehaviour))
            {
                _audioManager.StopSoundEffect(_itemUseEffect);
            }     
        }
        
        private void ItemInteractionBehaviour_OnItemPick()
        {
            PlaySoundEffect(SoundEffectType.ToolTake);
        }

        private void ItemInteractionBehaviour_OnItemDrop()
        {
            PlaySoundEffect(SoundEffectType.ToolTake);
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
        WrenchUse = 14,
        CutterUse = 15,
        RifleUse = 16
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
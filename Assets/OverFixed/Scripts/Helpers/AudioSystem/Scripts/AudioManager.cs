using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using OverFixed.Scripts.Helpers.CoroutineSystem;
using UnityEngine;
using UnityEngine.UI;

namespace OverFixed.Scripts.Helpers.AudioSystem.Scripts
{
    public enum MusicTransitionType{ None, Swift, LinearFade, CrossFade }
    
    public class AudioManager
    {
        #region Constants

        private const int AudioSourceInitialAmount = 6;
        private const int AudioSourceIncreaseAmount = 3;

        private const string MasterVolumeKey = "MasterVolumeKey";
        private const string BackgroundMusicVolumeKey = "BackgroundMusicVolKey";
        private const string SoundEffectVolumeKey = "SoundEffectVolumeKey";
        private const string MuteKey = "MuteKey";

        #endregion

        #region Variables

        private readonly Transform _audioSourceContainer;
        
        private readonly Queue<AudioSource> _availableAudioSources;
        private readonly List<AudioSource> _busyAudioSources;

        private readonly Dictionary<Playlist, Coroutine> _activePlaylistDict;
        private readonly Dictionary<AudioSource, SoundEffect> _activeSoundEffectDict;

        private readonly List<Toggle> _backgroundMusicBindToggles;
        private readonly List<Toggle> _soundEffectBindToggles;
        private readonly List<Slider> _backgroundMusicBindSliders;
        private readonly List<Slider> _soundEffectBindSliders;
        
        private int _audioSourceUniqueId;
        
        private float _masterVolume, _backgroundMusicVolume, _soundEffectVolume;
        private float MasterVolume
        {
            get { return _masterVolume; }
            set
            {
                _masterVolume = value;
                Refresh();
                PlayerPrefs.SetFloat(MasterVolumeKey, _masterVolume);
            }
        }
        private float BackgroundMusicVolume
        {
            get { return _backgroundMusicVolume; }
            set
            {
                _backgroundMusicVolume = value;
                Refresh();
                PlayerPrefs.SetFloat(BackgroundMusicVolumeKey, _backgroundMusicVolume);
            }
        }
        private float SoundEffectVolume
        {
            get { return _soundEffectVolume; }
            set
            {
                _soundEffectVolume = value;

                Refresh();
                PlayerPrefs.SetFloat(SoundEffectVolumeKey, _soundEffectVolume);
            }
        }

        public bool IsMasterMuted
        {
            get
            {
                if (!PlayerPrefs.HasKey(MasterVolumeKey + MuteKey))
                    PlayerPrefs.SetInt(MasterVolumeKey + MuteKey, false.ToInt());

                return PlayerPrefs.GetInt(MasterVolumeKey + MuteKey).ToBool();
            }
            private set { PlayerPrefs.SetInt(MasterVolumeKey + MuteKey, value.ToInt()); }
        }
        public bool IsBackgroundMusicMuted
        {
            get
            {
                if (!PlayerPrefs.HasKey(BackgroundMusicVolumeKey + MuteKey))
                    PlayerPrefs.SetInt(BackgroundMusicVolumeKey + MuteKey, false.ToInt());

                return PlayerPrefs.GetInt(BackgroundMusicVolumeKey + MuteKey).ToBool();
            }
            private set
            {
                PlayerPrefs.SetInt(BackgroundMusicVolumeKey + MuteKey, value.ToInt());                
            }
        }
        public bool IsSoundEffectMuted
        {
            get
            {
                if (!PlayerPrefs.HasKey(SoundEffectVolumeKey + MuteKey))
                    PlayerPrefs.SetInt(SoundEffectVolumeKey + MuteKey, false.ToInt());

                return PlayerPrefs.GetInt(SoundEffectVolumeKey + MuteKey).ToBool();
            }
            private set
            {
                PlayerPrefs.SetInt(SoundEffectVolumeKey + MuteKey, value.ToInt());   
            }
        }
        
        #endregion

        public AudioManager()
        {
            _availableAudioSources = new Queue<AudioSource>();
            _busyAudioSources = new List<AudioSource>();
            
            _activePlaylistDict = new Dictionary<Playlist, Coroutine>();
            _activeSoundEffectDict = new Dictionary<AudioSource, SoundEffect>();
            
            _backgroundMusicBindToggles = new List<Toggle>();
            _soundEffectBindToggles = new List<Toggle>();
            _backgroundMusicBindSliders = new List<Slider>();
            _soundEffectBindSliders = new List<Slider>();
        }
        
        public AudioManager(Transform audioSourceContainer) : this()
        {
            _audioSourceContainer = audioSourceContainer;
            
            Initialize();
        }

        ~AudioManager()
        {
            Clear();
        }
        
        #region Main Methods
        
        private void Initialize()
        {
            CreateAudioSources(AudioSourceInitialAmount);

            MasterVolume = PlayerPrefs.HasKey(MasterVolumeKey)
                ? PlayerPrefs.GetFloat(MasterVolumeKey)
                : 1f;
            BackgroundMusicVolume = PlayerPrefs.HasKey(BackgroundMusicVolumeKey)
                ? PlayerPrefs.GetFloat(BackgroundMusicVolumeKey)
                : 1f;
            SoundEffectVolume = PlayerPrefs.HasKey(SoundEffectVolumeKey)
                ? PlayerPrefs.GetFloat(SoundEffectVolumeKey)
                : 1f;
        }

        private void Clear()
        {            
            _backgroundMusicBindToggles.Clear();
            _soundEffectBindToggles.Clear();
            _backgroundMusicBindSliders.Clear();
            _soundEffectBindSliders.Clear();
            
            for (int i = 0; i < _activePlaylistDict.Count; i++)
            {
                var item = _activePlaylistDict.ElementAt(i);
                StopPlaylist(item.Key);
            }
            _activePlaylistDict.Clear();
            
            for (int i = 0; i < _activeSoundEffectDict.Count; i++)
            {
                StopSoundEffect(_activeSoundEffectDict.ElementAt(i).Key);
            }
            _activeSoundEffectDict.Clear();
        }
        
        private void Refresh()
        {
            _backgroundMusicBindSliders.ForEach(item => item.Set(BackgroundMusicVolume));
            _soundEffectBindSliders.ForEach(item => item.Set(SoundEffectVolume));
            
            foreach (var playlist in _activePlaylistDict)
            {
                playlist.Key.AudioSource.SetVolume(playlist.Key.OriginalVolume * BackgroundMusicVolume * MasterVolume);
            }

            foreach (var soundEffect in _activeSoundEffectDict)
            {
                soundEffect.Key.SetVolume(soundEffect.Value.OriginalVolume * SoundEffectVolume * MasterVolume);
            }
        }
        
        public void Pause()
        {
            foreach (var item in _activePlaylistDict)
            {
                item.Key.AudioSource.mute = true;
            }

            foreach (var item in _activeSoundEffectDict)
            {
                item.Key.mute = true;
            }
        }

        public void Resume()
        {
            foreach (var item in _activePlaylistDict)
            {
                item.Key.AudioSource.mute = IsMasterMuted || IsBackgroundMusicMuted;
            }

            foreach (var item in _activeSoundEffectDict)
            {
                item.Key.mute = IsMasterMuted || IsSoundEffectMuted;
            }
        }
                
        #endregion

        #region Playlist Methods

        public Playlist PlayPlaylist(Playlist playlist)
        {
            if (_activePlaylistDict.ContainsKey(playlist)) return null;
            
            playlist.AudioSource = GetAudioSource();

            _activePlaylistDict.Add(playlist, CoroutineManager.StartChildCoroutine(PlayPlaylistCoroutine(playlist)));

            return playlist;
        }

        private IEnumerator PlayPlaylistCoroutine(Playlist playlist)
        {
            playlist.SetStartClip(0);

            while (!playlist.Paused)
            {
                playlist.AudioSource
                    .SetMute(IsMasterMuted || IsBackgroundMusicMuted)
                    .SetClip(playlist.CurrentClip)
                    .SetVolume(0f);

                playlist.AudioSource.DOFade(playlist.OriginalVolume * MasterVolume * BackgroundMusicVolume,
                    playlist.TransitionDuration);

                bool shouldInTransition = false;
                playlist.AudioSource
                    .SetBeforeComplete(playlist.TransitionDuration, () => { shouldInTransition = true; })
                    .DevePlay();

                yield return new WaitUntil(() => shouldInTransition);

                playlist.AudioSource.DOFade(0f, playlist.TransitionDuration);

                yield return new WaitForSeconds(playlist.TransitionDuration);

                playlist.MoveToNext();
            }
        }
        
        public void StopPlaylist(Playlist playlist, bool withFade = false)
        {
            if (!_activePlaylistDict.ContainsKey(playlist)) return;

            CoroutineManager.StopChildCoroutine(_activePlaylistDict[playlist]);
            _activePlaylistDict[playlist] = null;
            _activePlaylistDict.Remove(playlist);

            if (withFade)
            {
                playlist.AudioSource.DOFade(0f, playlist.TransitionDuration).OnComplete(() =>
                {
                    RecycleAudioSource(playlist.AudioSource);
                });                               
            }
            else
            {
                RecycleAudioSource(playlist.AudioSource);
            }
        }

        #endregion

        #region Sound Effect Methods

        public AudioSource PlaySoundEffect(SoundEffect soundEffect, bool shouldRecycle = true)
        {               
            AudioSource audioSource = GetAudioSource();
            soundEffect.AudioSource = audioSource;
            
            soundEffect.AudioSource
                .SetClip(soundEffect.Clip)
                .SetLoop(false)
                .SetVolume(soundEffect.OriginalVolume * SoundEffectVolume * MasterVolume)
                .SetPitch(1f)
                .SetMute(IsMasterMuted || IsSoundEffectMuted)
                .DevePlay();

            if (soundEffect.UseAudioMixerGroup) soundEffect.AudioSource.SetMixer(soundEffect.AudioMixerGroup);
            if (shouldRecycle) audioSource.SetOnComplete(() => StopSoundEffect(audioSource));
            
            _activeSoundEffectDict.Add(audioSource, soundEffect);

            return soundEffect.AudioSource;
        }

        public AudioSource PlaySoundEffect(AudioClip audioClip, bool shouldRecycle = true)
        {
            AudioSource audioSource = GetAudioSource().SetClip(audioClip).DevePlay();;
            
            _activeSoundEffectDict.Add(audioSource, new SoundEffect());

            if (shouldRecycle) audioSource.SetOnComplete(() => StopSoundEffect(audioSource));
            
            return audioSource;
        }

        public void StopSoundEffect(AudioSource audioSource, bool useFadeOut = true)
        {
            if (audioSource == null || !_activeSoundEffectDict.ContainsKey(audioSource)) return;
                
            _activeSoundEffectDict.Remove(audioSource);

            if (useFadeOut)
            {
                audioSource.DOFade(0f, 0.25f).OnComplete(() =>
                {
                    RecycleAudioSource(audioSource);
                });   
            }
            else
            {
                RecycleAudioSource(audioSource);
            }            
        }
        
        #endregion

        #region Volume Methods

        public void ToggleMaster()
        {
            IsMasterMuted = !IsMasterMuted;


            foreach (var item in _activePlaylistDict)
            {
                item.Key.AudioSource.mute = IsMasterMuted || IsBackgroundMusicMuted;
            }

            foreach (var item in _activeSoundEffectDict)
            {
                item.Key.mute = IsMasterMuted || IsSoundEffectMuted;
            }
        }

        public void ToggleBackgroundMusicVolume()
        {
            IsBackgroundMusicMuted = !IsBackgroundMusicMuted;
            
            foreach (var item in _activePlaylistDict)
            {
                item.Key.AudioSource.mute = IsBackgroundMusicMuted;
            }

            _backgroundMusicBindToggles.ForEach(toggle => toggle.Set(IsBackgroundMusicMuted));   
        }

        public void ToggleSoundEffectVolume()
        {
            IsSoundEffectMuted = !IsSoundEffectMuted;
            
            foreach (var item in _activeSoundEffectDict)
            {
                item.Key.mute = IsSoundEffectMuted;
            }
            _soundEffectBindToggles.ForEach(toggle => toggle.Set(IsSoundEffectMuted, false));
        }

        #endregion

        #region UI Binding Methods

        public void BindToggleToMaster(Toggle toggle)
        {
            toggle.isOn = IsMasterMuted;
            toggle.onValueChanged.AddListener(value => { ToggleMaster(); });
        }

        public void BindToggleToBackgroundMusic(Toggle toggle)
        {
            if (_backgroundMusicBindToggles.Contains(toggle)) return;
            
            _backgroundMusicBindToggles.Add(toggle);
            
            toggle.isOn = IsBackgroundMusicMuted;
            toggle.onValueChanged.AddListener(value => { ToggleBackgroundMusicVolume(); });                        
        }

        public void BindToggleToSoundEffect(Toggle toggle)
        {
            if (_soundEffectBindToggles.Contains(toggle)) return;
            
            _soundEffectBindToggles.Add(toggle);
            
            toggle.isOn = IsSoundEffectMuted;
            toggle.onValueChanged.AddListener(value => { ToggleSoundEffectVolume(); });
        }

        public void BindSliderToMaster(Slider slider)
        {
            slider.onValueChanged.AddListener(value => { MasterVolume = value; });
        }

        public void BindSliderToBackgroundMusic(Slider slider)
        {
            if (_backgroundMusicBindSliders.Contains(slider)) return;

            _backgroundMusicBindSliders.Add(slider);
            
            slider.value = BackgroundMusicVolume;
            slider.onValueChanged.AddListener(value => { BackgroundMusicVolume = value; });
        }

        public void BindSliderToSoundEffect(Slider slider)
        {
            if(_soundEffectBindSliders.Contains(slider)) return;
            
            _soundEffectBindSliders.Add(slider);

            slider.value = SoundEffectVolume;
            slider.onValueChanged.AddListener(value => { SoundEffectVolume = value; });
        }

        #endregion
        
        #region Audio Source Pool Methods

        private AudioSource GetAudioSource()
        {
            if (_availableAudioSources.Count == 0) CreateAudioSources(AudioSourceIncreaseAmount);

            AudioSource audioSource = _availableAudioSources.Dequeue();
            _busyAudioSources.Add(audioSource);

            audioSource.mute = false;
            audioSource.gameObject.SetActive(true);

            return audioSource;
        }

        private void CreateAudioSources(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject go = new GameObject();
                go.SetActive(false);
                go.name = "AudioSource" + (++_audioSourceUniqueId) ;
                go.transform.SetParent(_audioSourceContainer);
                go.transform.localPosition = Vector3.zero;          
                
                AudioSource audioSource  = go.AddComponent<AudioSource>();  
                _availableAudioSources.Enqueue(audioSource);
            }
        }

        private void RecycleAudioSource(AudioSource audioSource)
        {
            audioSource.DOComplete();
            audioSource.SetVolume(1f)
                .SetPitch(1f)
                .SetLoop(false)
                .SetPlaybackTime(0f)
                .SetSpatialBlend(0f)
                .SetMixer(null);
            
            audioSource.transform.SetParent(_audioSourceContainer);
            audioSource.transform.localPosition = Vector3.zero;
            audioSource.gameObject.SetActive(false);
         
            _busyAudioSources.Remove(audioSource);
            _availableAudioSources.Enqueue(audioSource);

            for (int i = 0; i < _busyAudioSources.Count; i++)
            {
                if (_busyAudioSources[i].clip == audioSource.clip) return;
            }   
        }

        #endregion
    }
}
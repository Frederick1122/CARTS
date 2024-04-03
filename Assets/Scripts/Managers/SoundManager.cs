using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Base;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        private const string CUSTOM_SOUND_LIBRARY_PATH = "Configs/CustomSoundLibrary";
        private const string FMOD_MUSIC_PATH = "Music";
        private const string FMOD_SFX_PATH = "SFX";
        private const string FMOD_BUS_PREFIX = "bus:/";
        private const string FMOD_VCA_PREFIX = "vca:/";
    
        [SerializeField] private StudioEventEmitter _emmiter;

        private CustomSoundLibraryConfig _customSoundLibrary;
        
        private Bus _masterBus;
        private VCA _musicVca;
        private VCA _sfxVca;
        
        private float _masterVolume;

        private CancellationTokenSource _refreshBackgroundCts;

        protected override void Awake()
        {
            base.Awake();
            
            _customSoundLibrary = Resources.Load<CustomSoundLibraryConfig>(CUSTOM_SOUND_LIBRARY_PATH);
            
            _masterBus = RuntimeManager.GetBus(FMOD_BUS_PREFIX);
            _musicVca = RuntimeManager.GetVCA(FMOD_VCA_PREFIX + FMOD_MUSIC_PATH);
            _sfxVca = RuntimeManager.GetVCA(FMOD_VCA_PREFIX + FMOD_SFX_PATH);
            
            SetVolume(SoundType.Sound, SettingsManager.Instance.GetVolume(SoundType.Sound));

            SettingsManager.Instance.OnValueChanged += SetVolume;
        }

        public void PlayBackground(SceneType sceneType, int compositionIdx = -1, int lastCompositionIdx = -1)
        {
            var currentCompositions = GetSceneCompositions(sceneType);
            var currentCompositionIdx = compositionIdx != -1
                ? Mathf.Clamp(compositionIdx, 0, currentCompositions.Count)
                : GetRandomCompositionIdx(currentCompositions.Count, lastCompositionIdx);

            Play(currentCompositions[currentCompositionIdx]);
            _refreshBackgroundCts?.Cancel();
            _refreshBackgroundCts = new CancellationTokenSource();
            _emmiter.EventDescription.getLength(out var length);
            var delay = (float)length / 1000;
            
            RefreshSoundTask(_refreshBackgroundCts.Token, delay, sceneType, currentCompositionIdx).Forget();
        }
        
        public void StopAllSound()
        {
            _masterBus.stopAllEvents(STOP_MODE.IMMEDIATE);
            _refreshBackgroundCts.Cancel();
        }
        
        private void OnDestroy()
        {
            if (SettingsManager.Instance != null)
                SettingsManager.Instance.OnValueChanged -= SetVolume;
        }

        private void Play(EventReference eventReference)
        {
            _emmiter.EventReference = eventReference;
            _emmiter.Play();
        }
        
        private void UpdateAllVolumeChannels()
        {
            SetVolume(SoundType.Music, SettingsManager.Instance.GetVolume(SoundType.Music));
            SetVolume(SoundType.Sfx, SettingsManager.Instance.GetVolume(SoundType.Sfx));
        } 

        private void SetVolume(SoundType soundType, float value)
        {
            switch (soundType)
            {
                case SoundType.Sound:
                    _masterVolume = value;
                    UpdateAllVolumeChannels();
                    break;
                case SoundType.Music:
                    _musicVca.setVolume(value * _masterVolume);
                    break;
                case SoundType.Sfx:
                    _sfxVca.setVolume(value * _masterVolume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
            }
        }

        private List<EventReference> GetSceneCompositions(SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.Lobby:
                    return _customSoundLibrary.lobbyMusicCompositions;
                case SceneType.LapRace:
                    return _customSoundLibrary.lapRaceMusicCompositions;
                case SceneType.FreeRide:
                    return _customSoundLibrary.freeRideMusicCompositions;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
            }
        }
        
        private int GetRandomCompositionIdx(int compositionCount, int exclude)
        {
            int idx;
            if (compositionCount == 1)
            {
                idx = 0;
            }
            else if (exclude == -1)
            {
                idx = Random.Range(0, compositionCount);
            }
            else
            {
                var suitableExclude = Mathf.Clamp(exclude, 0, compositionCount);
                var range = Enumerable.Range(0, compositionCount).Where(i => i != suitableExclude).ToList();
                idx = Random.Range(0, range.Count);
            }

            return idx;
        }
        
        private async UniTaskVoid RefreshSoundTask(CancellationToken token, float delay, 
            SceneType sceneType, int compositionIdx)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            
            PlayBackground(sceneType, -1, compositionIdx);
        }
    }
   
    public enum SoundType
    {
        Sound = 0,
        Music = 1,
        Sfx = 2
    }

    public enum SceneType
    {
        Lobby = 0,
        LapRace = 1,
        FreeRide = 2
    }
}
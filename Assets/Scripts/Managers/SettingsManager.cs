using System;
using Base;
using Newtonsoft.Json;
using UI.Windows.Pause;
using UnityEngine;

namespace Managers
{
    public class SettingsManager : SaveLoadManager<SettingsData, SettingsManager>
    {
        private const string SETTINGS_JSON_PATH = "Settings.json";

        public event Action<SoundType, float> OnValueChanged = delegate(SoundType type, float f) {  };

        [SerializeField] private float _baseMusicVolume = 0.5f;
        [SerializeField] private float _baseSoundVolume = 0.5f;
        
        public void SetVolume(SoundType soundType, float value)
        {
            switch (soundType)
            {
                case SoundType.Music:
                    _saveData.musicVolume = value;
                    break;
                case SoundType.Sound:
                    _saveData.soundVolume = value;
                    break;
                case SoundType.Sfx:
                    _saveData.sfxVolume = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
            }
            
            OnValueChanged?.Invoke(soundType, value);
        }

        public float GetVolume(SoundType soundType)
        {
            return soundType switch
            {
                SoundType.Music => _saveData.musicVolume,
                SoundType.Sound => _saveData.soundVolume,
                SoundType.Sfx => _saveData.sfxVolume,
                _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
            };
        }

        protected override void Load()
        {
            base.Load();
            if (_saveData != null)
                return;
            
            _saveData = new SettingsData{
                musicVolume = _baseMusicVolume,
                soundVolume = _baseSoundVolume
            };
            
            Save();
        }

        protected override void UpdatePath()
        {
            _secondPath = SETTINGS_JSON_PATH;
            base.UpdatePath();
        }
    }
    
    public class SettingsData 
    {
        private const string SOUND = "SOUND_VOLUME";
        private const string MUSIC = "MUSIC_VOLUME";
        private const string SFX = "SFX_VOLUME";

        [JsonProperty(SOUND)]
        public float soundVolume = 1f;
        [JsonProperty(MUSIC)]
        public float musicVolume = 1f;
        [JsonProperty(SFX)]
        public float sfxVolume = 1f;
    }
}
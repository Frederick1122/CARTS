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

        public event Action<SliderType, float> OnValueChanged = delegate(SliderType type, float f) {  };

        [SerializeField] private float _baseMusicVolume = 0.5f;
        [SerializeField] private float _baseSoundVolume = 0.5f;
        
        public void SetVolume(SliderType sliderType, float value)
        {
            switch (sliderType)
            {
                case SliderType.Music:
                    _saveData.musicVolume = value;
                    break;
                case SliderType.Sound:
                    _saveData.soundVolume = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sliderType), sliderType, null);
            }
            
            OnValueChanged?.Invoke(sliderType, value);
        }

        public float GetVolume(SliderType sliderType)
        {
            return sliderType switch
            {
                SliderType.Music => _saveData.musicVolume,
                SliderType.Sound => _saveData.soundVolume,
                _ => throw new ArgumentOutOfRangeException(nameof(sliderType), sliderType, null)
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

        [JsonProperty(SOUND)]
        public float soundVolume = 1f;
        [JsonProperty(MUSIC)]
        public float musicVolume = 1f;
    }
}
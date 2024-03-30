using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Pause
{
    public class DefaultPauseView : PauseWindowView
    {
        public event Action<SliderType, float> OnSliderValueChanged;
        
        [SerializeField] private Button _lobbyButton;
        [SerializeField] private Button _resumeButton;

        [SerializeField] private SliderAction _musicSliderAction;
        [SerializeField] private SliderAction _soundSliderAction;
        
        public override void Init(UIModel uiModel)
        {
            _lobbyButton.onClick.AddListener(BackToLobby);
            _resumeButton.onClick.AddListener(Resume);

            _musicSliderAction.OnValueChanged += OnSliderValueChanged;
            _soundSliderAction.OnValueChanged += OnSliderValueChanged;
            
            _musicSliderAction.Init();
            _soundSliderAction.Init();
        }

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);
            var castModel = (DefaultPauseModel) uiModel;
            
            _musicSliderAction.Update(castModel.musicVolume);
            _soundSliderAction.Update(castModel.soundVolume);
        }

        private void OnDestroy()
        {
            _lobbyButton?.onClick.RemoveListener(BackToLobby);
            _resumeButton?.onClick.RemoveListener(Resume);
            
            _musicSliderAction.OnValueChanged -= OnSliderValueChanged;
            _soundSliderAction.OnValueChanged -= OnSliderValueChanged;

            _musicSliderAction?.Terminate();
            _soundSliderAction?.Terminate();
        }
    }

    public class DefaultPauseModel : UIModel
    {
        public float soundVolume;
        public float musicVolume;
    }
    
    [Serializable]
    public class SliderAction
    {
        public event Action<SliderType, float> OnValueChanged = delegate(SliderType type, float f) {  };

        [SerializeField] private Slider _slider;
        [SerializeField] private SliderType _sliderType;
        
        public void Init()
        {
            _slider.onValueChanged.AddListener(delegate (float value) { OnValueChanged?.Invoke(_sliderType, value); });
            _slider.onValueChanged.AddListener(delegate(float value) { Debug.Log($"{_sliderType} : {value}"); });
        }

        public void Update(float value)
        {
            _slider.value = value;
        }

        public void Terminate()
        {
            _slider?.onValueChanged.RemoveAllListeners();
        }
    }

    public enum SliderType
    {
        Music = 0,
        Sound = 1,
    }
}

using System;
using Managers;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Settings
{
    public class SettingsWindowView : UIView
    {
        public event Action<LocalizationLanguage> OnChangeLanguageAction = delegate {  };
        public event Action OpenLobbyAction = delegate {  };

        [field: SerializeField] public SliderView MusicSlider { get; private set; }
        [field: SerializeField] public SliderView SoundSlider { get; private set; }

        [SerializeField] private Button _openLobbyButton;
        [SerializeField] private Button _englishButton;
        [SerializeField] private Button _russianButton;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            _openLobbyButton.onClick.AddListener(OpenLobby);
            _russianButton.onClick.AddListener(() => ChangeLocalizationLanguage(LocalizationLanguage.Russian));
            _englishButton.onClick.AddListener(() => ChangeLocalizationLanguage(LocalizationLanguage.English));
        }

        private void OnDestroy()
        {
            _openLobbyButton?.onClick.RemoveAllListeners();
            _russianButton?.onClick.RemoveAllListeners();
            _englishButton?.onClick.RemoveAllListeners();
        }

        private void OpenLobby() => OpenLobbyAction?.Invoke();

        private void ChangeLocalizationLanguage(LocalizationLanguage language) => OnChangeLanguageAction?.Invoke(language);
    }

    public class SettingsWindowModel : UIModel { }
}
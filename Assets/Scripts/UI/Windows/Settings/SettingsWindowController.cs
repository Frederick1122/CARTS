using System;
using Managers;
using UI.Elements;
using Unity.VisualScripting;

namespace UI.Windows.Settings
{
    public class SettingsWindowController : UIController
    {
        public event Action OpenLobbyAction;

        private SettingsWindowView _castView;

        private SliderController _musicSliderController;
        private SliderController _sfxSliderController;

        public override void Init()
        { 
            base.Init();
            
            _castView = GetView<SettingsWindowView>();
            _castView.OpenLobbyAction += OpenLobby;
            _castView.OnChangeLanguageAction += ChangeLanguage;

            _musicSliderController = _castView.MusicSlider.AddComponent<SliderController>();
            _musicSliderController.Setup(_castView.MusicSlider,
                () => SettingsManager.Instance.GetVolume(SoundType.Music),
                value => SettingsManager.Instance.SetVolume(SoundType.Music, value));
            
            _sfxSliderController = _castView.SfxSlider.AddComponent<SliderController>();
            _sfxSliderController.Setup(_castView.SfxSlider,
                () => SettingsManager.Instance.GetVolume(SoundType.Sfx),
                value => SettingsManager.Instance.SetVolume(SoundType.Sfx, value));
        }

        public override void Show()
        {
            base.Show();
            _musicSliderController.Show();
            _sfxSliderController.Show();
        }

        public override void Hide()
        {
            _musicSliderController.Hide();
            _sfxSliderController.Hide();
            base.Hide();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            _castView.OpenLobbyAction -= OpenLobby;
            _castView.OnChangeLanguageAction -= ChangeLanguage;
        }

        protected override UIModel GetViewData() => 
            new SettingsWindowModel();

        private void OpenLobby() =>
            OpenLobbyAction?.Invoke();

        private void ChangeLanguage(LocalizationLanguage language) => 
            LocalizationManager.Instance.SetLocalization(language);
    }
}
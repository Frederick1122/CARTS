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
        private SliderController _soundSliderController;

        public override void Init()
        { 
            base.Init();
            
            _castView = GetView<SettingsWindowView>();
            _castView.OpenLobbyAction += OpenLobby;

            _musicSliderController = _castView.MusicSlider.AddComponent<SliderController>();
            _musicSliderController.Setup(_castView.MusicSlider,
                () => SettingsManager.Instance.GetVolume(SoundType.Music),
                value => SettingsManager.Instance.SetVolume(SoundType.Music, value));
            
            _soundSliderController = _castView.SoundSlider.AddComponent<SliderController>();
            _soundSliderController.Setup(_castView.SoundSlider,
                () => SettingsManager.Instance.GetVolume(SoundType.Sound),
                value => SettingsManager.Instance.SetVolume(SoundType.Sound, value));
        }

        public override void Show()
        {
            base.Show();
            _musicSliderController.Show();
            _soundSliderController.Show();
        }

        public override void Hide()
        {
            _musicSliderController.Hide();
            _soundSliderController.Hide();
            base.Hide();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            _castView.OpenLobbyAction -= OpenLobby;
        }

        protected override UIModel GetViewData()
        {
            return new SettingsWindowModel();
        }

        private void OpenLobby() =>
            OpenLobbyAction?.Invoke();
    }
}
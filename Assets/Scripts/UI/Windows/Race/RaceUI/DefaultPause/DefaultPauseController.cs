using Managers;
using UI.Elements;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Windows.Pause
{
    public class DefaultPauseController : PauseWindowController
    {
        private DefaultPauseView _castView;
        private SliderController _musicSliderController;
        private SliderController _soundSliderController;

        public override void Init()
        {
            base.Init();

            _castView = GetView<DefaultPauseView>();
            _castView.OnBackToLobby += BackToLobby;
            _castView.OnResume += Resume;

            _musicSliderController = _castView.MusicSlider.AddComponent<SliderController>();
            _musicSliderController.Setup(_castView.MusicSlider,
                () => SettingsManager.Instance.GetVolume(SoundType.Music),
                value => SettingsManager.Instance.SetVolume(SoundType.Music, value));
            
            _soundSliderController = _castView.SoundSlider.AddComponent<SliderController>();
            _soundSliderController.Setup(_castView.SoundSlider,
                () => SettingsManager.Instance.GetVolume(SoundType.Sound),
                value => SettingsManager.Instance.SetVolume(SoundType.Sound, value));
        }

        private void OnDestroy()
        {
            _castView.OnBackToLobby -= BackToLobby;
            _castView.OnResume -= Resume;
        }

        public override void Show()
        {
            base.Show();
            Time.timeScale = 0;
            _musicSliderController.Show();
            _soundSliderController.Show();
        }

        public override void Hide()
        {
            _musicSliderController.Hide();
            _soundSliderController.Hide();
            Time.timeScale = 1;
            base.Hide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Resume();
        }
    }
}

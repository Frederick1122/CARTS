using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI.Windows.Pause
{
    public class DefaultPauseController : PauseWindowController
    {
        private DefaultPauseView _castView;

        public override void Init()
        {
            _castView = GetView<DefaultPauseView>();
            _castView.OnSliderValueChanged += UpdateSoundVolume;
            _castView.OnBackToLobby += BackToLobby;
            _castView.OnResume += Resume;
            base.Init();
        }

        private void OnDestroy()
        {
            _castView.OnSliderValueChanged -= UpdateSoundVolume;
            _castView.OnBackToLobby -= BackToLobby;
            _castView.OnResume -= Resume;
        }

        public override void Show()
        {
            Time.timeScale = 0;
            base.Show();

            var model = new DefaultPauseModel
            {
                musicVolume = SettingsManager.Instance.GetVolume(SliderType.Music),
                soundVolume = SettingsManager.Instance.GetVolume(SliderType.Sound)
            };
            
            _castView.UpdateView(model);
        }

        public override void Hide()
        {
            Time.timeScale = 1;
            base.Hide();
        }

        private void UpdateSoundVolume(SliderType sliderType, float value)
        {
            SettingsManager.Instance.SetVolume(sliderType, value);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Resume();
        }
    }
}

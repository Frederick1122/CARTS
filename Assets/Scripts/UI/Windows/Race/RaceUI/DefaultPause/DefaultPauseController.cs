using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Pause
{
    public class DefaultPauseController : PauseWindowController
    {
        private DefaultPauseView _castView;

        public override void Init()
        {
            _castView = GetView<DefaultPauseView>();
            _castView.OnBackToLobby += BackToLobby;
            _castView.OnResume += Resume;
            base.Init();
        }

        private void OnDestroy()
        {
            _castView.OnBackToLobby -= BackToLobby;
            _castView.OnResume -= Resume;
        }

        public override void Show()
        {
            Time.timeScale = 0;
            base.Show();
        }

        public override void Hide()
        {
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

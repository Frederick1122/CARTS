using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Pause
{
    public class DefaultPauseView : PauseWindowView
    {
        [SerializeField] private Button _lobbyButton;
        [SerializeField] private Button _resumeButton;

        public override void Init(UIModel uiModel)
        {
            _lobbyButton.onClick.AddListener(BackToLobby);
            _resumeButton.onClick.AddListener(Resume);
        }

        private void OnDestroy()
        {
            _lobbyButton.onClick.RemoveListener(BackToLobby);
            _resumeButton.onClick.RemoveListener(Resume);
        }
    }
}

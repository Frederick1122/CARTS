using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Settings
{
    public class SettingsWindowView : UIView
    {
        public event Action OpenLobbyAction;

        [SerializeField] private Button _openLobbyButton;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            _openLobbyButton.onClick.AddListener(OpenLobbyAction.Invoke);
        }

        private void OnDestroy() =>
            _openLobbyButton?.onClick.RemoveAllListeners();
    }

    public class SettingsWindowModel : UIModel { }
}
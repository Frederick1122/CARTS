using System;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Settings
{
    public class SettingsWindowView : UIView
    {
        public event Action OpenLobbyAction = delegate {  };

        [field: SerializeField] public SliderView MusicSlider { get; private set; }
        [field: SerializeField] public SliderView SoundSlider { get; private set; }

        [SerializeField] private Button _openLobbyButton;
        
        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            _openLobbyButton.onClick.AddListener(OpenLobby);
        }

        private void OnDestroy() =>
            _openLobbyButton?.onClick.RemoveAllListeners();

        private void OpenLobby()
        {
            OpenLobbyAction?.Invoke();
        }
    }

    public class SettingsWindowModel : UIModel { }
}
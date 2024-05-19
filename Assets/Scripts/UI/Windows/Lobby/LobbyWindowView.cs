using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Lobby
{
    public class LobbyWindowView : UIView
    {
        public event Action OpenShopAction;
        public event Action OpenSettingsAction;
        public event Action OpenMapSelectionAction;
        public event Action OpenGarageAction;
        public event Action OpenTutorialAction;

        [SerializeField] private Button _openTutorialButton;
        [SerializeField] private Button _openShopButton;
        [SerializeField] private Button _openSettingsButton;
        [SerializeField] private Button _openMapSelectionButton;
        [SerializeField] private Button _openGarageButton;

        public override void Init(UIModel model)
        {
            base.Init(model);
            _openShopButton.onClick.AddListener(OpenShopAction.Invoke);
            _openSettingsButton.onClick.AddListener(OpenSettingsAction.Invoke);
            _openMapSelectionButton.onClick.AddListener(OpenMapSelectionAction.Invoke);
            _openGarageButton.onClick.AddListener(OpenGarageAction.Invoke);
            _openTutorialButton.onClick.AddListener(OpenTutorialAction.Invoke);
        }

        private void OnDestroy()
        {
            _openShopButton?.onClick.RemoveAllListeners();
            _openSettingsButton?.onClick.RemoveAllListeners();
            _openMapSelectionButton?.onClick.RemoveAllListeners();
            _openGarageButton?.onClick.RemoveAllListeners();
        }
    }

    public class LobbyWindowModel : UIModel { }
}
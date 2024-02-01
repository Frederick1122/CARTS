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
        }

        private void OnDestroy()
        {
            _openShopButton?.onClick.RemoveAllListeners();
            _openSettingsButton?.onClick.RemoveAllListeners();
            _openMapSelectionButton?.onClick.RemoveAllListeners();
            _openGarageButton?.onClick.RemoveListener(OpenGarageAction.Invoke);
        }
    }

    public class LobbyWindowModel : UIModel { }
}
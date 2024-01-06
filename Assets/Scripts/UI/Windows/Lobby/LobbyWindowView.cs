using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Lobby
{
    public class LobbyWindowView : UIView<LobbyWindowModel>
    {
        public event Action OpenShopAction;
        public event Action OpenSettingsAction;
        public event Action OpenMapSelectionAction;
        
        [SerializeField] private Button _openShopButton;
        [SerializeField] private Button _openSettingsButton;
        [SerializeField] private Button _openMapSelectionButton;

        public override void Init(LobbyWindowModel uiModel)
        {
            base.Init(uiModel);
            _openShopButton.onClick.AddListener(OpenShopAction.Invoke);
            _openSettingsButton.onClick.AddListener(OpenSettingsAction.Invoke);
            _openMapSelectionButton.onClick.AddListener(OpenMapSelectionAction.Invoke);
        }

        private void OnDestroy()
        {
            _openShopButton?.onClick.RemoveAllListeners();
            _openSettingsButton?.onClick.RemoveAllListeners();
            _openMapSelectionButton?.onClick.RemoveAllListeners();
        }
    }

    public class LobbyWindowModel : UIModel
    {
        
    }
}
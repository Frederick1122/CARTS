using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopWindowView : UIView
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

    public class ShopWindowModel : UIModel
    {

    }
}
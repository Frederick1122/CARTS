using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.MapSelection
{
    public class MapSelectionWindowView : UIView
    {
        public event Action OpenLobbyAction;

        public event Action GoToGameAction;

        [SerializeField] private Button _openLobbyButton;
        [SerializeField] private Button _goToGameButton;

        public override void Init(UIModel model)
        {
            base.Init(model);
            _openLobbyButton.onClick.AddListener(OpenLobbyAction.Invoke);
            _goToGameButton.onClick.AddListener(GoToGameAction.Invoke);
        }

        private void OnDestroy()
        {
            _openLobbyButton?.onClick.RemoveAllListeners();
            _goToGameButton?.onClick.RemoveAllListeners();
        }
    }

    public class MapSelectionWindowModel : UIModel { }
}
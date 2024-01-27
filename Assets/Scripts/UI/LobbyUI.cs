using System;
using UI.Windows.Lobby;
using UI.Windows.MapSelection;
using UI.Windows.Settings;
using UI.Windows.Shop;
using UnityEngine;

namespace UI
{
    public class LobbyUI : WindowManager
    {
        public event Action OpenShopAction = delegate { };
        public event Action OpenSettingsAction = delegate { };
        public event Action OpenMapSelectionAction = delegate { };

        public event Action OpenLobbyAction = delegate { };
        public event Action GoToGameAction = delegate { };

        [Header("Controllers")]
        [SerializeField] private LobbyWindowController _lobbyWindowController;
        [SerializeField] private ShopWindowController _shopWindowController;
        [SerializeField] private MapSelectionWindowController _mapSelectionWindowController;
        [SerializeField] private SettingsWindowController _settingsWindowController;

        protected override void AddControllers()
        {
            _controllers.Add(_lobbyWindowController.GetType(), _lobbyWindowController);
            _controllers.Add(_shopWindowController.GetType(), _shopWindowController);
            _controllers.Add(_mapSelectionWindowController.GetType(), _mapSelectionWindowController);
            _controllers.Add(_settingsWindowController.GetType(), _settingsWindowController);
        }

        public override void Init()
        {
            base.Init();

            _lobbyWindowController.OpenShopAction += RequestToOpenShop;
            _lobbyWindowController.OpenSettingsAction += RequestToOpenSettings;
            _lobbyWindowController.OpenMapSelectionAction += RequestToOpenMapSelection;

            _shopWindowController.OpenLobbyAction += RequestToOpenLobby;

            _settingsWindowController.OpenLobbyAction += RequestToOpenLobby;

            _mapSelectionWindowController.OpenLobbyAction += RequestToOpenLobby;
            _mapSelectionWindowController.GoToGameAction += RequestToGoToGame;
        }

        private void OnDestroy()
        {
            if (_lobbyWindowController != null)
            {
                _lobbyWindowController.OpenShopAction -= RequestToOpenShop;
                _lobbyWindowController.OpenSettingsAction -= RequestToOpenSettings;
                _lobbyWindowController.OpenMapSelectionAction -= RequestToOpenMapSelection;
            }

            if (_shopWindowController != null)
                _shopWindowController.OpenLobbyAction -= RequestToOpenLobby;

            if (_settingsWindowController != null)
                _settingsWindowController.OpenLobbyAction -= RequestToOpenLobby;

            if (_mapSelectionWindowController != null)
            {
                _mapSelectionWindowController.OpenLobbyAction -= RequestToOpenLobby;
                _mapSelectionWindowController.GoToGameAction -= RequestToGoToGame;
            }
        }

        private void RequestToOpenShop() =>
            OpenShopAction?.Invoke();

        private void RequestToOpenSettings() =>
            OpenSettingsAction?.Invoke();

        private void RequestToOpenMapSelection() =>
            OpenMapSelectionAction?.Invoke();

        private void RequestToOpenLobby() =>
            OpenLobbyAction?.Invoke();

        private void RequestToGoToGame() =>
            GoToGameAction?.Invoke();
    }
}
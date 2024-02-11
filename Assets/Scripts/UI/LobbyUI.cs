using System;
using UI.Windows.Garage;
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
        public event Action OpenGarageAction = delegate { };

        public event Action OpenLobbyAction = delegate { };
        public event Action GoToGameAction = delegate { };

        public event Action<CarData> OnCarInGarageUpdate = delegate { };
        public event Action<CarData> OnCarInShopUpdate = delegate { };

        [Header("Controllers")]
        [SerializeField] private LobbyWindowController _lobbyWindowController;
        [SerializeField] private ShopWindowController _shopWindowController;
        [SerializeField] private MapSelectionWindowController _mapSelectionWindowController;
        [SerializeField] private SettingsWindowController _settingsWindowController;
        [SerializeField] private GarageWindowController _garageWindowController;

        protected override void AddControllers()
        {
            _controllers.Add(_lobbyWindowController.GetType(), _lobbyWindowController);
            _controllers.Add(_shopWindowController.GetType(), _shopWindowController);
            _controllers.Add(_mapSelectionWindowController.GetType(), _mapSelectionWindowController);
            _controllers.Add(_settingsWindowController.GetType(), _settingsWindowController);
            _controllers.Add(_garageWindowController.GetType(), _garageWindowController);
        }

        public override void Init()
        {
            base.Init();

            _lobbyWindowController.OpenShopAction += RequestToOpenShop;
            _lobbyWindowController.OpenSettingsAction += RequestToOpenSettings;
            _lobbyWindowController.OpenMapSelectionAction += RequestToOpenMapSelection;
            _lobbyWindowController.OpenGarageAction += RequestToOpenGarage;

            _shopWindowController.OnOpenLobby += RequestToOpenLobby;

            _settingsWindowController.OpenLobbyAction += RequestToOpenLobby;

            _mapSelectionWindowController.OpenLobbyAction += RequestToOpenLobby;
            _mapSelectionWindowController.GoToGameAction += RequestToGoToGame;

            _garageWindowController.OnOpenLobby += RequestToOpenLobby;
            _garageWindowController.OnCarInGarageUpdate += RequestToUpdateCarInGarage;
        }

        private void OnDestroy()
        {
            if (_lobbyWindowController != null)
            {
                _lobbyWindowController.OpenShopAction -= RequestToOpenShop;
                _lobbyWindowController.OpenSettingsAction -= RequestToOpenSettings;
                _lobbyWindowController.OpenMapSelectionAction -= RequestToOpenMapSelection;
                _lobbyWindowController.OpenGarageAction -= RequestToOpenGarage;
            }

            if (_shopWindowController != null)
            {
                _shopWindowController.OnOpenLobby -= RequestToOpenLobby;
            }

            if (_settingsWindowController != null)
                _settingsWindowController.OpenLobbyAction -= RequestToOpenLobby;

            if (_mapSelectionWindowController != null)
            {
                _mapSelectionWindowController.OpenLobbyAction -= RequestToOpenLobby;
                _mapSelectionWindowController.GoToGameAction -= RequestToGoToGame;
            }

            if(_garageWindowController != null)
            {
                _garageWindowController.OnOpenLobby -= RequestToOpenLobby;
                _garageWindowController.OnCarInGarageUpdate -= RequestToUpdateCarInGarage;
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

        private void RequestToOpenGarage() =>
            OpenGarageAction?.Invoke();

        private void RequestToUpdateCarInGarage(CarData carData) =>
            OnCarInGarageUpdate?.Invoke(carData);
    }
}
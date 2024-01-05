using Base;
using UI.Windows.Lobby;
using UI.Windows.MapSelection;
using UI.Windows.Settings;
using UI.Windows.Shop;
using UnityEngine;

namespace UI
{
    public class LobbyUIManager : Singleton<LobbyUIManager>
    {
        [Header("Controllers")]
        [SerializeField] private LobbyWindowController _lobbyWindowController;
        [SerializeField] private ShopWindowController _shopWindowController;
        [SerializeField] private MapSelectionWindowController _mapSelectionWindowController;
        [SerializeField] private SettingsWindowController _settingsWindowController;

        public void ShowLobby()
        {
            HideAll();
            _lobbyWindowController.Show();
        }
        
        public void ShowShop()
        {
            HideAll();
            _shopWindowController.Show();
        }
        
        public void ShowMapSelection()
        {
            HideAll();
            _mapSelectionWindowController.Show();
        }
        
        public void ShowSettings()
        {
            HideAll();
            _settingsWindowController.Show();
        }
        
        private void Start()
        {
            _lobbyWindowController.Init();
            _shopWindowController.Init();
            _mapSelectionWindowController.Init();
            _settingsWindowController.Init();
            
            HideAll();
        }

        private void HideAll()
        {
            _lobbyWindowController.Hide();
            _shopWindowController.Hide();
            _mapSelectionWindowController.Hide();
            _settingsWindowController.Hide();
        }
    }
}
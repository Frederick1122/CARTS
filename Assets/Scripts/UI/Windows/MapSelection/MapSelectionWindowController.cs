using System;

namespace UI.Windows.MapSelection
{
    public class MapSelectionWindowController : UIController<MapSelectionWindowView, MapSelectionWindowModel>
    {
        public event Action OpenLobbyAction;
        public event Action GoToGameAction;

        public override void Init()
        {
            _view.OpenLobbyAction += OpenLobby;
            _view.GoToGameAction += GoToGame;
            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;
            
            _view.OpenLobbyAction -= OpenLobby;
            _view.GoToGameAction -= GoToGame;
        }
        
        protected override MapSelectionWindowModel GetViewData()
        {
            return new MapSelectionWindowModel();
        }
        
        private void OpenLobby()
        {
            OpenLobbyAction?.Invoke();
        }

        private void GoToGame()
        {
            GoToGameAction?.Invoke();
        }
    }
}
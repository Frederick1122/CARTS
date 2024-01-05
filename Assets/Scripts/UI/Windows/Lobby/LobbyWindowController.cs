using System;

namespace UI.Windows.Lobby
{
    public class LobbyWindowController : UIController<LobbyWindowView, LobbyWindowModel>
    {
        public override void Init()
        {
            _view.OpenShopAction += LobbyUIManager.Instance.ShowShop;
            _view.OpenSettingsAction += LobbyUIManager.Instance.ShowSettings;
            _view.OpenMapSelectionAction += LobbyUIManager.Instance.ShowMapSelection;
            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;
            
            _view.OpenShopAction -= LobbyUIManager.Instance.ShowShop;
            _view.OpenSettingsAction -= LobbyUIManager.Instance.ShowSettings;
            _view.OpenMapSelectionAction -= LobbyUIManager.Instance.ShowMapSelection;
        }

        protected override LobbyWindowModel GetViewData()
        {
            return new LobbyWindowModel();
        }
    }
}
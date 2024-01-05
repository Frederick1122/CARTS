using System;

namespace UI.Windows.Shop
{
    public class ShopWindowController : UIController<ShopWindowView, ShopWindowModel>
    {
        public event Action OpenLobbyAction;

        public override void Init()
        {
            _view.OpenLobbyAction += OpenLobby;
            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;
            
            _view.OpenLobbyAction -= OpenLobby;
        }
        
        protected override ShopWindowModel GetViewData()
        {
            return new ShopWindowModel();
        }
        
                
        private void OpenLobby()
        {
            OpenLobbyAction?.Invoke();
        }
    }
}
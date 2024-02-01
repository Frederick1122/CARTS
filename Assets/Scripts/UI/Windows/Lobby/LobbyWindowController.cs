using System;

namespace UI.Windows.Lobby
{
    public class LobbyWindowController : UIController
    {
        public event Action OpenShopAction;
        public event Action OpenSettingsAction;
        public event Action OpenMapSelectionAction;

        public override void Init()
        {
            GetView<LobbyWindowView>().OpenShopAction += OpenShop;
            GetView<LobbyWindowView>().OpenSettingsAction += OpenSettings;
            GetView<LobbyWindowView>().OpenMapSelectionAction += OpenMapSelection;
            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            GetView<LobbyWindowView>().OpenShopAction -= OpenShop;
            GetView<LobbyWindowView>().OpenSettingsAction -= OpenSettings;
            GetView<LobbyWindowView>().OpenMapSelectionAction -= OpenMapSelection;
        }

        protected override UIModel GetViewData()
        {
            return new LobbyWindowModel();
        }

        private void OpenShop() =>
            OpenShopAction?.Invoke();

        private void OpenSettings() =>
            OpenSettingsAction?.Invoke();

        private void OpenMapSelection() =>
            OpenMapSelectionAction?.Invoke();
    }
}
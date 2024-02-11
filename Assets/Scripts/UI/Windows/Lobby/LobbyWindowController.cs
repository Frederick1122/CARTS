using System;
using UI.Widgets.CurrencyWidget;

namespace UI.Windows.Lobby
{
    public class LobbyWindowController : UIController
    {
        public event Action OpenShopAction;
        public event Action OpenSettingsAction;
        public event Action OpenMapSelectionAction;
        public event Action OpenGarageAction = delegate { };

        public override void Init()
        {
            GetView<LobbyWindowView>().OpenShopAction += OpenShop;
            GetView<LobbyWindowView>().OpenSettingsAction += OpenSettings;
            GetView<LobbyWindowView>().OpenMapSelectionAction += OpenMapSelection;
            GetView<LobbyWindowView>().OpenGarageAction += OpenGarage;

            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            GetView<LobbyWindowView>().OpenShopAction -= OpenShop;
            GetView<LobbyWindowView>().OpenSettingsAction -= OpenSettings;
            GetView<LobbyWindowView>().OpenMapSelectionAction -= OpenMapSelection;
            GetView<LobbyWindowView>().OpenGarageAction -= OpenGarage;
        }

        public override void Show()
        {
            UIManager.Instance.GetWidgetUI().ShowWindow(typeof(CurrencyWidgetController), false);
            base.Show();
        }

        public override void Hide()
        {
            UIManager.Instance.GetWidgetUI().HideWindow(typeof(CurrencyWidgetController));
            base.Hide();
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

        private void OpenGarage() =>
            OpenGarageAction?.Invoke();
    }
}
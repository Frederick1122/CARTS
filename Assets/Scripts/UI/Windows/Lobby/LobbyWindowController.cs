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
        public event Action OpenTutorialAction = delegate { };

        public override void Init()
        {
            LobbyWindowView castView = GetView<LobbyWindowView>();

            castView.OpenShopAction += OpenShop;
            castView.OpenSettingsAction += OpenSettings;
            castView.OpenMapSelectionAction += OpenMapSelection;
            castView.OpenGarageAction += OpenGarage;
            castView.OpenTutorialAction += OpenTutorial;

            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            LobbyWindowView castView = GetView<LobbyWindowView>();

            castView.OpenShopAction -= OpenShop;
            castView.OpenSettingsAction -= OpenSettings;
            castView.OpenMapSelectionAction -= OpenMapSelection;
            castView.OpenGarageAction -= OpenGarage;
            castView.OpenTutorialAction -= OpenTutorial;
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

        private void OpenTutorial() =>
            OpenTutorialAction?.Invoke();

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
using System;

namespace UI.Windows.Settings
{
    public class SettingsWindowController : UIController<SettingsWindowView, SettingsWindowModel>
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

        protected override SettingsWindowModel GetViewData()
        {
            return new SettingsWindowModel();
        }

        private void OpenLobby() =>
            OpenLobbyAction?.Invoke();
    }
}
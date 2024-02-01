using System;

namespace UI.Windows.Settings
{
    public class SettingsWindowController : UIController
    {
        public event Action OpenLobbyAction;

        public override void Init()
        { 
            GetView<SettingsWindowView>().OpenLobbyAction += OpenLobby;
            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            GetView<SettingsWindowView>().OpenLobbyAction -= OpenLobby;
        }

        protected override UIModel GetViewData()
        {
            return new SettingsWindowModel();
        }

        private void OpenLobby() =>
            OpenLobbyAction?.Invoke();
    }
}
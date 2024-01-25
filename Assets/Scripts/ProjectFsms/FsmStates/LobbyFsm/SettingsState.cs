using Core.FSM;
using UI;
using UI.Windows.Settings;

namespace FsmStates.LobbyFsm
{
    public class SettingsState : FsmState
    {
        private LobbyUI _lobbyUI;

        public SettingsState(Fsm fsm, LobbyUI lobbyUI) : base(fsm)
        {
            _lobbyUI = lobbyUI;
            _lobbyUI.OpenLobbyAction += OpenLobby;
        }

        ~SettingsState()
        {
            if (_lobbyUI == null)
                return;

            _lobbyUI.OpenLobbyAction -= OpenLobby;
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetLobbyUi().ShowWindow(typeof(SettingsWindowController), true);
        }

        private void OpenLobby()
        {
            _fsm.SetState<LobbyState>();
        }
    }
}
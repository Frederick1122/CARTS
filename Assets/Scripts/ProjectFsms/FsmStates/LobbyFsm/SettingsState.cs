using Core.FSM;
using UI;

namespace FsmStates.LobbyFsm
{
    public class SettingsState : FsmState
    {
        private LobbyUIManager _lobbyUIManager;

        public SettingsState(Fsm fsm, LobbyUIManager lobbyUIManager) : base(fsm)
        {
            _lobbyUIManager = lobbyUIManager;
            _lobbyUIManager.OpenLobbyAction += OpenLobby;
        }

        ~SettingsState()
        {
            if (_lobbyUIManager == null)
                return;
            
            _lobbyUIManager.OpenLobbyAction -= OpenLobby;
        }

        public override void Enter()
        {
            base.Enter();
            LobbyUIManager.Instance.OpenSettings();
        }
        
        private void OpenLobby()
        {
            _fsm.SetState<LobbyState>();
        }
    }
}
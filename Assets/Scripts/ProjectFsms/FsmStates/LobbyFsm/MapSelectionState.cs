using Core.FSM;
using UI;

namespace FsmStates.LobbyFsm
{
    public class MapSelectionState : FsmState
    {
        private LobbyUIManager _lobbyUIManager;

        public MapSelectionState(Fsm fsm, LobbyUIManager lobbyUIManager) : base(fsm)
        {
            _lobbyUIManager = lobbyUIManager;
            _lobbyUIManager.OpenLobbyAction += OpenLobby;
            _lobbyUIManager.GoToGameAction += GoToGame;
        }

        ~MapSelectionState()
        {
            if (_lobbyUIManager == null)
                return;
            
            _lobbyUIManager.OpenLobbyAction -= OpenLobby;
            _lobbyUIManager.GoToGameAction -= GoToGame;
        }

        public override void Enter()
        {
            base.Enter();
            LobbyUIManager.Instance.OpenMapSelection();
        }
        
        private void OpenLobby()
        {
            _fsm.SetState<LobbyState>();
        }

        private void GoToGame()
        {
            _fsm.SetState<StartGameState>();
        }
    }
}
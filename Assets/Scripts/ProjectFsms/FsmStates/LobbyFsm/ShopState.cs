using Core.FSM;
using UI;

namespace FsmStates.LobbyFsm
{
    public class ShopState : FsmState
    {
        private LobbyUIManager _lobbyUIManager;

        public ShopState(Fsm fsm, LobbyUIManager lobbyUIManager) : base(fsm)
        {
            _lobbyUIManager = lobbyUIManager;
            _lobbyUIManager.OpenLobbyAction += OpenLobby;
        }
        
        ~ShopState()
        {
            if (_lobbyUIManager == null)
                return;
            
            _lobbyUIManager.OpenLobbyAction -= OpenLobby;
        }

        public override void Enter()
        {
            base.Enter();
            LobbyUIManager.Instance.OpenShop();
        }
        
        private void OpenLobby()
        {
            _fsm.SetState<LobbyState>();
        }
    }
}
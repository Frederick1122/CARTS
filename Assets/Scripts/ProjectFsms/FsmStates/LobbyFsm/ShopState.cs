using Core.FSM;
using UI;
using UI.Windows.Shop;

namespace FsmStates.LobbyFsm
{
    public class ShopState : FsmState
    {
        private readonly LobbyUI _lobbyUI;

        public ShopState(Fsm fsm, LobbyUI lobbyUI) : base(fsm)
        {
            _lobbyUI = lobbyUI;
            _lobbyUI.OpenLobbyAction += OpenLobby;
        }

        ~ShopState()
        {
            if (_lobbyUI == null)
                return;

            _lobbyUI.OpenLobbyAction -= OpenLobby;
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetLobbyUi().ShowWindow(typeof(ShopWindowController), true);
        }

        private void OpenLobby() =>
            _fsm.SetState<LobbyState>();
    }
}
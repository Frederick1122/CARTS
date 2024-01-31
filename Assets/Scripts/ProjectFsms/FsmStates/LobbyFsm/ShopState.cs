using CameraManger.Lobby;
using Core.FSM;
using Lobby.Garage;
using UI;
using UI.Windows.Shop;

namespace FsmStates.LobbyFsm
{
    public class ShopState : FsmState
    {
        private readonly LobbyUI _lobbyUI;
        private readonly Garage _garage;

        public ShopState(Fsm fsm, LobbyUI lobbyUI, Garage garage) : base(fsm)
        {
            _lobbyUI = lobbyUI;
            _garage = garage;
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
            LobbyCameraManager.Instance.SwitchCamera(CameraPositions.Shop);
            UIManager.Instance.GetLobbyUi().ShowWindow(typeof(ShopWindowController), true);
        }

        private void OpenLobby() =>
            _fsm.SetState<LobbyState>();
    }
}
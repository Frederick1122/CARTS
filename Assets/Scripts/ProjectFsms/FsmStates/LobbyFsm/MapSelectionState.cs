using CameraManger.Lobby;
using Core.FSM;
using UI;
using UI.Windows.MapSelection;

namespace FsmStates.LobbyFsm
{
    public class MapSelectionState : FsmState
    {
        private readonly LobbyUI _lobbyUI;

        public MapSelectionState(Fsm fsm, LobbyUI lobbyUI) : base(fsm)
        {
            _lobbyUI = lobbyUI;
            _lobbyUI.OpenLobbyAction += OpenLobby;
            _lobbyUI.GoToGameAction += GoToGame;
        }

        ~MapSelectionState()
        {
            if (_lobbyUI == null)
                return;

            _lobbyUI.OpenLobbyAction -= OpenLobby;
            _lobbyUI.GoToGameAction -= GoToGame;
        }

        public override void Enter()
        {
            base.Enter();
            LobbyCameraManager.Instance.SwitchCamera(CameraPositions.StartRace);
            UIManager.Instance.GetLobbyUi().ShowWindow(typeof(MapSelectionWindowController));
        }

        private void OpenLobby() =>
            _fsm.SetState<LobbyState>();

        private void GoToGame() =>
            _fsm.SetState<StartGameState>();
    }
}
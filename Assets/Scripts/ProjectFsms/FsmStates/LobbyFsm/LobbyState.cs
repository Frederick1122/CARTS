using CameraManger.Lobby;
using Core.FSM;
using Managers;
using ProjectFsms;
using UI;
using UI.Windows.Lobby;

namespace FsmStates.LobbyFsm
{
    public class LobbyState : FsmState
    {
        private readonly LobbyUI _lobbyUI;
        private readonly ProjectFsms.LobbyFsm _lobbyFsm;

        public LobbyState(ProjectFsms.LobbyFsm fsm, LobbyUI lobbyUI) : base(fsm)
        {
            _lobbyFsm = fsm;
            _lobbyUI = lobbyUI;
            _lobbyUI.OpenShopAction += OpenShop;
            _lobbyUI.OpenSettingsAction += OpenSettings;
            _lobbyUI.OpenMapSelectionAction += OpenMapSelection;
            _lobbyUI.OpenGarageAction += OpenGarage;
        }

        ~LobbyState()
        {
            if (_lobbyUI == null)
                return;

            _lobbyUI.OpenShopAction -= OpenShop;
            _lobbyUI.OpenSettingsAction -= OpenSettings;
            _lobbyUI.OpenMapSelectionAction -= OpenMapSelection;
            _lobbyUI.OpenGarageAction -= OpenGarage;
        }

        public override void Enter()
        {
            base.Enter();
            LobbyCameraManager.Instance.SwitchCamera(CameraPositions.Default);
            UIManager.Instance.GetLobbyUi().ShowWindow(typeof(LobbyWindowController), true);
            PlayerManager.Instance.PurchaseDefaultCar();
            if (_lobbyFsm.IsFirstOpen)
            {
                UIManager.Instance.GetLobbyUi().ShowWindow(typeof(TutorialWindowController), false);
                _lobbyFsm.SetFirstOpen();
            }
        }

        private void OpenShop() =>
            _fsm.SetState<ShopState>();

        private void OpenSettings() =>
            _fsm.SetState<SettingsState>();

        private void OpenMapSelection() =>
            _fsm.SetState<MapSelectionState>();

        private void OpenGarage() =>
            _fsm.SetState<GarageState>();
    }
}
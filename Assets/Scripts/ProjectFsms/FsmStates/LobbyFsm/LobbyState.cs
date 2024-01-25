using Core.FSM;
using UI;
using UI.Windows.Lobby;

namespace FsmStates.LobbyFsm
{
    public class LobbyState : FsmState
    {
        private LobbyUI _lobbyUI;

        public LobbyState(Fsm fsm, LobbyUI lobbyUI) : base(fsm)
        {
            _lobbyUI = lobbyUI;
            _lobbyUI.OpenShopAction += OpenShop;
            _lobbyUI.OpenSettingsAction += OpenSettings;
            _lobbyUI.OpenMapSelectionAction += OpenMapSelection;
        }

        ~LobbyState()
        {
            if (_lobbyUI == null)
                return;

            _lobbyUI.OpenShopAction -= OpenShop;
            _lobbyUI.OpenSettingsAction -= OpenSettings;
            _lobbyUI.OpenMapSelectionAction -= OpenMapSelection;
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetLobbyUi().ShowWindow(typeof(LobbyWindowController), true);
        }

        private void OpenShop()
        {
            _fsm.SetState<ShopState>();
        }

        private void OpenSettings()
        {
            _fsm.SetState<SettingsState>();
        }

        private void OpenMapSelection()
        {
            _fsm.SetState<MapSelectionState>();
        }
    }
}
using Core.FSM;
using UI;
using UI.Windows.Lobby;

namespace FsmStates.LobbyFsm
{
    public class LobbyState : FsmState
    {
        private LobbyUIManager _lobbyUIManager;

        public LobbyState(Fsm fsm, LobbyUIManager lobbyUIManager) : base(fsm)
        {
            _lobbyUIManager = lobbyUIManager;
            _lobbyUIManager.OpenShopAction += OpenShop;
            _lobbyUIManager.OpenSettingsAction += OpenSettings;
            _lobbyUIManager.OpenMapSelectionAction += OpenMapSelection;
        }

        ~LobbyState()
        {
            if (_lobbyUIManager == null)
                return;

            _lobbyUIManager.OpenShopAction -= OpenShop;
            _lobbyUIManager.OpenSettingsAction -= OpenSettings;
            _lobbyUIManager.OpenMapSelectionAction -= OpenMapSelection;
        }

        public override void Enter()
        {
            base.Enter();
            LobbyUIManager.Instance.ShowWindow(typeof(LobbyWindowController), true);
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
using Core.FSM;
using FsmStates.LobbyFsm;
using UI;

namespace ProjectFsms
{
    public class LobbyFsm : Fsm
    {
        private LobbyUIManager _lobbyUIManager;

        public LobbyFsm(LobbyUIManager lobbyUIManager)
        {
            _lobbyUIManager = lobbyUIManager;
        }
        
        public override void Init()
        {
            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(LobbyState), new LobbyState(this, _lobbyUIManager));
            _states.Add(typeof(ShopState), new ShopState(this, _lobbyUIManager));
            _states.Add(typeof(SettingsState), new SettingsState(this, _lobbyUIManager));
            _states.Add(typeof(MapSelectionState), new MapSelectionState(this, _lobbyUIManager));
            _states.Add(typeof(StartGameState), new StartGameState(this));
            base.Init();
        }
    }
}
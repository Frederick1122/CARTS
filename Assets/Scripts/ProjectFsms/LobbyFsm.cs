using Core.FSM;
using FsmStates.LobbyFsm;
using UI;
using UI.Windows.MapSelection;

namespace ProjectFsms
{
    public class LobbyFsm : Fsm
    {
        private LobbyUI _lobbyUI;
        public GameType CurrentGameType;
        
        public override void Init()
        {
            _lobbyUI = UIManager.Instance.GetLobbyUi();
            
            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(LobbyState), new LobbyState(this, _lobbyUI));
            _states.Add(typeof(ShopState), new ShopState(this, _lobbyUI));
            _states.Add(typeof(SettingsState), new SettingsState(this, _lobbyUI));
            _states.Add(typeof(MapSelectionState), new MapSelectionState(this, _lobbyUI));
            _states.Add(typeof(StartGameState), new StartGameState(this));
            
            SetState<PreInitializeState>();
            base.Init();
        }
    }
}
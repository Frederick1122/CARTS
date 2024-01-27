using Core.FSM;
using FsmStates.LobbyFsm;
using Installers;
using UI;
using Zenject;

namespace ProjectFsms
{
    public class LobbyFsm : Fsm
    {
        private LobbyUI _lobbyUI;

        [Inject] public GameDataInstaller.GameData gameData;
        
        
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
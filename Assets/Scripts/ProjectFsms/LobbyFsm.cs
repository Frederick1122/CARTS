using Core.FSM;
using FsmStates.LobbyFsm;

namespace ProjectFsms
{
    public class LobbyFsm : Fsm
    {
        protected override void Init()
        {
            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(LobbyState), new LobbyState(this));
            _states.Add(typeof(ShopState), new ShopState(this));
            _states.Add(typeof(SettingsState), new SettingsState(this));
            _states.Add(typeof(ChangeMapState), new ChangeMapState(this));
            _states.Add(typeof(StartGameState), new StartGameState(this));
            base.Init();
        }
    }
}
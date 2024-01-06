using Core.FSM;
using FsmStates.RaceFsm;

namespace ProjectFsms
{
    public class RaceFsm : Fsm
    {
        private RaceManager _raceManager;
        
        public RaceFsm(RaceManager raceManager)
        {
            _raceManager = raceManager;
        }

        public override void Init()
        {
            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(StartRaceState), new StartRaceState(this, _raceManager));
            _states.Add(typeof(RaceState), new RaceState(this));
            _states.Add(typeof(FinishRaceState), new FinishRaceState(this));
            _states.Add(typeof(StartLobbyState), new StartLobbyState(this));
            base.Init();
        }
    }
}
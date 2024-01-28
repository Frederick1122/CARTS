using Core.FSM;
using FsmStates.RaceFsm;
using Race;

namespace ProjectFsms
{
    public class RaceFsm : Fsm
    {
        private RaceManager _raceManager;

        public override void Init()
        {
            _raceManager = LapRaceManager.Instance;

            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(StartRaceState), new StartRaceState(this, _raceManager));
            _states.Add(typeof(RaceState), new RaceState(this));
            _states.Add(typeof(FinishRaceState), new FinishRaceState(this));
            _states.Add(typeof(StartLobbyState), new StartLobbyState(this));

            SetState<PreInitializeState>();
            base.Init();
        }
    }
}
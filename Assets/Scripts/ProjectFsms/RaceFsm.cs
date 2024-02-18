using Core.FSM;
using FsmStates.RaceFsm;
using Race.RaceManagers;

namespace ProjectFsms
{
    public class RaceFsm : Fsm
    {
        private RaceFsmData _raceFsmData = new ();
        
        public override void Init()
        {
            _raceFsmData.raceManager = RaceManager.Instance;

            _states.Add(typeof(PreInitializeState), new PreInitializeState(this, _raceFsmData));
            _states.Add(typeof(StartRaceState), new StartRaceState(this, _raceFsmData));
            _states.Add(typeof(InRaceState), new InRaceState(this, _raceFsmData));
            _states.Add(typeof(FinishRaceState), new FinishRaceState(this, _raceFsmData));
            _states.Add(typeof(StartLobbyState), new StartLobbyState(this, _raceFsmData));
            _states.Add(typeof(PauseState), new PauseState(this, _raceFsmData));

            base.Init();
        }
    }

    public class RaceFsmData
    {
        public RaceManager raceManager;
        public RaceType raceType;
    }
}
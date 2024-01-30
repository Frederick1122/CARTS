using Core.FSM;
using FsmStates.FreeRideFsm;
using Race.RaceManagers;

namespace ProjectFsms
{
    public class FreeRideFsm : Fsm
    {
        private RaceManager _raceManager;

        public override void Init()
        {
            _raceManager = RaceManager.Instance;

            _states.Add(typeof(PreInitializeState), new PreInitializeState(this, _raceManager));
            _states.Add(typeof(StartFreeRideState), new StartFreeRideState(this, _raceManager));
            _states.Add(typeof(InFreeRideState), new InFreeRideState(this, _raceManager));
            _states.Add(typeof(FinishRaceState), new FinishRaceState(this));

            base.Init();
        }
    }
}

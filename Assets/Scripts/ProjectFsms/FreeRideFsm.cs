using Core.FSM;
using FsmStates.FreeRideFsm;

namespace ProjectFsms
{
    public class FreeRideFsm : Fsm
    {
        private RaceManager _raceManager;
        private FreeRideManager _freeRideManager;

        public override void Init()
        {
            _raceManager = FreeRideManager.Instance;

            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(StartFreeRideState), new StartFreeRideState(this, _raceManager));
            _states.Add(typeof(RaceState), new RaceState(this, (FreeRideManager)_raceManager));
            _states.Add(typeof(FinishRaceState), new FinishRaceState(this));

            SetState<PreInitializeState>();
            base.Init();
        }
    }
}

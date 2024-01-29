using Core.FSM;
using FreeRide;
using FsmStates.FreeRideFsm;
using UnityEngine;


namespace ProjectFsms
{
    public class FreeRideFsm : Fsm
    {
        private RaceManager _raceManager;

        public override void Init()
        {
            Debug.Log(FreeRideManager.Instance);
            _raceManager = FreeRideManager.Instance;

            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(StartFreeRideState), new StartFreeRideState(this, _raceManager));
            _states.Add(typeof(FreeRideState), new FreeRideState(this, (FreeRideManager)_raceManager));
            _states.Add(typeof(FinishRaceState), new FinishRaceState(this));

            SetState<PreInitializeState>();
            base.Init();
        }
    }
}

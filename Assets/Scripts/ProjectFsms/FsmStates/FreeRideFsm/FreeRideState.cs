using Core.FSM;
using FreeRide;

namespace FsmStates.FreeRideFsm
{
    public class FreeRideState : FsmState
    {
        private readonly FreeRideManager _freeRideManager;

        public FreeRideState(Fsm fsm, FreeRideManager freeRideManager) : base(fsm)
        {
            _freeRideManager = freeRideManager;
            _freeRideManager.OnFinish += Finish;
        }

        ~FreeRideState() =>
            _freeRideManager.OnFinish -= Finish;

        private void Finish() =>
            _fsm.SetState<FinishRaceState>();
    }
}

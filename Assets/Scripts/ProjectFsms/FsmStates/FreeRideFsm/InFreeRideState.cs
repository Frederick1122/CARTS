using Core.FSM;
using Race.RaceManagers;

namespace FsmStates.FreeRideFsm
{
    public class InFreeRideState : FsmState
    {
        private readonly FreeRideState _freeRideState;

        public InFreeRideState(Fsm fsm, RaceManager raceManager) : base(fsm)
        {
            _freeRideState = raceManager.GetState<FreeRideState>();
            _freeRideState.OnFinishAction += FinishAction;
        }

        ~InFreeRideState() =>
            _freeRideState.OnFinishAction -= FinishAction;

        private void FinishAction() =>
            _fsm.SetState<FinishRaceState>();
    }
}

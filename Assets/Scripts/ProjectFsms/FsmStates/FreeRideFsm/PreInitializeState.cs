using Core.FSM;
using UI;

namespace FsmStates.FreeRideFsm
{
    public class PreInitializeState : FsmState
    {
        public PreInitializeState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();

            UIManager.Instance.SetUiType(UiType.FreeRide);

            _fsm.SetState<StartFreeRideState>();
        }
    }
}

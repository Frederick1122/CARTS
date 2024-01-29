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
            UIManager.Instance.SetUiType(UiType.MobileLayout, false);

            _fsm.SetState<StartFreeRideState>();
        }
    }
}

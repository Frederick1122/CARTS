using Core.FSM;
using UI;

namespace FsmStates.RaceFsm
{
    public class PreInitializeState : FsmState
    {
        public PreInitializeState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();

            UIManager.Instance.SetUiType(UiType.Race);
            UIManager.Instance.SetUiType(UiType.MobileLayout, false);

            _fsm.SetState<StartRaceState>();
        }
    }
}
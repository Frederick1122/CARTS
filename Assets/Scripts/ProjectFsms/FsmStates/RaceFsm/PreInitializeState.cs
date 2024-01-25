using Core.FSM;
using UI;

namespace FsmStates.RaceFsm
{
    public class PreInitializeState : FsmState
    {
        public PreInitializeState(Fsm fsm) : base(fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            UIManager.Instance.SetUiType(UiType.Race);
            
            _fsm.SetState<StartRaceState>();
        }
    }
}
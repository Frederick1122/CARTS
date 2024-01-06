using Core.FSM;

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
            _fsm.SetState<StartRaceState>();
        }
    }
}
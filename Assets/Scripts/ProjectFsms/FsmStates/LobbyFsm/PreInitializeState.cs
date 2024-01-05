using Core.FSM;

namespace FsmStates.LobbyFsm
{
    public class PreInitializeState : FsmState
    {
        public PreInitializeState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();
            //In this moment we can get stats or something else
            
            _fsm.SetState<LobbyState>();
        }
    }
}
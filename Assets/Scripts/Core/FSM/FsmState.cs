namespace Core.FSM
{
    public abstract class FsmState
    {
        protected Fsm _fsm;

        public FsmState(Fsm fsm) => _fsm = fsm;

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}
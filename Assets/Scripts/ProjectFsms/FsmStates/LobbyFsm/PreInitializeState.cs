using Core.FSM;
using UI;

namespace FsmStates.LobbyFsm
{
    public class PreInitializeState : FsmState
    {
        public PreInitializeState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();
            //In this moment we can get stats or something else

            UIManager.Instance.SetUiType(UiType.Lobby);
            
            _fsm.SetState<LobbyState>();
        }
    }
}
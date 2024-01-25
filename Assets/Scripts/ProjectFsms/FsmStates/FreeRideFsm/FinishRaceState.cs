using Core.FSM;
using UnityEngine;

namespace FsmStates.FreeRideFsm
{
    public class FinishRaceState : FsmState
    {
        public FinishRaceState(Fsm fsm) : base(fsm)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enter finish");
            base.Enter();
        }
    }
}

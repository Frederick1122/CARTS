using Core.FSM;
using UnityEngine;

namespace FsmStates.FreeRideFsm
{
    public class RaceState : FsmState
    {
        private FreeRideManager _freeRideManager;

        public RaceState(Fsm fsm, FreeRideManager freeRideManager) : base(fsm)
        {
            _freeRideManager = freeRideManager;
            _freeRideManager.OnFinish += Finish;
        }

        ~RaceState()
        {
            _freeRideManager.OnFinish -= Finish;
        }

        public override void Exit()
        {
            Debug.Log("Exit race state");
            base.Exit();
        }

        private void Finish()
        {
            _fsm.SetState<FinishRaceState>();
        }
    }
}

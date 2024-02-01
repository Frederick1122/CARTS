using Core.FSM;
using UI;
using UI.Windows.FreeRide;
using UI.Windows.LapRace;
using UnityEngine;

namespace FsmStates.FreeRideFsm
{
    public class FinishRaceState : FsmState
    {
        public FinishRaceState(Fsm fsm) : base(fsm) { }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter finish");
            UIManager.Instance.GetRaceUi().GetFinishWindowController<FreeRideWindowController>().Show();
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<FreeRideWindowController>().Hide();
        }
    }
}

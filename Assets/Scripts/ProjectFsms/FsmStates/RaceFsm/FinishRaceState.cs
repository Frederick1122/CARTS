using Core.FSM;
using UI;
using UI.Windows.FreeRide;
using UI.Windows.LapRace;

namespace FsmStates.RaceFsm
{
    public class FinishRaceState : FsmState
    {
        public FinishRaceState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<LapRaceLayoutController>().Show();
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<LapRaceLayoutController>().Hide();
        }
    }
}
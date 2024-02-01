using Core.FSM;
using UI;
using UI.Windows.LapRace;

namespace FsmStates.RaceFsm
{
    public class InRaceState : FsmState
    {
        public InRaceState(Fsm fsm) : base(fsm) { }
        
        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetRaceUi().GetRaceLayout<LapRaceLayoutController>().Show();
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetRaceLayout<LapRaceLayoutController>().Hide();
        }
    }
}
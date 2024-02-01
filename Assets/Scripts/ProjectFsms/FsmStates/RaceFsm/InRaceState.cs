using Core.FSM;
using Race.RaceManagers;
using UI;
using UI.Windows.LapRace;

namespace FsmStates.RaceFsm
{
    public class InRaceState : FsmState
    {
        private RaceManager _raceManager;
        
        public InRaceState(Fsm fsm, RaceManager raceManager) : base(fsm)
        {
            _raceManager = raceManager;
        }

        ~InRaceState()
        {
            _raceManager.GetState<LapRaceState>().OnFinishAction -= SetFinishState;
        }
        
        
        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetRaceUi().GetRaceLayout<LapRaceLayoutController>().Show();
            _raceManager.GetState<LapRaceState>().OnFinishAction += SetFinishState;
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetRaceLayout<LapRaceLayoutController>().Hide();
            _raceManager.GetState<LapRaceState>().OnFinishAction -= SetFinishState;
        }

        private void SetFinishState()
        {
            _fsm.SetState<FinishRaceState>();
        }
    }
}
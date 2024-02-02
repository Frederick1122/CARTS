using Core.FSM;
using ProjectFsms;
using UI;

namespace FsmStates.RaceFsm
{
    public class InRaceState : FsmState
    {
        private RaceFsmData _raceFsmData;

        public InRaceState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;

        ~InRaceState()
        {
            _raceFsmData.raceManager.GetState(_raceFsmData.raceType).OnFinishAction -= SetFinishState;
        }
        
        
        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetRaceUi().GetRaceLayout(_raceFsmData.raceType).Show();
            _raceFsmData.raceManager.GetState(_raceFsmData.raceType).OnFinishAction += SetFinishState;
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetRaceLayout(_raceFsmData.raceType).Hide();
            _raceFsmData.raceManager.GetState(_raceFsmData.raceType).OnFinishAction -= SetFinishState;
        }

        private void SetFinishState()
        {
            _fsm.SetState<FinishRaceState>();
        }
    }
}
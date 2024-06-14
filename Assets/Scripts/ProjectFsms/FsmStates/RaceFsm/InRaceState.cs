using Core.FSM;
using ProjectFsms;
using Race.RaceManagers;
using UI;

namespace FsmStates.RaceFsm
{
    public class InRaceState : FsmState
    {
        private readonly RaceFsmData _raceFsmData;
        private RaceState _curState;

        public InRaceState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;

        ~InRaceState()
        {
            if (_curState == null)
                return;
            
            _curState.OnFinishAction -= SetFinishState;
            _curState.OnPauseAction -= SetPauseState;
        }
        
        public override void Enter()
        {
            base.Enter();
            _curState = _raceFsmData.raceManager.GetState(_raceFsmData.raceType);
            UIManager.Instance.GetRaceUi().GetRaceLayout(_raceFsmData.raceType).Show();
            _curState.OnFinishAction += SetFinishState;
            _curState.OnPauseAction += SetPauseState;
        }

        public override void Exit()
        {
            base.Exit();
            _curState = _raceFsmData.raceManager.GetState(_raceFsmData.raceType);
            UIManager.Instance.GetRaceUi().GetRaceLayout(_raceFsmData.raceType).Hide();
            _curState.OnFinishAction -= SetFinishState;
            _curState.OnPauseAction -= SetPauseState;
        }

        private void SetFinishState() => _fsm.SetState<FinishRaceState>();

        private void SetPauseState() => _fsm.SetState<PauseState>();

    }
}
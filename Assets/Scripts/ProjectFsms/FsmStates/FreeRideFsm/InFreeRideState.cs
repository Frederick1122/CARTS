using Core.FSM;
using Race.RaceManagers;
using UI;
using UI.Windows.FreeRide;
using UI.Windows.LapRace;

namespace FsmStates.FreeRideFsm
{
    public class InFreeRideState : FsmState
    {
        private readonly FreeRideState _freeRideState;

        public InFreeRideState(Fsm fsm, RaceManager raceManager) : base(fsm)
        {
            _freeRideState = raceManager.GetState<FreeRideState>();
            _freeRideState.OnFinishAction += FinishAction;
        }

        ~InFreeRideState() =>
            _freeRideState.OnFinishAction -= FinishAction;
        
        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetRaceUi().GetRaceLayout<FreeRideWindowController>().Show();
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetRaceLayout<FreeRideWindowController>().Hide();
        }

        private void FinishAction() =>
            _fsm.SetState<FinishRaceState>();
    }
}

using Core.FSM;
using Race.RaceManagers;
using UI;
using UI.Windows.FreeRide;

namespace FsmStates.FreeRideFsm
{
    public class PreInitializeState : FsmState
    {
        private readonly RaceManager _raceManager;

        public PreInitializeState(Fsm fsm, RaceManager raceManager) : base(fsm) =>
            _raceManager = raceManager;
        
        public override void Enter()
        {
            base.Enter();

            _raceManager.SetState<FreeRideState>();
            _raceManager.InitState();
            
            UIManager.Instance.SetUiType(UiType.Race);
            UIManager.Instance.GetRaceUi().GetRaceLayout<FreeRideLayoutController>().Show();
            UIManager.Instance.SetUiType(UiType.MobileLayout, false);

            _fsm.SetState<StartFreeRideState>();
        }
    }
}

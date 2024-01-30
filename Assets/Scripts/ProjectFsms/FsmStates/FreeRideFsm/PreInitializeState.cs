using Core.FSM;
using Race.RaceManagers;
using UI;

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
            
            UIManager.Instance.SetUiType(UiType.FreeRide);
            UIManager.Instance.SetUiType(UiType.MobileLayout, false);

            _fsm.SetState<StartFreeRideState>();
        }
    }
}

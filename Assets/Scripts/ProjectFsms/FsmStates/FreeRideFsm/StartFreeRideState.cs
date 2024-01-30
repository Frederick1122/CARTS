using Core.FSM;
using Race.RaceManagers;
using UI;
using UI.Windows.FreeRide;

namespace FsmStates.FreeRideFsm
{
    public class StartFreeRideState : FsmState
    {
        private readonly RaceManager _raceManager;

        public StartFreeRideState(Fsm fsm, RaceManager raceManager) : base(fsm) =>
            _raceManager = raceManager;

        public override void Enter()
        {
            //todo: make delay before start
            
            _raceManager.StartRace();
            
            UIManager.Instance.GetFreeRideUI().ShowWindow(typeof(FreeRideWindowController), false);
            base.Enter();
            _fsm.SetState<InFreeRideState>();
        }

    }
}

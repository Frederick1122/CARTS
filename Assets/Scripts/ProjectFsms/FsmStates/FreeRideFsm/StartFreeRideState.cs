using Core.FSM;

namespace FsmStates.FreeRideFsm
{
    public class StartFreeRideState : FsmState
    {
        private RaceManager _raceManager;

        public StartFreeRideState(Fsm fsm, RaceManager raceManager) : base(fsm)
        {
            _raceManager = raceManager;
        }

        public override void Enter()
        {
            //todo: make delay before start
            _raceManager.Init();
            _raceManager.StartRace();
            base.Enter();
            _fsm.SetState<RaceState>();
        }

    }
}

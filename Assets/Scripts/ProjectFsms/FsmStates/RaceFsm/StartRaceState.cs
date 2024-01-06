using Core.FSM;

namespace FsmStates.RaceFsm
{
    public class StartRaceState: FsmState
    {
        private RaceManager _raceManager;

        public StartRaceState(Fsm fsm, RaceManager raceManager) : base(fsm)
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
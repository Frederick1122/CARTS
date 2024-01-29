using Core.FSM;
using UI;
using UI.Windows.LapRace;

namespace FsmStates.RaceFsm
{
    public class StartRaceState : FsmState
    {
        private RaceManager _raceManager;

        public StartRaceState(Fsm fsm, RaceManager raceManager) : base(fsm) =>
            _raceManager = raceManager;

        public override void Enter()
        {
            //todo: make delay before start
            _raceManager.Init();
            _raceManager.StartRace();
            UIManager.Instance.GetRaceUi().ShowWindow(typeof(RaceWindowController), false);
            base.Enter();
            _fsm.SetState<RaceState>();
        }
    }
}
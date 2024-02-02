using Core.FSM;
using ProjectFsms;
using UI;
using UI.Windows.LapRace;

namespace FsmStates.RaceFsm
{
    public class StartRaceState : FsmState
    {
        private RaceFsmData _raceFsmData;

        public StartRaceState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;

        public override void Enter()
        {
            //todo: make delay before start
            _raceFsmData.raceManager.StartRace();

            //UIManager.Instance.GetRaceUi().ShowWindow(typeof(LapRaceLayoutController), false);
            UIManager.Instance.GetRaceUi().GetRaceLayout(_raceFsmData.raceType).Show();

            base.Enter();
            _fsm.SetState<InRaceState>();
        }
    }
}
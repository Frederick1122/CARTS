using Core.FSM;
using Race.RaceManagers;
using UI;

namespace FsmStates.RaceFsm
{
    public class PreInitializeState : FsmState
    {
        private RaceManager _raceManager;

        public PreInitializeState(Fsm fsm, RaceManager raceManager) : base(fsm) =>
            _raceManager = raceManager;

        public override void Enter()
        {
            base.Enter();
            
            _raceManager.SetState<LapRaceState>();
            _raceManager.InitState();

            UIManager.Instance.SetUiType(UiType.Race);
            UIManager.Instance.SetUiType(UiType.MobileLayout, false);

            _fsm.SetState<StartRaceState>();
        }
    }
}
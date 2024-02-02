using Core.FSM;
using ProjectFsms;
using Race.RaceManagers;
using UI;
using UI.Windows.FreeRide;
using UI.Windows.LapRace;
using UnityEngine.SceneManagement;

namespace FsmStates.RaceFsm
{
    public class PreInitializeState : FsmState
    {
        private const string LAP_RACE_SCENE = "LapRace";
        private const string FREE_RIDE_SCENE = "FreeRide";
        
        private RaceFsmData _raceFsmData;

        public PreInitializeState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;

        public override void Enter()
        {
            base.Enter();

            switch (SceneManager.GetActiveScene().name)
            {
                case LAP_RACE_SCENE:
                    _raceFsmData.raceType = RaceType.LAP_RACE;
                    break;
                case FREE_RIDE_SCENE:
                    _raceFsmData.raceType = RaceType.FREE_RIDE;
                    break;
            } 
            
            _raceFsmData.raceManager.SetState(_raceFsmData.raceType);
            _raceFsmData.raceManager.InitState();

            UIManager.Instance.SetUiType(UiType.Race);
            UIManager.Instance.GetRaceUi().GetRaceLayout(_raceFsmData.raceType).Show();
            UIManager.Instance.SetUiType(UiType.MobileLayout, false);

            _fsm.SetState<StartRaceState>();
        }
    }
}
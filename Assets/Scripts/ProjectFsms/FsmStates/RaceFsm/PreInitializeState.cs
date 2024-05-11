using ConfigScripts;
using Core.FSM;
using Installers;
using Managers;
using Managers.Libraries;
using ProjectFsms;
using Race.RaceManagers;
using UI;
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

            UIManager.Instance.SetActiveLoadingScreen(true);

            SceneType sceneType = SceneType.LapRace;
            
            switch (SceneManager.GetActiveScene().name)
            {
                case LAP_RACE_SCENE:
                    _raceFsmData.raceType = RaceType.LAP_RACE;
                    break;
                case FREE_RIDE_SCENE:
                    _raceFsmData.raceType = RaceType.FREE_RIDE;
                    sceneType = SceneType.FreeRide;
                    break;
            }

            SoundManager.Instance.PlayBackground(sceneType, TrackLibrary.Instance.GetConfig(_raceFsmData.gameData.gameModeData.trackKey).sound);

            _raceFsmData.raceManager.SetState(_raceFsmData.raceType);
            _raceFsmData.raceManager.InitState();

            UIManager.Instance.SetUiType(UiType.Race);
            UIManager.Instance.SetUiType(UiType.MobileLayout, false);
            
            _fsm.SetState<StartRaceState>();
        }
        
        public override void Exit()
        {
            base.Exit();
            
            UIManager.Instance.SetActiveLoadingScreen(false);
        }
    }
}
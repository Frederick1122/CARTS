using System;
using Core.FSM;
using Installers;
using UI.Windows.MapSelection;
using UnityEngine.SceneManagement;

namespace FsmStates.LobbyFsm
{
    public class StartGameState : FsmState
    {
        private const string LAP_RACE_SCENE = "LapRace";
        private const string FREE_RIDE_SCENE = "FreeRide";

        public StartGameState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();
            
            //In this moment we can async load new scene
            var sceneName = "";
            switch (((ProjectFsms.LobbyFsm)_fsm).gameData.gameType)
            {
                case GameDataInstaller.GameType.LapRace:
                    sceneName = LAP_RACE_SCENE;
                    break;
                case GameDataInstaller.GameType.FreeRide:
                    sceneName = FREE_RIDE_SCENE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            SceneManager.LoadScene(sceneName);
        }
    }
}
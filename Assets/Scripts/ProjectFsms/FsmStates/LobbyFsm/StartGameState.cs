using Core.FSM;
using Installers;
using System;
using Managers;
using UI;
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

            UIManager.Instance.SetActiveLoadingScreen(true);
            SoundManager.Instance.StopAllSound();

            //In this moment we can async load new scene
            var sceneName = ((ProjectFsms.LobbyFsm)_fsm).gameData.gameType switch
            {
                GameDataInstaller.GameType.LapRace => LAP_RACE_SCENE,
                GameDataInstaller.GameType.FreeRide => FREE_RIDE_SCENE,
                _ => throw new ArgumentOutOfRangeException(),
            };
            SceneManager.LoadScene(sceneName);
        }
    }
}
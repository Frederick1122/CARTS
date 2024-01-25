using System;
using Core.FSM;
using UI.Windows.MapSelection;
using UnityEngine.SceneManagement;

namespace FsmStates.LobbyFsm
{
    public class StartGameState : FsmState
    {
        private const string GAME_SCENE = "Game";
        private const string FREE_RIDE_SCENE = "FreeRideScene";

        public StartGameState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();
            //In this moment we can async load new scene
            var sceneName = "";
            switch (((ProjectFsms.LobbyFsm)_fsm).CurrentGameType)
            {
                case GameType.DefaultRace:
                    sceneName = GAME_SCENE;
                    break;
                case GameType.FreeRide:
                    sceneName = FREE_RIDE_SCENE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
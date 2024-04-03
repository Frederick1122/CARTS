using Core.FSM;
using Managers;
using ProjectFsms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FsmStates.RaceFsm
{
    public class StartLobbyState : FsmState
    {
        private const string LOBBY_SCENE = "Lobby";

        private RaceFsmData _raceFsmData;

        public StartLobbyState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;
        
        public override void Enter()
        {
            base.Enter();
            SceneManager.LoadScene(LOBBY_SCENE);
        }

        public override void Exit()
        {
            base.Exit();
            SoundManager.Instance.StopAllSound();
            Time.timeScale = 1f;
        }
    }
}
using Core.FSM;
using UnityEngine.SceneManagement;

namespace FsmStates.LobbyFsm
{
    public class StartGameState : FsmState
    {
        private const string GAME_SCENE = "Game";
        
        public StartGameState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();
            //In this moment we can async load new scene
            SceneManager.LoadScene(GAME_SCENE);
        }
    }
}
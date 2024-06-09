using Core.FSM;
using ProjectFsms;
using UI;
using UnityEngine;

namespace FsmStates.RaceFsm
{
    public class PauseState : FsmState
    {
        private readonly RaceFsmData _raceFsmData;

        public PauseState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 0;

            var controller = UIManager.Instance.GetRaceUi().GetPauseWindowController(_raceFsmData.raceType);
            controller.Show();
            controller.OnBackToLobby += GoToLobby;
            controller.OnResume += Resume;
        }

        public override void Exit()
        {
            base.Exit();
            Time.timeScale = 1;

            var controller = UIManager.Instance.GetRaceUi().GetPauseWindowController(_raceFsmData.raceType);
            controller.Hide();
            controller.OnBackToLobby -= GoToLobby;
            controller.OnResume -= Resume;
        }

        private void GoToLobby() => _fsm.SetState<StartLobbyState>();

        private void Resume() => _fsm.SetState<InRaceState>();
    }
}

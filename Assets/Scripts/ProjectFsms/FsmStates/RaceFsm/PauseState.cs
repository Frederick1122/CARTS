using Core.FSM;
using ProjectFsms;
using UI;

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
            var controller = UIManager.Instance.GetRaceUi().GetPauseWindowController(_raceFsmData.raceType);
            controller.Show();
            controller.OnBackToLobby += GoToLobby;
            controller.OnResume += Resume;
        }

        public override void Exit()
        {
            base.Exit();
            var controller = UIManager.Instance.GetRaceUi().GetPauseWindowController(_raceFsmData.raceType);
            controller.Hide();
            controller.OnBackToLobby -= GoToLobby;
            controller.OnResume -= Resume;
        }

        private void GoToLobby() => _fsm.SetState<StartLobbyState>();

        private void Resume() => _fsm.SetState<InRaceState>();
    }
}

using System;
using System.Threading;
using Core.FSM;
using ProjectFsms;
using UI;
using Cysharp.Threading.Tasks;

namespace FsmStates.RaceFsm
{
    public class StartRaceState : FsmState
    {
        private RaceFsmData _raceFsmData;
        private CancellationTokenSource _startDelayCts;
        private const int HIDDEN_DELAY = 2;
        private const int DELAY = 3;

        public StartRaceState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;

        public override void Enter()
        {
            _startDelayCts?.Cancel();
            _startDelayCts = new CancellationTokenSource();
            StartDelayTask(_startDelayCts.Token).Forget();

            base.Enter();
            _fsm.SetState<InRaceState>();
        }

        private async UniTaskVoid StartDelayTask(CancellationToken token)
        {
            var raceLayout = UIManager.Instance.GetRaceUi().GetRaceLayout(_raceFsmData.raceType);
            
            await UniTask.Delay(TimeSpan.FromSeconds(HIDDEN_DELAY), cancellationToken: token);
            
            raceLayout.Show();
            raceLayout.SetStartDelay(DELAY);
            
            await UniTask.Delay(TimeSpan.FromSeconds(DELAY), cancellationToken: token);
            
            _raceFsmData.raceManager.StartRace();
        }
    }
}
using System;
using System.Threading;
using Core.FSM;
using Cysharp.Threading.Tasks;
using Managers;
using ProjectFsms;
using Race.RaceManagers;
using UI;
using UnityEngine;

namespace FsmStates.RaceFsm
{
    public class FinishRaceState : FsmState
    {
        private RaceFsmData _raceFsmData;

        private readonly float _freezeLifetimeTicks = 10;
        private CancellationTokenSource _cancellationTokenSource;
        
        public FinishRaceState(Fsm fsm, RaceFsmData raceFsmData) : base(fsm) =>
            _raceFsmData = raceFsmData;

        ~FinishRaceState()
        {
            _cancellationTokenSource?.Cancel();
            UIManager.Instance.GetRaceUi().GetFinishWindowController(_raceFsmData.raceType).OnGoToMainMenuAction -= GoToMenu;
        }

        public override void Enter()
        {
            base.Enter();
            PlayerManager.Instance.IncreaseCurrency(CurrencyType.Soft, _raceFsmData.raceManager.GetResult());
            UIManager.Instance.GetRaceUi().GetFinishWindowController(_raceFsmData.raceType).Show();
            UIManager.Instance.GetRaceUi().GetFinishWindowController(_raceFsmData.raceType).OnGoToMainMenuAction += GoToMenu;
            _cancellationTokenSource = new CancellationTokenSource();
            FreezeTime(_cancellationTokenSource.Token).Forget();
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetFinishWindowController(_raceFsmData.raceType).Hide();
            UIManager.Instance.GetRaceUi().GetFinishWindowController(_raceFsmData.raceType).OnGoToMainMenuAction -= GoToMenu;
            RaceManager.Instance.DestroyRace();
            _cancellationTokenSource.Cancel();
        }

        private void GoToMenu()
        { 
            _fsm.SetState<StartLobbyState>();
        }
        
        private async UniTaskVoid FreezeTime(CancellationToken token)
        {
            for (var i = 1; i <= _freezeLifetimeTicks; i += 1)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token);
                Time.timeScale = 1 - i / _freezeLifetimeTicks;
            }
        }
    }
}
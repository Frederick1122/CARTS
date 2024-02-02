using System;
using System.Threading;
using Core.FSM;
using Cysharp.Threading.Tasks;
using Race.RaceManagers;
using UI;
using UI.Windows.LapRace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FsmStates.RaceFsm
{
    public class FinishRaceState : FsmState
    {
        private const string LOBBY_SCENE = "Lobby";

        private readonly float _freezeLifetimeTicks = 10;
        private readonly CancellationTokenSource _positionCts = new();
        
        public FinishRaceState(Fsm fsm) : base(fsm) { }

        ~FinishRaceState()
        {
            _positionCts.Cancel();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<LapRaceLayoutController>().OnGoToMainMenuAction -= GoToMenu;
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<LapRaceLayoutController>().Show();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<LapRaceLayoutController>().OnGoToMainMenuAction += GoToMenu;
            FreezeTime(_positionCts.Token).Forget();
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<LapRaceLayoutController>().Hide();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<LapRaceLayoutController>().OnGoToMainMenuAction -= GoToMenu;
            RaceManager.Instance.DestroyRace();
            _positionCts.Cancel();
            Time.timeScale = 1f;
        }

        private void GoToMenu()
        { 
            SceneManager.LoadScene(LOBBY_SCENE);
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
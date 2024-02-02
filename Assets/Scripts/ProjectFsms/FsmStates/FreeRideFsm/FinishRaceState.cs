using System;
using System.Threading;
using Core.FSM;
using Cysharp.Threading.Tasks;
using UI;
using UI.Windows.FreeRide;
using UI.Windows.LapRace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FsmStates.FreeRideFsm
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
            UIManager.Instance.GetRaceUi().GetFinishWindowController<FreeRideLayoutController>().OnGoToMainMenuAction -= GoToMenu;
        }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter finish");
            UIManager.Instance.GetRaceUi().GetFinishWindowController<FreeRideLayoutController>().Show();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<FreeRideLayoutController>().OnGoToMainMenuAction += GoToMenu;
            FreezeTime(_positionCts.Token).Forget();
        }

        public override void Exit()
        {
            base.Exit();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<FreeRideLayoutController>().Hide();
            UIManager.Instance.GetRaceUi().GetFinishWindowController<FreeRideLayoutController>().OnGoToMainMenuAction -= GoToMenu;
            _positionCts.Cancel();
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

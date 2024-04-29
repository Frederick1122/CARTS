using Core.FSM;
using Managers;
using UI;
using UnityEngine;

namespace FsmStates.LobbyFsm
{
    public class PreInitializeState : FsmState
    {
        public PreInitializeState(Fsm fsm) : base(fsm) { }

        public override void Enter()
        {
            base.Enter();
            //In this moment we can get stats or something else

            UIManager.Instance.SetActiveLoadingScreen(true);
            
            Debug.Log("preinit start");

            if (SoundManager.Instance.IsSoundBanksLoaded())
            {
                StartLobbyState();
                return;
            }

            Debug.Log("preinit waiting");
            SoundManager.Instance.OnSoundBanksLoaded += StartLobbyState;
        }

        public override void Exit()
        {
            base.Exit();
            
            UIManager.Instance.SetActiveLoadingScreen(false);
        }

        private void StartLobbyState()
        {
            Debug.Log("preinit start lobby state");

            SoundManager.Instance.OnSoundBanksLoaded -= StartLobbyState;

            UIManager.Instance.SetUiType(UiType.Lobby);
            SoundManager.Instance.PlayBackground(SceneType.Lobby);
            _fsm.SetState<LobbyState>();
        }
    }
}
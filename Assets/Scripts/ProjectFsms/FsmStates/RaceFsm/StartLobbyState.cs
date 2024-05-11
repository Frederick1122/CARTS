﻿using Core.FSM;
using Managers;
using ProjectFsms;
using UI;
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
            _raceFsmData.raceManager.FinishRace();

            UIManager.Instance.SetActiveLoadingScreen(true);
            UIManager.Instance.GetRaceUi().HideAll();
            
            SoundManager.Instance.StopAllSound();

            SceneManager.LoadScene(LOBBY_SCENE);
        }

        public override void Exit()
        {
            base.Exit();
            Time.timeScale = 1f;
        }
    }
}
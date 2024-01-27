﻿using Core.FSM;
using Installers;
using UI;
using UI.Windows.MapSelection;

namespace FsmStates.LobbyFsm
{
    public class MapSelectionState : FsmState
    {
        private LobbyUI _lobbyUI;

        public MapSelectionState(Fsm fsm, LobbyUI lobbyUI) : base(fsm)
        {
            _lobbyUI = lobbyUI;
            _lobbyUI.OpenLobbyAction += OpenLobby;
            _lobbyUI.GoToGameAction += GoToGame;
        }

        ~MapSelectionState()
        {
            if (_lobbyUI == null)
                return;

            _lobbyUI.OpenLobbyAction -= OpenLobby;
            _lobbyUI.GoToGameAction -= GoToGame;
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.GetLobbyUi().ShowWindow(typeof(MapSelectionWindowController), true);
        }

        private void OpenLobby()
        {
            _fsm.SetState<LobbyState>();
        }

        private void GoToGame()
        {
            _fsm.SetState<StartGameState>();
        }
    }
}
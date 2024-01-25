using System;
using Base;
using UnityEngine;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LobbyUI _lobbyUI;
        [SerializeField] private RaceUI _raceUI;

        public void Init()
        {
            _lobbyUI.Init();
            _raceUI.Init();
        }
        
        public LobbyUI GetLobbyUi()
        {
            return _lobbyUI;
        }

        public RaceUI GetRaceUi()
        {
            return _raceUI;
        }

        public void SetUiType(UiType uiType)
        {
            _lobbyUI.gameObject.SetActive(false);
            _raceUI.gameObject.SetActive(false);
            
            switch (uiType)
            {
                case UiType.Lobby:
                    _lobbyUI.gameObject.SetActive(true);
                    break;
                case UiType.Race:
                    _raceUI.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uiType), uiType, null);
            }
        }
    }

    public enum UiType
    {
        Lobby, 
        Race
    }
}
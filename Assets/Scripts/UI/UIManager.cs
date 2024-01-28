using Base;
using System;
using UnityEngine;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LobbyUI _lobbyUI;
        [SerializeField] private RaceUI _raceUI;
        [SerializeField] private FreeRideUI _freeRideUI;

        public void Init()
        {
            _lobbyUI.Init();
            _raceUI.Init();
            _freeRideUI.Init();
        }

        public LobbyUI GetLobbyUi()
        {
            return _lobbyUI;
        }

        public RaceUI GetRaceUi()
        {
            return _raceUI;
        }

        public FreeRideUI GetFreeRideUI()
        {
            return _freeRideUI;
        }

        public void SetUiType(UiType uiType)
        {
            _lobbyUI.gameObject.SetActive(false);
            _raceUI.gameObject.SetActive(false);
            _freeRideUI.gameObject.SetActive(false);

            switch (uiType)
            {
                case UiType.Lobby:
                    _lobbyUI.gameObject.SetActive(true);
                    break;
                case UiType.Race:
                    _raceUI.gameObject.SetActive(true);
                    break;
                case UiType.FreeRide:
                    _freeRideUI.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uiType), uiType, null);
            }
        }
    }

    public enum UiType
    {
        Lobby,
        Race,
        FreeRide
    }
}
using Base;
using System;
using UnityEngine;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LobbyUI _lobbyUI;
        [SerializeField] private MobileLayoutUI _mobileLayoutUI;

        [SerializeField] private RaceUI _raceUI;
        //[SerializeField] private FreeRideUI _freeRideUI;

        public void Init()
        {
            _lobbyUI.Init();
            _raceUI.Init();
            //_freeRideUI.Init();
            _mobileLayoutUI.Init();
        }

        public LobbyUI GetLobbyUi()
        {
            return _lobbyUI;
        }

        public RaceUI GetRaceUi()
        {
            return _raceUI;
        }

        // public FreeRideUI GetFreeRideUI()
        // {
        //     return _freeRideUI;
        // }

        public MobileLayoutUI GetMobileLayoutUI()
        {
            return _mobileLayoutUI;
        }

        public void SetUiType(UiType uiType, bool hideOther = true)
        {
            if (hideOther)
            {
                _lobbyUI.gameObject.SetActive(false);
                _raceUI.gameObject.SetActive(false);
                _mobileLayoutUI.gameObject.SetActive(false);
            }

            switch (uiType)
            {
                case UiType.Lobby:
                    _lobbyUI.gameObject.SetActive(true);
                    break;
                case UiType.Race:
                    _raceUI.gameObject.SetActive(true);
                    break;
                case UiType.MobileLayout:
                    _mobileLayoutUI.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uiType), uiType, null);
            }
        }
    }

    public enum UiType
    {
        Lobby = 0,
        MobileLayout = 1,
        Race = 2
    }
}
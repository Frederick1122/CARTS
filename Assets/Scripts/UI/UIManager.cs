using Base;
using System;
using UI.Widgets;
using UnityEngine;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LobbyUI _lobbyUI;
        [SerializeField] private RaceUI _raceUI;
        [SerializeField] private WidgetUi _widgetUi;
        [SerializeField] private MobileLayoutUI _mobileLayoutUI;

        public void Init()
        {
            _widgetUi.Init();
            _lobbyUI.Init();
            _raceUI.Init();
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

        public WidgetUi GetWidgetUI()
        {
            return _widgetUi;
        }

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
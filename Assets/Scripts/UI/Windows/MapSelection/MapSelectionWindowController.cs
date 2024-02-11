using Installers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Elements;
using UnityEngine;
using Zenject;

namespace UI.Windows.MapSelection
{
    public class MapSelectionWindowController : UIController
    {
        private const string DEFAULT_RACE_MODE = "Default race";
        private const string FREE_RIDE_MODE = "Free ride";

        private const string ONE_LAP_MODE = "One lap";
        private const string THREE_LAP_MODE = "Three laps";
        private const string ONE_BOT_MODE = "One bot";
        private const string THREE_BOT_MODE = "Three bots";

        public event Action OpenLobbyAction = delegate { };
        public event Action GoToGameAction = delegate { };
        [Space]
        [SerializeField] private ToggleCustomScroll _toggleCustomScroll;
        [Space]
        [SerializeField] private CustomToggleController _defaultRaceToggleController;
        [SerializeField] private CustomToggleController _freeRideToggleController;
        [Space]
        [SerializeField] private CustomToggleController _oneLapToggleController;
        [SerializeField] private CustomToggleController _threeLapsToggleController;
        [SerializeField] private CustomToggleController _oneBotRaceToggleController;
        [SerializeField] private CustomToggleController _threeBotsToggleController;

        [Inject] private GameDataInstaller.GameData _gameData;

        [Inject] private readonly GameDataInstaller.LapRaceGameData _defaultLapRaceGameData;
        [Inject] private readonly GameDataInstaller.FreeRideGameData _defaultFreeRideGameData;
        
        private GameDataInstaller.LapRaceGameData _lapRaceGameData;
        private GameDataInstaller.FreeRideGameData _freeRideGameData;

        private CustomToggleModel _currentCustomToggleModel;
        private List<CustomToggleModel> _trackModels = new();
        private GameDataInstaller.GameType _gameType;
        private int _lapCount;
        private int _botCount;

        public override void Init()
        {
            GetView<MapSelectionWindowView>().OpenLobbyAction += OpenLobby;
            GetView<MapSelectionWindowView>().GoToGameAction += GoToGame;
            _toggleCustomScroll.OnSelectAction += SelectNewTrack;
            _defaultRaceToggleController.OnSelectAction += SetDefaultRaceState;
            _freeRideToggleController.OnSelectAction += SetFreeRideState;

            _oneLapToggleController.OnSelectAction += SetOneLapMode;
            _threeLapsToggleController.OnSelectAction += SetThreeLapsMode;
            _oneBotRaceToggleController.OnSelectAction += SetOneBotMode;
            _threeBotsToggleController.OnSelectAction += SetThreeBotsMode;

            _lapRaceGameData = _defaultLapRaceGameData;
            _freeRideGameData = _defaultFreeRideGameData;
            
            _oneLapToggleController.Init();
            _threeLapsToggleController.Init();
            _oneBotRaceToggleController.Init();
            _threeBotsToggleController.Init();

            _defaultRaceToggleController.Init();
            _freeRideToggleController.Init();

            _defaultRaceToggleController.UpdateView(new CustomToggleModel(DEFAULT_RACE_MODE, true));
            _freeRideToggleController.UpdateView(new CustomToggleModel(FREE_RIDE_MODE, false));

            _oneLapToggleController.UpdateView(new CustomToggleModel(ONE_LAP_MODE, true));
            _threeLapsToggleController.UpdateView(new CustomToggleModel(THREE_LAP_MODE, false));
            _oneBotRaceToggleController.UpdateView(new CustomToggleModel(ONE_BOT_MODE, false));
            _threeBotsToggleController.UpdateView(new CustomToggleModel(THREE_BOT_MODE, true));

            InitAllTracks();
            base.Init();
        }

        public override void UpdateView() => UpdateAllTracks();

        protected override UIModel GetViewData()
        {
            return new MapSelectionWindowModel();
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                GetView<MapSelectionWindowView>().OpenLobbyAction -= OpenLobby;
                GetView<MapSelectionWindowView>().GoToGameAction -= GoToGame;
            }

            if (_toggleCustomScroll != null)
                _toggleCustomScroll.OnSelectAction -= SelectNewTrack;

            if (_defaultRaceToggleController != null)
                _defaultRaceToggleController.OnSelectAction -= SetDefaultRaceState;

            if (_toggleCustomScroll != null)
                _toggleCustomScroll.OnSelectAction -= SetFreeRideState;

            if (_oneLapToggleController != null)
                _oneLapToggleController.OnSelectAction -= SetOneLapMode;

            if (_threeLapsToggleController != null)
                _threeLapsToggleController.OnSelectAction -= SetThreeLapsMode;

            if (_oneBotRaceToggleController != null)
                _oneBotRaceToggleController.OnSelectAction -= SetOneBotMode;

            if (_threeBotsToggleController != null)
                _threeBotsToggleController.OnSelectAction -= SetThreeBotsMode;
        }

        private void OpenLobby() =>
            OpenLobbyAction?.Invoke();

        private void GoToGame()
        {
            _gameData.gameType = _gameType;

            _gameData.gameModeData = _gameType switch
            {
                GameDataInstaller.GameType.LapRace => _lapRaceGameData,
                GameDataInstaller.GameType.FreeRide => _freeRideGameData,
                _ => throw new ArgumentOutOfRangeException()
            };

            GoToGameAction?.Invoke();
        }

        private void SelectNewTrack(CustomToggleModel uiModel)
        {
            if (_currentCustomToggleModel?.text == uiModel.text)
                return;

            _lapRaceGameData.trackKey = uiModel.key;
            _currentCustomToggleModel = uiModel;
            UpdateAllTracks();
        }

        private void SetThreeLapsMode(CustomToggleModel uiModel)
        {
            _oneLapToggleController.Unselect();
            SetMode(3, ref _lapRaceGameData.lapCount);
        }

        private void SetOneLapMode(CustomToggleModel uiModel)
        {
            _threeLapsToggleController.Unselect();
            SetMode(1, ref _lapRaceGameData.lapCount);
        }

        private void SetThreeBotsMode(CustomToggleModel uiModel)
        {
            _oneBotRaceToggleController.Unselect();
            SetMode(3, ref _lapRaceGameData.botCount);
        }

        private void SetOneBotMode(CustomToggleModel uiModel)
        {
            _threeBotsToggleController.Unselect();
            SetMode(1, ref _lapRaceGameData.botCount);
        }

        private void SetMode(int count, ref int counter) =>
            counter = count;

        private void SetDefaultRaceState(CustomToggleModel customToggleModel)
        {
            _freeRideToggleController.Unselect();
            SetGameState(GameDataInstaller.GameType.LapRace);
        }

        private void SetFreeRideState(CustomToggleModel customToggleModel)
        {
            _defaultRaceToggleController.Unselect();
            SetGameState(GameDataInstaller.GameType.FreeRide);
        }

        private void SetGameState(GameDataInstaller.GameType gameType) =>
            _gameType = gameType;

        private void InitAllTracks()
        {
            var trackConfigs = TrackLibrary.Instance.GetAllConfigs();
            var currentTrackConfigKey = _lapRaceGameData.trackKey;
            foreach (var trackConfig in trackConfigs)
            {
                var isSelectedTrack = trackConfig.configKey == currentTrackConfigKey;
                _trackModels.Add(new CustomToggleModel(TrackLibrary.Instance.GetConfig(trackConfig.configKey).configName, trackConfig.configKey, isSelectedTrack));

                if (isSelectedTrack)
                    _currentCustomToggleModel = _trackModels[^1];
            }

            _toggleCustomScroll.AddRange(_trackModels);
        }

        private void UpdateAllTracks()
        {
            foreach (var carModel in _trackModels)
                carModel.isSelected = carModel.text == _currentCustomToggleModel.text;

            _toggleCustomScroll.HideAll();
            _toggleCustomScroll.AddRange(_trackModels);
        }
    }
}
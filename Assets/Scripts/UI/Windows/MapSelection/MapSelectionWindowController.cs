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
    public class MapSelectionWindowController : UIController<MapSelectionWindowView, MapSelectionWindowModel>
    {
        private const string DEFAULT_RACE_MODE = "Default race";
        private const string FREE_RIDE_MODE = "Free ride";
        
        public event Action OpenLobbyAction = delegate {  };
        public event Action<GameType> GoToGameAction = delegate {  };

        [SerializeField] private ToggleCustomScroll _toggleCustomScroll;
        
        [SerializeField] private CustomToggleController _defaultRaceToggleController;
        [SerializeField] private CustomToggleController _freeRideToggleController;

        [Inject] private TrackData _trackData;

        private CustomToggleModel _currentCustomToggleModel;
        private List<CustomToggleModel> _trackModels = new();
        private GameType _gameType;

        public override void Init()
        {
            _view.OpenLobbyAction += OpenLobby;
            _view.GoToGameAction += GoToGame;
            _toggleCustomScroll.OnSelectAction += SelectedNewToggle;
            _defaultRaceToggleController.OnSelectAction += SetDefaultRaceState;
            _freeRideToggleController.OnSelectAction += SetFreeRideState;
            
            _defaultRaceToggleController.Init();
            _freeRideToggleController.Init();
            
            _defaultRaceToggleController.UpdateView(new CustomToggleModel(DEFAULT_RACE_MODE, true));
            _freeRideToggleController.UpdateView(new CustomToggleModel(FREE_RIDE_MODE, false));
            
            InitAllTracks();
            base.Init();
        }

        public override void UpdateView()
        {
            UpdateAllTracks();
        }

        protected override MapSelectionWindowModel GetViewData()
        {
            return new MapSelectionWindowModel();
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                _view.OpenLobbyAction -= OpenLobby;
                _view.GoToGameAction -= GoToGame;
            }

            if (_toggleCustomScroll != null)
                _toggleCustomScroll.OnSelectAction -= SelectedNewToggle;
            
            if (_defaultRaceToggleController != null)
                _defaultRaceToggleController.OnSelectAction -= SetDefaultRaceState;
            
            if (_toggleCustomScroll != null)
                _toggleCustomScroll.OnSelectAction -= SetFreeRideState;
        }

        private void OpenLobby()
        {
            OpenLobbyAction?.Invoke();
        }

        private void GoToGame()
        {
            GoToGameAction?.Invoke(_gameType);
        }

        private void SelectedNewToggle(CustomToggleModel uiModel)
        {
            if (_currentCustomToggleModel.text == uiModel.text)
                return;

            _trackData.configKey = uiModel.text;
            _currentCustomToggleModel = uiModel;
            UpdateAllTracks();
        }
        
        private void SetDefaultRaceState(CustomToggleModel customToggleModel) => SetGameState(GameType.DefaultRace);

        private void SetFreeRideState(CustomToggleModel customToggleModel) => SetGameState(GameType.FreeRide);

        private void SetGameState(GameType gameType)
        {
            switch (_gameType)
            {
                case GameType.DefaultRace:
                    _defaultRaceToggleController.Unselect();
                    break;
                case GameType.FreeRide:
                    _freeRideToggleController.Unselect();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _gameType = gameType;
        }

        private void InitAllTracks()
        {
            var trackConfigs = TrackLibrary.Instance.GetAllConfigs().Values.ToList();
            var currentTrackConfigKey = _trackData.configKey;
            foreach (var trackConfig in trackConfigs)
            {
                var isSelectedTrack = trackConfig.configKey == currentTrackConfigKey;
                _trackModels.Add(new CustomToggleModel(TrackLibrary.Instance.GetConfig(trackConfig.configKey).configName, isSelectedTrack));

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

    public enum GameType
    {
        DefaultRace,
        FreeRide
    }
}
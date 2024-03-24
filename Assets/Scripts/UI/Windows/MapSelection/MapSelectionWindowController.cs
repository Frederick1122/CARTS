using ConfigScripts;
using Installers;
using Managers;
using Managers.Libraries;
using Swiper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace UI.Windows.MapSelection
{
    public class MapSelectionWindowController : UIController
    {
        private const string MAP_PATH = "Configs/Mods";

        public event Action OpenLobbyAction = delegate { };
        public event Action GoToGameAction = delegate { };

        [Inject] private readonly GameDataInstaller.GameData _gameData;

        [Inject] private readonly GameDataInstaller.LapRaceGameData _defaultLapRaceGameData;
        [Inject] private readonly GameDataInstaller.FreeRideGameData _defaultFreeRideGameData;
        
        private GameDataInstaller.LapRaceGameData _lapRaceGameData;
        private GameDataInstaller.FreeRideGameData _freeRideGameData;

        private Dictionary<string, ModeConfig> _keyModePairs = new();
        private GameDataInstaller.GameType _gameType = GameDataInstaller.GameType.LapRace; 

        public override void Init()
        {
            var castView = GetView<MapSelectionWindowView>();

            castView.OpenLobbyAction += OpenLobby;

            castView.OnModSelect += SelectMode;
            castView.OnMapSelect += SelectMap;

            castView.OnModeSwipe += CheckIfCanBeParticipant;

            _lapRaceGameData = _defaultLapRaceGameData;
            _freeRideGameData = _defaultFreeRideGameData;

            var allMods = Resources.LoadAll(MAP_PATH, typeof(ModeConfig));
            foreach (var item in allMods)
            {
                var castItem = (ModeConfig)item;
                _keyModePairs.Add(castItem.configKey, castItem);
            }

            base.Init();
        }

        public override void Show()
        {
            AddMods();

            GetView<MapSelectionWindowView>().ShowModSelection();
            base.Show();
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                var castView = GetView<MapSelectionWindowView>();

                castView.OpenLobbyAction -= OpenLobby;
                castView.OnModeSwipe -= CheckIfCanBeParticipant;
                castView.OnModSelect -= SelectMode;
                castView.OnMapSelect -= SelectMap;
            }
        }

        protected override UIModel GetViewData()
        {
            return new MapSelectionWindowModel();
        }

        private void AddMods()
        {
            var view = GetView<MapSelectionWindowView>();
            view.ClearModSwiper();

            foreach (var item in _keyModePairs.Values)
            {
                var data = new SwiperData(item.configKey, item.Icon, item.configName);
                view.AddMod(data);
            }
        }

        private void AddMaps(string key)
        {
            var view = GetView<MapSelectionWindowView>();
            view.ClearMapSwiper();

            var mode = _keyModePairs[key];
            foreach (var map in mode.Maps)
            {
                var data = new SwiperData(map.configKey, null, map.configName);
                view.AddMap(data);
            }
        }

        private void SelectMode(string key)
        {
            _gameType = _keyModePairs[key].GetGameType();
            SetLapCount(_keyModePairs[key].GetLapCount());
            SetBotCount(_keyModePairs[key].GetLapCount());
            AddMaps(key);

            GetView<MapSelectionWindowView>().ShowMapSelection();
        }

        private void CheckIfCanBeParticipant(string modeKey)
        {
            var modeClass = _keyModePairs[modeKey].GetCarClass();
            var carClass = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey).Rarity;

            var castView = GetView<MapSelectionWindowView>();

            if (modeClass == carClass || modeClass == Rarity.Default)
                castView.OpenMode();
            else
                castView.CloseMode();
        }

        private void SelectMap(string key)
        {
            SelectTrack(key);
            GoToGame();
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

        private void SelectTrack(string key)
        {
            switch(_gameType)
            {
                case GameDataInstaller.GameType.LapRace:
                    _lapRaceGameData.trackKey = key;
                    break;

                case GameDataInstaller.GameType.FreeRide:
                    _freeRideGameData.trackKey = key;
                    break;
            }
        }

        private void SetLapCount(int count)
        {
            switch (_gameType)
            {
                case GameDataInstaller.GameType.LapRace:
                    _lapRaceGameData.lapCount = count;
                    break;
            }
        }

        private void SetBotCount(int count)
        {
            switch (_gameType)
            {
                case GameDataInstaller.GameType.LapRace:
                    _lapRaceGameData.botCount = count;
                    break;
            }
        }
    }
}
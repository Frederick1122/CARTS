using ConfigScripts;
using Installers;
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
            GetView<MapSelectionWindowView>().OpenLobbyAction += OpenLobby;

            GetView<MapSelectionWindowView>().OnModSelect += SelectMode;
            GetView<MapSelectionWindowView>().OnMapSelect += SelectMap;

            _lapRaceGameData = _defaultLapRaceGameData;
            _freeRideGameData = _defaultFreeRideGameData;

            var allMods = Resources.LoadAll(MAP_PATH, typeof(ModeConfig));
            foreach (var item in allMods)
            {
                var castItem = (ModeConfig)item;
                _keyModePairs.Add(castItem.configKey, castItem);
            }

            AddAllMods();
            base.Init();
        }

        public override void Show()
        {
            GetView<MapSelectionWindowView>().ShowModSelection();
            base.Show();
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                GetView<MapSelectionWindowView>().OpenLobbyAction -= OpenLobby;
                //GetView<MapSelectionWindowView>().GoToGameAction -= GoToGame;
            }
        }

        protected override UIModel GetViewData()
        {
            return new MapSelectionWindowModel();
        }

        private void AddAllMods()
        {
            foreach (var item in _keyModePairs.Values)
            {
                var data = new SwiperData(item.configKey, item.Icon, item.configName);
                GetView<MapSelectionWindowView>().AddMod(data);
            }
        }

        private void AddAllMaps()
        {
            var maps = GetMaps();
            foreach (var item in maps)
            {
                var data = new SwiperData(item.configKey, null, item.configName);
                GetView<MapSelectionWindowView>().AddMap(data);
            }
        }

        private void SelectMode(string key)
        {
            _gameType = _keyModePairs[key].GetGameType();
            SetLapCount(_keyModePairs[key].GetLapCount());
            SetBotCount(_keyModePairs[key].GetLapCount());
            AddAllMaps();

            GetView<MapSelectionWindowView>().ShowMapSelection();
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

        private List<BaseConfig> GetMaps()
        {
            var maps = new List<BaseConfig>();
            switch (_gameType)
            {
                case GameDataInstaller.GameType.LapRace:
                    maps = (List<BaseConfig>)TrackLibrary.Instance.GetAllConfigs();
                    break;

                case GameDataInstaller.GameType.FreeRide:
                    maps = (List<BaseConfig>)FreeRideTrackLibrary.Instance.GetAllConfigs();
                    break;
            }
            return maps;
        }
    }
}
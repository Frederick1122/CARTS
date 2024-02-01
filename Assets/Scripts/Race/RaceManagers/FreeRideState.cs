using System;
using Cars.Controllers;
using Cars.InputSystem;
using Cars.InputSystem.Player;
using FreeRide;
using FreeRide.Map;
using Installers;
using Managers;
using Managers.Libraries;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Race.RaceManagers
{
    public class FreeRideState : RaceState
    {
        private const string PLAYER_PRESET_NAME = "PlayerPreset";

        public event Action<int> OnResultUpdateAction = delegate {  };

        [Inject] private GameDataInstaller.GameData _gameData;
        [Inject] private GameDataInstaller.FreeRideGameData _defaultFreeRideGameData;

        private GameDataInstaller.FreeRideGameData _freeRideGameData;
        
        private CarController _player;

        private Transform _startPosition;
        private MapFabric _mapFabric;
        private DifficultyModifier _difficultyModifier;
        private FreeRidePrefabData _currentTrack;

        public override void Init()
        {
            if (_gameData.gameModeData is GameDataInstaller.FreeRideGameData freeRideGameData)
            {
                _freeRideGameData = freeRideGameData;
            }
            else
            {
                Debug.LogWarning("ALERT! TEST MODE! Game initialize in base config");
                _freeRideGameData = _defaultFreeRideGameData;
            }

            InitTrack();
            InitPlayer();
            _mapFabric.Init();
            _mapFabric.OnResultUpdate += UpdateResult;
            _mapFabric.OnFall += PlayerFall;

            _difficultyModifier.Init(_player, this);
        }

        public override void Destroy()
        {
            if (_mapFabric == null)
                return;
            
            _mapFabric.OnResultUpdate -= UpdateResult;
            _mapFabric.OnFall -= PlayerFall;
        }
        
        public override void StartRace()
        {
            _player.StartCar();
            base.StartRace();
        }

        public override void FinishRace()
        {
            
        }

        private void InitPlayer()
        {
            var playerConfig = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey);
            var playerPreset = PresetLibrary.Instance.GetConfig(PLAYER_PRESET_NAME);
            var playerPrefab = playerConfig.prefab;
            var player = Object.Instantiate(playerPrefab, _startPosition);
            _player = (CarController)player.gameObject.AddComponent(playerPreset.CarController);

            var playerInputSystem = (IInputSystem)_player.gameObject.AddComponent(typeof(FreeRideInputSystem));
            playerInputSystem.Init(playerPreset, playerPrefab);

            _player.Init(playerInputSystem, playerConfig, playerPreset);
        }

        private void InitTrack()
        {
            var trackConfig = FreeRideTrackLibrary.Instance.GetConfig(_freeRideGameData.trackKey);
            _currentTrack = Object.Instantiate(trackConfig.freeRidePrefab);
            
            _startPosition = _currentTrack.startPosition;
            _mapFabric = _currentTrack.mapFabric;
            _difficultyModifier = _currentTrack.difficultyModifier;
        }

        private void PlayerFall()
        {
            _player.StopCar();
            FinishRace();
        }

        private void UpdateResult(int val)
        {
            Debug.Log($"Result {val}");
            OnResultUpdateAction?.Invoke(val);
        }
    }
}

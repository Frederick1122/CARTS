using Cars.Controllers;
using Cars.InputSystem;
using Cars.InputSystem.Player;
using Cysharp.Threading.Tasks;
using Installers;
using Managers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Race.RaceManagers
{
    public class LapRaceState : RaceState
    {
        private const string PLAYER_PRESET_NAME = "PlayerPreset";
        
        public event Action<int> OnPlayerChangePositionAction = delegate { };
        public event Action OnPlayerEndsLapAction = delegate { };

        [Inject] private GameDataInstaller.GameData _gameData;
        [Inject] private GameDataInstaller.LapRaceGameData _defaultLapRaceGameData;

        private GameDataInstaller.LapRaceGameData _lapRaceGameData;

        private CarController _player;

        private Track _currentTrack;
        private readonly List<CarController> _enemies = new();
        private readonly List<int> _lapsStats = new();

        private int _lastPlayerPosition;
        private readonly float _checkPositionDelay = 0.1f;
        private readonly CancellationTokenSource _positionCts = new();
        private DateTime _startTime;

        public override void Init()
        {
            if (_gameData.gameModeData is GameDataInstaller.LapRaceGameData lapRaceGameData)
            {
                _lapRaceGameData = lapRaceGameData;
            }
            else
            {
                Debug.LogWarning("ALERT! TEST MODE! Game initialize in base config");
                _lapRaceGameData = _defaultLapRaceGameData;
            }

            InitTrack();
            InitPlayer();
            InitAi();
        }

        public override void Destroy()
        {
            _positionCts.Cancel();
            _enemies.Clear();
            _lapsStats.Clear();
            
            Object.Destroy(_currentTrack.gameObject); 
            Object.Destroy(_player.gameObject); 
        }

        public override void StartRace()
        {
            foreach (var en in _enemies)
                en.StartCar();

            _player.StartCar();
            CheckPlayerPosition(_positionCts.Token).Forget();
            _startTime = DateTime.Now;
            base.StartRace();
        }

        public override void FinishRace()
        {
            _positionCts.Cancel();
            base.FinishRace();
        }

        public int GetMaxLapCount() =>
            _lapRaceGameData.lapCount;

        public int GetMaxPositions() =>
            _lapRaceGameData.botCount + 1;

        public int GetPlayerPosition()
        {
            var playerPassedDistance = _player.GetPassedDistance();
            var playerPosition = GetMaxPositions();

            foreach (var enemy in _enemies)
            {
                var enemyPassedDistance = enemy.GetPassedDistance();

                if (playerPassedDistance > enemyPassedDistance)
                    playerPosition--;
            }

            return playerPosition;
        }

        public TimeSpan GetPassTime()
        {
            var dateTime = DateTime.Now - _startTime;
            return new TimeSpan(dateTime.Days, dateTime.Hours, dateTime.Minutes, dateTime.Seconds,
                dateTime.Milliseconds);
        }

        private void UpdateLapStats(int lapStatsIndex)
        {
            _lapsStats[lapStatsIndex]++;
            if (lapStatsIndex == 0)
            {
                Debug.Log("PLAYER ENDS LAP");
                OnPlayerEndsLapAction?.Invoke();
                
                if (GetMaxLapCount() <= _lapsStats[lapStatsIndex]) 
                    FinishRace();
            }
            else
                Debug.Log($"BOT {lapStatsIndex} ENDS LAP");
        }

        private async UniTaskVoid CheckPlayerPosition(CancellationToken token)
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_checkPositionDelay), cancellationToken: token);
                var playerPosition = GetPlayerPosition();

                if (playerPosition == _lastPlayerPosition)
                    continue;

                OnPlayerChangePositionAction?.Invoke(playerPosition);
                _lastPlayerPosition = playerPosition;
            }
        }

        #region initialization

        protected void InitPlayer()
        {
            _lapsStats.Add(0);
            var playerConfig = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey);
            var playerPreset = PresetLibrary.Instance.GetConfig(PLAYER_PRESET_NAME);
            var playerPrefab = playerConfig.prefab;
            var spawnPlayerData = _currentTrack.SpawnPlayer(playerPrefab);
            _player = (CarController)spawnPlayerData.car.gameObject.AddComponent(playerPreset.CarController);

            var playerInputSystem = (IInputSystem)_player.gameObject.AddComponent(typeof(AutoGasInputSystem));
            playerInputSystem.Init(playerPreset, playerPrefab);

            var waypointTracker =
                _player.gameObject.AddComponent<WaypointProgressTracker>();
            waypointTracker.Circuit = spawnPlayerData.circuit;
            waypointTracker.OnLapEndAction += () => UpdateLapStats(0);

            waypointTracker.Init(_player, playerInputSystem);
            _player.Init(playerInputSystem, playerConfig, playerPreset, waypointTracker);
        }

        private void InitAi()
        {
            var enemyConfigs = CarLibrary.Instance.GetRandomsConfigs(_currentTrack.GetCarPlacesCount() - 1);

            var spawnEnemyDatas = _currentTrack.SpawnAiTrucks(enemyConfigs, _lapRaceGameData.botCount);

            for (var i = 0; i < spawnEnemyDatas.Count; i++)
            {
                var enemyPreset = PresetLibrary.Instance.GetRandomConfig(PLAYER_PRESET_NAME);
                _enemies.Add((CarController)spawnEnemyDatas[i].car.gameObject.AddComponent(enemyPreset.CarController));

                var aiInputSystem = (IInputSystem)_enemies[i].gameObject.AddComponent(enemyPreset.InputSystem);
                aiInputSystem.Init(enemyPreset, spawnEnemyDatas[i].car);

                var waypointTracker = _enemies[i].gameObject.AddComponent<WaypointProgressTracker>();
                var lapStatsIndex = i + 1;
                waypointTracker.OnLapEndAction += () => UpdateLapStats(lapStatsIndex);
                waypointTracker.Circuit = spawnEnemyDatas[i].circuit;
                _lapsStats.Add(0);
                waypointTracker.Init(_enemies[i], aiInputSystem);

                _enemies[i].Init(aiInputSystem, enemyConfigs[i], PresetLibrary.Instance.GetRandomConfig(),
                    waypointTracker);
            }
        }

        private void InitTrack()
        {
            var trackConfig = TrackLibrary.Instance.GetConfig(_lapRaceGameData.trackKey);
            _currentTrack = Object.Instantiate(trackConfig.trackPrefab);
        }

        #endregion
    }
}
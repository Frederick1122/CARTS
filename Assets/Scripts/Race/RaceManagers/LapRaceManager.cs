using Cars.Controllers;
using Installers;
using Managers;
using Managers.Libraries;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Race
{
    public class LapRaceManager : RaceManager
    {
        private const string PLAYER_PRESET_NAME = "PlayerPreset";

        [Inject] private TrackData _trackData;

        private Track _currentTrack;
        private List<CarController> _enemies = new();
        private List<int> _lapsStats = new();

        public override void Init()
        {
            InitTrack();
            base.Init();
            InitAi();
        }

        public override void StartRace()
        {
            foreach (var en in _enemies)
                en.StartCar();

            _player.StartCar();
        }

        [ContextMenu("Test Race")]
        public void TestRace()
        {
            Init();
            StartRace();
        }

        protected override void InitPlayer()
        {
            _lapsStats.Add(0);
            var playerConfig = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey);
            var playerPreset = PresetLibrary.Instance.GetConfig(PLAYER_PRESET_NAME);
            var playerPrefab = playerConfig.prefab;
            var spawnPlayerData = _currentTrack.SpawnPlayer(playerPrefab);
            _player = (CarController)spawnPlayerData.car.gameObject.AddComponent(playerPreset.CarController);

            var playerInputSystem = (IInputSystem)_player.gameObject.AddComponent(playerPreset.InputSystem);
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

            var spawnEnemyDatas = _currentTrack.SpawnAiTrucks(enemyConfigs);

            for (var i = 0; i < spawnEnemyDatas.Count; i++)
            {
                var enemyPreset = PresetLibrary.Instance.GetRandomConfig(PLAYER_PRESET_NAME);
                _enemies.Add((CarController)spawnEnemyDatas[i].car.gameObject.AddComponent(enemyPreset.CarController));

                var aiInputSystem = (IInputSystem)_enemies[i].gameObject.AddComponent(enemyPreset.InputSystem);
                aiInputSystem.Init(enemyPreset, spawnEnemyDatas[i].car);

                var waypointTracker = _enemies[i].gameObject.AddComponent<WaypointProgressTracker>();
                var lapStatindex = i + 1;
                waypointTracker.OnLapEndAction += () => UpdateLapStats(lapStatindex);
                waypointTracker.Circuit = spawnEnemyDatas[i].circuit;
                _lapsStats.Add(0);
                waypointTracker.Init(_enemies[i], aiInputSystem);

                _enemies[i].Init(aiInputSystem, enemyConfigs[i], PresetLibrary.Instance.GetRandomConfig(),
                    waypointTracker);
            }
        }

        private void InitTrack()
        {
            var trackConfig = TrackLibrary.Instance.GetConfig(_trackData.configKey);
            _currentTrack = Instantiate(trackConfig.trackPrefab);
        }

        private void UpdateLapStats(int lapStatsIndex)
        {
            _lapsStats[lapStatsIndex]++;
            if (lapStatsIndex == 0)
                Debug.Log("PLAYER ENDS LAP");
            else
                Debug.Log($"BOT {lapStatsIndex} ENDS LAP");
        }
    }
}
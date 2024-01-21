using System.Collections.Generic;
using Cars.Controllers;
using Installers;
using Managers;
using Managers.Libraries;
using UnityEngine;
using Zenject;

namespace Race
{
    public class LapRaceManager : RaceManager
    {
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
            var playerPrefab = playerConfig.prefab;
            var spawnPlayerData = _currentTrack.SpawnPlayer(playerPrefab);
            _player = spawnPlayerData.car.gameObject.AddComponent<PlayerCarController>();
            
            var waypointTracker =
                _player.gameObject.AddComponent<WaypointProgressTracker>();
            waypointTracker.Circuit = spawnPlayerData.circuit;
            waypointTracker.OnLapEndAction += () => UpdateLapStats(0);

            var playerInputSystem = _player.gameObject.AddComponent<KeyBoardInputSystem>();

            waypointTracker.Init(_player, playerInputSystem);
            _player.Init(playerInputSystem, playerConfig, PresetLibrary.Instance.GetRandomConfig(), waypointTracker);
        }
    
        private void InitAi()
        {
            var enemyConfigs = CarLibrary.Instance.GetRandomsConfigs(_currentTrack.GetCarPlacesCount() - 1);
 
            var spawnEnemyDatas = _currentTrack.SpawnAiTrucks(enemyConfigs);

            for (var i = 0; i < spawnEnemyDatas.Count; i++)
            {
                _enemies.Add(spawnEnemyDatas[i].car.gameObject.AddComponent<AITargetCarController>());
                var waypointTracker =
                    _enemies[i].gameObject.AddComponent<WaypointProgressTracker>();
                waypointTracker.OnLapEndAction += () => UpdateLapStats(i + 1);
                waypointTracker.Circuit = spawnEnemyDatas[i].circuit;
                _lapsStats.Add(0);

                var aiInputSystem = _enemies[i].gameObject.AddComponent<AITargetInputSystem>();
                waypointTracker.Init(_enemies[i], aiInputSystem);

                _enemies[i].Init(aiInputSystem, enemyConfigs[i], PresetLibrary.Instance.GetRandomConfig(), waypointTracker);
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
                Debug.Log("BOT ENDS LAP");
        }
    }
}


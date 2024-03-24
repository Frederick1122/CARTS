using Cars.Controllers;
using Cars.InputSystem;
using Cars.InputSystem.Player;
using Cars.Tools;
using Cysharp.Threading.Tasks;
using Installers;
using Managers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Cars;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Race.RaceManagers
{
    public class LapRaceState : RaceState, IWorldCollisionsSetUpper
    {
        private const string PLAYER_PRESET_NAME = "PlayerPreset";
        
        public event Action<int> OnPlayerChangePositionAction = delegate { };
        public event Action OnPlayerEndsLapAction = delegate { };

        [Inject] private GameDataInstaller.GameData _gameData;
        [Inject] private GameDataInstaller.LapRaceGameData _defaultLapRaceGameData;

        private GameDataInstaller.LapRaceGameData _lapRaceGameData;
        private LapRaceAIHelper _aiHelper;
        private CarController _player;

        private Track _currentTrack;
        private readonly List<CarController> _enemies = new();
        private readonly List<int> _lapsStats = new();

        private int _lastPlayerPosition;
        private readonly float _checkPositionDelay = 0.1f;
        private CancellationTokenSource _positionCts;
        private DateTime _startTime;

        public Dictionary<CarCollisionDetection, Collider> AllCollisions { get; private set; } = new();

        public override void Init()
        {
            Clear();

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
            InitAiHelper();

            InitCollisionsDetetections();

            var allCars = new List<CarController> {_player};
            allCars.AddRange(_enemies);
            _currentTrack.GetWaypointMainProgressTracker().Init(allCars);
            _currentTrack.GetWaypointMainProgressTracker().OnLapEndAction += UpdateLapStats;
        }

        private void Clear()
        {
            AllCollisions.Clear();
            _enemies.Clear();
            _lapsStats.Clear();
        }

        public override void Destroy()
        {
            _positionCts?.Cancel();
            Clear();

            Object.Destroy(_currentTrack != null ? _currentTrack.gameObject : null); 
            Object.Destroy(_player != null ? _player.gameObject : null);
            Object.Destroy(_aiHelper != null ? _aiHelper.gameObject : null);
        }

        public override int GetResult()
        {
            return GameResult.GetLapRaceResult(_lastPlayerPosition, (float)GetPassTime().TotalSeconds);
        }

        public override void StartRace()
        {
            foreach (var en in _enemies)
                en.TurnEngineOn();

            _player.TurnEngineOn();
            _currentTrack.StartRace();
            _aiHelper.StartRace();
            _positionCts = new CancellationTokenSource();
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
            var playerPassedDistance = GetPlayerPassedDistance();
            var playerPosition = GetMaxPositions();

            for (var i = 0; i < _enemies.Count; i++)
            {
                var enemyPassedDistance =  _currentTrack.GetWaypointMainProgressTracker().GetPassedDistance(i + 1);

                if (playerPassedDistance > enemyPassedDistance)
                    playerPosition--;
            }

            return playerPosition;
        }

        public float GetPlayerPassedDistance()
        {
            return _currentTrack.GetWaypointMainProgressTracker().GetPassedDistance(0);
        }

        public float GetLapDistance()
        {
            return _currentTrack.GetWaypointMainProgressTracker().GetLapDistance();
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

        private void InitPlayer()
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

            waypointTracker.Init(_player, playerInputSystem);

            var collisionDetection = _player.gameObject.AddComponent<CarCollisionDetection>();
            AddCollision(collisionDetection, _player.gameObject.GetComponent<BoxCollider>());

            _player.Init(playerInputSystem, playerConfig, playerPreset, collisionDetection, waypointTracker);
        }

        private void InitAi()
        {
            var carClass = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey).Rarity;

            var enemyConfigs = ((CarLibrary)CarLibrary.Instance).GetConfigsByRarity(carClass, _currentTrack.GetCarPlacesCount() - 1);

            var spawnEnemyDatas = _currentTrack.SpawnAiTrucks(enemyConfigs, _lapRaceGameData.botCount);

            for (var i = 0; i < spawnEnemyDatas.Count; i++)
            {
                var enemyPreset = PresetLibrary.Instance.GetRandomConfig(PLAYER_PRESET_NAME);
                _enemies.Add((CarController)spawnEnemyDatas[i].car.gameObject.AddComponent(enemyPreset.CarController));

                var enemy = _enemies[i];

                var aiInputSystem = (IInputSystem)enemy.gameObject.AddComponent(enemyPreset.InputSystem);
                aiInputSystem.Init(enemyPreset, spawnEnemyDatas[i].car);

                var waypointTracker = enemy.gameObject.AddComponent<WaypointProgressTracker>();
                waypointTracker.Circuit = spawnEnemyDatas[i].circuit;
                _lapsStats.Add(0);
                waypointTracker.Init(enemy, aiInputSystem);

                var collisionDetection = enemy.gameObject.AddComponent<CarCollisionDetection>();
                AddCollision(collisionDetection, enemy.gameObject.GetComponent<BoxCollider>());

                enemy.Init(aiInputSystem, enemyConfigs[i], PresetLibrary.Instance.GetRandomConfig(), 
                    collisionDetection, waypointTracker);
            }
        }
        
        private void InitAiHelper()
        {
            var aiHelperGameObject = new GameObject("AiHelper", new[] {typeof(LapRaceAIHelper)});
            aiHelperGameObject.transform.parent = _currentTrack.transform;
            _aiHelper = aiHelperGameObject.GetComponent<LapRaceAIHelper>();
            _aiHelper.Init(_currentTrack.GetWaypointMainProgressTracker(), _enemies);
        }
        
        private void InitTrack()
        {
            var trackConfig = TrackLibrary.Instance.GetConfig(_lapRaceGameData.trackKey);
            _currentTrack = Object.Instantiate(trackConfig.trackPrefab);
        }

        public void AddCollision(CarCollisionDetection collision, Collider collider) =>
            AllCollisions.Add(collision, collider);

        public void InitCollisionsDetetections()
        {
            foreach (var collision in AllCollisions.Keys)
                collision.SetUpAllWorldCollider(AllCollisions.Values.ToList());
        }

        #endregion
    }
}
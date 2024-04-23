using System;
using System.Collections.Generic;
using System.Linq;
using Cars.Controllers;
using Cars.InputSystem;
using Cars.InputSystem.Player;
using Cars.Tools;
using ConfigScripts;
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
    public class FreeRideState : RaceState, IWorldCollisionsSetUpper
    {
        private const string PLAYER_PRESET_NAME = "PlayerPreset";

        public event Action<int> OnResultUpdateAction = delegate { };

        [Inject] private GameDataInstaller.GameData _gameData;
        [Inject] private GameDataInstaller.FreeRideGameData _defaultFreeRideGameData;

        private GameDataInstaller.FreeRideGameData _freeRideGameData;

        private CarController _player;
        private FreeRideCarHelper _carHelper;

        private FreeRideTrackConfig _config;
        private Transform _startPosition;
        private MapFabric _mapFabric;
        private DifficultyModifier _difficultyModifier;
        private FreeRidePrefabData _currentTrack;
        private int _score;
        private DateTime _startTime;

        public Dictionary<CarCollisionDetection, Collider> AllCollisions { get; private set; } = new();

        public override void Init()
        {
            _score = 0;

            AllCollisions.Clear();

            if (_gameData.gameModeData is GameDataInstaller.FreeRideGameData freeRideGameData)
            {
                _freeRideGameData = freeRideGameData;
            }
            else
            {
                Debug.LogWarning("ALERT! TEST MODE! Game initialize in base config");
                _freeRideGameData = _defaultFreeRideGameData;
            }

            _config = FreeRideTrackLibrary.Instance.GetConfig(_freeRideGameData.trackKey);

            InitTrack();
            InitPlayer();
            InitCollisionsDetetections();

            _mapFabric.Init(_config.mapFabricData, this);

            _difficultyModifier.Init(_player, this, _config.freeRideDifficultyModifierData);
            _startTime = DateTime.Now;
        }

        public override void Destroy()
        {
            if (_mapFabric == null)
                return;

            _carHelper.OnFall -= PlayerFall;
            _carHelper.OnReachPiece -= UpdateScore;

            Object.Destroy(_currentTrack?.gameObject);
            Object.Destroy(_player?.gameObject);
        }

        public override void StartRace()
        {
            _player.TurnEngineOn();
            _score = 0;

            base.StartRace();
        }

        public int GetScore() { return _score; }

        public override int GetResult() { return GameResult.GetFreeRideResult(_score); }

        public TimeSpan GetPassTime()
        {
            var dateTime = DateTime.Now - _startTime;
            return new TimeSpan(dateTime.Days, dateTime.Hours, dateTime.Minutes, dateTime.Seconds,
                dateTime.Milliseconds);
        }

        private void InitPlayer()
        {
            var playerConfig = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey);
            var playerPreset = PresetLibrary.Instance.GetConfig(PLAYER_PRESET_NAME);
            var playerPrefab = playerConfig.prefab;
            _startPosition.transform.position -= playerPrefab.GetLowestPoint();
            var player = Object.Instantiate(playerPrefab, _startPosition);
            _player = (CarController)player.gameObject.AddComponent(playerPreset.CarController);

            _carHelper = player.gameObject.AddComponent<FreeRideCarHelper>();
            _carHelper.OnReachPiece += ReachPiece;
            _carHelper.OnFall += PlayerFall;

            var playerInputSystem = (IInputSystem)_player.gameObject.AddComponent(typeof(FreeRideInputSystem));
            playerInputSystem.Init(playerPreset, playerPrefab);

            var collisionDetection = _player.gameObject.AddComponent<CarCollisionDetection>();
            AddCollision(collisionDetection, _player.gameObject.GetComponent<BoxCollider>());

            _player.Init(playerInputSystem, playerConfig, playerPreset, collisionDetection);
        }

        private void InitTrack()
        {
            _currentTrack = Object.Instantiate(_config.freeRidePrefab);

            _startPosition = _currentTrack.mapFabric.GetPlayerSpawnPoint();
            _mapFabric = _currentTrack.mapFabric;
            _difficultyModifier = _currentTrack.difficultyModifier;
        }

        private void PlayerFall()
        {
            _player.TurnEngineOff();
            _mapFabric.StopFabric();
            FinishRace();
        }

        private void UpdateScore()
        {
            _score++;
            Debug.Log($"Result {_score}");
            OnResultUpdateAction?.Invoke(_score);
        }

        private void ReachPiece()
        {
            UpdateScore();
            _mapFabric.SpawnPiece();
        }

        public void AddCollision(CarCollisionDetection collision, Collider collider) =>
            AllCollisions.Add(collision, collider);

        public void InitCollisionsDetetections()
        {
            foreach (var collision in AllCollisions.Keys)
                collision.SetUpAllWorldCollider(AllCollisions.Values.ToList());
        }
    }
}

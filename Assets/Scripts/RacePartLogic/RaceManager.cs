using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldCode
{
    public class RaceManager : MonoBehaviour
    {
        [Header("Players")] [SerializeField] private List<CarController> _players;
        [SerializeField] private RaceCamera _playerCameraPrefab;

        [Header("Enemies")] [SerializeField] private List<CarController> _enemies;

        [Header("Race points")] [SerializeField]
        private List<Transform> _startPositions = new();

        [SerializeField] private CheckPointsHolder _checkPointsHolder;

        private readonly List<RaceCamera> _playerCameras = new();

        private void Start()
        {
            SetUpRace();
        }

        public void SetUpRace()
        {
            PlaceCars();
            InitCars();
            SetUpCameras();
        }

        public void AddPlayers(List<CarController> pl)
        {
            foreach (CarController player in _players)
                _players.Add(player);
        }

        private void PlaceCars()
        {
            int curPos = 0;

            // Add ai's car
            foreach (var enemy in _enemies)
            {
                enemy.gameObject.SetActive(false);
                enemy.transform.position = _startPositions[curPos].position + new Vector3(0, 1, 0);
                curPos++;
            }

            // Add player's car
            foreach (var player in _players)
            {
                player.gameObject.SetActive(false);
                player.transform.position = _startPositions[curPos].position + new Vector3(0, 1, 0);
                curPos++;
            }
        }

        private void InitCars()
        {
            // Initialize players
            foreach (var player in _players)
            {
                player.Init(_checkPointsHolder.Points);
                player.gameObject.SetActive(true);
            }

            // Initialize ai
            foreach (var enemy in _enemies)
            {
                enemy.Init(_checkPointsHolder.Points);
                enemy.gameObject.SetActive(true);
            }
        }

        // SetUp camera for every player
        private void SetUpCameras()
        {
            foreach (var player in _players)
            {
                var cam = Instantiate(_playerCameraPrefab);
                cam.Init(player.gameObject);
                _playerCameras.Add(cam);
            }
        }
    }
}
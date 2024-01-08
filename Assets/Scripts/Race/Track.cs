using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private int _playerPlace = -1;

        [SerializeField] private List<StartRacePlace> _carPlaces = new(4);
        [SerializeField] private List<CarController> _aiPrefabs = new();

        public CarController SpawnPlayer(CarController playerPrefab)
        {
            var playerTransform = _playerPlace == -1 ? _carPlaces[^1].transform : _carPlaces[_playerPlace].transform;
            var player = Instantiate(playerPrefab, playerTransform);
            player.transform.rotation = Quaternion.identity;
            return player;
        }

        public List<CarController> SpawnAiTrucks()
        {
            var skipPlace = _playerPlace == -1 ? _carPlaces.Count - 1 : _playerPlace;
            var enemies = new List<CarController>();
            for (int i = 0; i < _carPlaces.Count - 1; i++)
            {
                if(skipPlace == i)
                    continue;
    
                var enemy = Instantiate(_aiPrefabs[Random.Range(0, _aiPrefabs.Count)], _carPlaces[i].transform);
                enemy.transform.rotation = Quaternion.identity;
    
                enemies.Add(enemy);
                _carPlaces[i].Init(enemy);
            }

            return enemies;
        }
    }
}
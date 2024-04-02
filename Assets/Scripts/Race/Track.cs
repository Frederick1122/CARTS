using Cars;
using Cars.Controllers;
using ConfigScripts;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

namespace Race
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private int _playerPlace = -1;
        [SerializeField] private WaypointMainProgressTracker _waypointMainProgressTracker;
        [SerializeField] private List<StartRacePlace> _carPlaces = new(4);

        public void StartRace()
        {
            _waypointMainProgressTracker.StartRace();
        }
        
        public SpawnData SpawnPlayer(CarPrefabData playerPrefab)
        {
            var playerPlace = _playerPlace == -1 ? _carPlaces[^1] : _carPlaces[_playerPlace];
            playerPlace.transform.position -= playerPrefab.GetLowestPoint();
            var player = Instantiate(playerPrefab, playerPlace.transform);
            return new SpawnData(player, playerPlace.GetWaypointCircuit());
        }

        public List<SpawnData> SpawnAiTrucks(IReadOnlyList<CarConfig> enemyConfigs, int count)
        {
            var skipPlace = _playerPlace == -1 ? _carPlaces.Count - 1 : _playerPlace;
            var enemiesSpawnDatas = new List<SpawnData>();
            for (int i = 0; i < _carPlaces.Count - 1; i++)
            {
                if (enemiesSpawnDatas.Count == count)
                    break;

                if (skipPlace == i)
                    continue;

                _carPlaces[i].transform.position -= enemyConfigs[i].prefab.GetLowestPoint();
                var enemy = Instantiate(enemyConfigs[i].prefab, _carPlaces[i].transform);
                //enemy.transform.rotation = Quaternion.identity;

                enemiesSpawnDatas.Add(new SpawnData(enemy, _carPlaces[i].GetWaypointCircuit()));
            }

            return enemiesSpawnDatas;
        }

        public WaypointMainProgressTracker GetWaypointMainProgressTracker()
        {
            return _waypointMainProgressTracker;
        }
        
        public int GetCarPlacesCount()
        {
            return _carPlaces.Count;
        }
    }

    public class SpawnData
    {
        public CarPrefabData car;
        public WaypointCircuit circuit;

        public SpawnData() { }

        public SpawnData(CarPrefabData car, WaypointCircuit circuit)
        {
            this.car = car;
            this.circuit = circuit;
        }
    }
}
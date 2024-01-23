﻿using Cars;
using ConfigScripts;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private int _playerPlace = -1;
        [SerializeField] private List<StartRacePlace> _carPlaces = new(4);

        public SpawnData SpawnPlayer(CarPrefabData playerPrefab)
        {
            var playerPlace = _playerPlace == -1 ? _carPlaces[^1] : _carPlaces[_playerPlace];
            var player = Instantiate(playerPrefab, playerPlace.transform);
            player.transform.rotation = Quaternion.identity;
            return new SpawnData(player, playerPlace.GetWaypointCircuit());
        }

        public List<SpawnData> SpawnAiTrucks(List<CarConfig> enemyConfigs)
        {
            var skipPlace = _playerPlace == -1 ? _carPlaces.Count - 1 : _playerPlace;
            var enemiesSpawnDatas = new List<SpawnData>();
            for (int i = 0; i < _carPlaces.Count - 1; i++)
            {
                if (skipPlace == i)
                    continue;

                var enemy = Instantiate(enemyConfigs[i].prefab, _carPlaces[i].transform);
                enemy.transform.rotation = Quaternion.identity;

                enemiesSpawnDatas.Add(new SpawnData(enemy, _carPlaces[i].GetWaypointCircuit()));
            }

            return enemiesSpawnDatas;
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
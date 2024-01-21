using System;
using System.Collections.Generic;
using Base;
using Managers.Libraries;
using Newtonsoft.Json;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : SaveLoadManager<PlayerData, PlayerManager>
    {
        private const string PLAYER_JSON_PATH = "Player.json";
        
        [SerializeField] private CarData _defaultCar;

        public void AddPurchasedCar(string carConfigKey)
        {
            if(_saveData.purchasedCars.ContainsKey(carConfigKey))
                return;

            _saveData.purchasedCars.Add(carConfigKey, new CarData(carConfigKey));
            Save();
        }

        public void UpdateModificationLevel(string carConfigKey, int newLevel, ModificationType modificationType)
        {
            if (!_saveData.purchasedCars.ContainsKey(carConfigKey))
            {
                Debug.LogAssertion($"PlayerManager not founded {carConfigKey} in purchased cars. UpdateModificationLevel is impossible");
                return;
            }

            var carConfig = CarLibrary.Instance.GetConfig(carConfigKey);
            switch (modificationType)
            {
                case ModificationType.MaxSpeed:
                    _saveData.purchasedCars[carConfigKey].maxSpeedLevel =
                        Mathf.Clamp(newLevel, 0, carConfig.maxSpeedLevels.Count - 1);
                    break;
                case ModificationType.Acceleration:
                    _saveData.purchasedCars[carConfigKey].accelerationLevel =
                        Mathf.Clamp(newLevel, 0, carConfig.accelerationLevels.Count - 1);
                    break;
                case ModificationType.Turn:
                    _saveData.purchasedCars[carConfigKey].turnLevel =
                        Mathf.Clamp(newLevel, 0, carConfig.turnLevels.Count - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modificationType), modificationType, null);
            }

            if (_saveData.currentCar.configKey == carConfigKey) 
                _saveData.currentCar = _saveData.purchasedCars[carConfigKey];
            
            Save();
        }

        public void SetCurrentCar(string carConfigKey)
        {
            if (!_saveData.purchasedCars.ContainsKey(carConfigKey))
            {
                Debug.LogAssertion($"PlayerManager not founded {carConfigKey} in purchased cars. SetCurrentCar is impossible");
                return;
            }

            _saveData.currentCar = _saveData.purchasedCars[carConfigKey];
            Save();
        }

        public CarData GetCurrentCar()
        {
            if (_saveData == null)
                Load();
            
            return _saveData.currentCar;
        }
        
        protected override void Load()
        {
            base.Load();
            if (_saveData == null)
            {
                _saveData = new PlayerData(_defaultCar);
                Save();
            }
        }
        
        protected override void UpdatePath()
        {
            _secondPath = PLAYER_JSON_PATH;
            base.UpdatePath();
        }
    }
    
    public enum ModificationType
    {
        MaxSpeed,
        Acceleration,
        Turn
    }
}

[Serializable]
public class PlayerData
{
    [JsonProperty("CurrentCar")]
    public CarData currentCar;
    [JsonProperty("PurchasedCars")]
    public Dictionary<string, CarData> purchasedCars = new();
    
    [JsonConstructor]
    private PlayerData()
    {
    }
    
    public PlayerData(CarData baseCar)
    {
        currentCar = baseCar;
        purchasedCars.Add(baseCar.configKey, baseCar);
    }
}

[Serializable]
public class CarData
{
    [JsonProperty("ConfigKey")]
    public string configKey;
    [JsonProperty("MaxSpeedLevel")]
    public int maxSpeedLevel = 0;
    [JsonProperty("AccelerationLevel")]
    public int accelerationLevel = 0;
    [JsonProperty("TurnLevel")]
    public int turnLevel = 0;
    
    [JsonConstructor]
    private CarData()
    {
    }
    
    public CarData(string configKey)
    {
        this.configKey = configKey;
    }
}

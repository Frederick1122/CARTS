using System;
using System.Collections.Generic;
using Base;
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
    public CarData currentCar;
    public Dictionary<string, CarData> purchasedCars = new();

    public PlayerData(CarData baseCar)
    {
        currentCar = baseCar;
        purchasedCars.Add(baseCar.configKey, baseCar);
    }
}

[Serializable]
public class CarData
{
    public string configKey;
    public int maxSpeedLevel = 0;
    public int accelerationLevel = 0;
    public int turnLevel = 0;
    
    public CarData() {}

    public CarData(string configKey)
    {
        this.configKey = configKey;
    }
}

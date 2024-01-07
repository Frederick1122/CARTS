using System;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Managers
{
 
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private CarData _defaultCar;
    
        private CarData _currentCar;
        private Dictionary<string, CarData> _purchasedCars = new();

        public void AddPurchasedCar(CarData newCar)
        {
            if(_purchasedCars.ContainsKey(newCar.configKey))
                return;
        
            _purchasedCars.Add(newCar.configKey, newCar);
        }

        public void UpdateModificationLevel(string carConfigKey, int newLevel, ModificationType modificationType)
        {
            if (!_purchasedCars.ContainsKey(carConfigKey))
            {
                Debug.LogAssertion($"PlayerManager not founded {carConfigKey} in purchased cars. UpdateModificationLevel is impossible");
                return;
            }

            var carConfig = CarLibrary.Instance.GetCar(carConfigKey);
            switch (modificationType)
            {
                case ModificationType.MaxSpeed:
                    _purchasedCars[carConfigKey].maxSpeedLevel =
                        Mathf.Clamp(newLevel, 0, carConfig.maxSpeedLevels.Count - 1);
                    break;
                case ModificationType.Acceleration:
                    _purchasedCars[carConfigKey].accelerationLevel =
                        Mathf.Clamp(newLevel, 0, carConfig.accelerationLevels.Count - 1);
                    break;
                case ModificationType.Turn:
                    _purchasedCars[carConfigKey].turnLevel =
                        Mathf.Clamp(newLevel, 0, carConfig.turnLevels.Count - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modificationType), modificationType, null);
            }

            if (_currentCar.configKey == carConfigKey) 
                _currentCar = _purchasedCars[carConfigKey];
        }

        public void SetCurrentCar(string carConfigKey)
        {
            if (!_purchasedCars.ContainsKey(carConfigKey))
            {
                Debug.LogAssertion($"PlayerManager not founded {carConfigKey} in purchased cars. SetCurrentCar is impossible");
                return;
            }

            _currentCar = _purchasedCars[carConfigKey];
        }
        
        public enum ModificationType
        {
            MaxSpeed,
            Acceleration,
            Turn
        }
    }
}

[Serializable]
public class CarData
{
    public string configKey;
    public int maxSpeedLevel = 0;
    public int accelerationLevel = 0;
    public int turnLevel = 0;
}

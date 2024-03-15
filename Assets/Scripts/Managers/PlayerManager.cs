using Base;
using Managers.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ConfigScripts;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : SaveLoadManager<PlayerData, PlayerManager>
    {
        private const string PLAYER_JSON_PATH = "Player.json";

        public event Action<CarData> OnPlayerCarChange = delegate(CarData data) {  };
        public event Action<CurrencyType, int> OnCurrencyChange = delegate(CurrencyType type, int i) {  };

        [SerializeField] private CarData _defaultCar;

        public void AddPurchasedCar(string carConfigKey)
        {
            if (_saveData.purchasedCars.ContainsKey(carConfigKey))
                return;

            var carConfig = CarLibrary.Instance.GetConfig(carConfigKey);
            if (!IsThereEnoughMoney(carConfig.price))
                return;    
            
            DecreaseCurrency(carConfig.price.CurrencyType, carConfig.price.Value);
            
            _saveData.purchasedCars.Add(carConfigKey, new CarData(carConfigKey));
            Save();
        }

        public bool UpdateModificationLevel(string carConfigKey, ModificationType modificationType)
        {
            if (!_saveData.purchasedCars.ContainsKey(carConfigKey))
            {
                Debug.LogAssertion($"PlayerManager not founded {carConfigKey} in purchased cars. UpdateModificationLevel is impossible");
                return false;
            }

            var carConfig = CarLibrary.Instance.GetConfig(carConfigKey);
            int level;
            switch (modificationType)
            {
                case ModificationType.MaxSpeed:
                    level = Mathf.Clamp(_saveData.purchasedCars[carConfigKey].maxSpeedLevel + 1, 0,
                        carConfig.maxSpeedLevels.Count - 1);

                    var priceS = new Price(carConfig.maxSpeedLevels[level].Price, CurrencyType.Soft);
                    if (!IsThereEnoughMoney(priceS))
                        return false;
                    DecreaseCurrency(priceS.CurrencyType, priceS.Value);

                    _saveData.purchasedCars[carConfigKey].maxSpeedLevel = level;
                    break;

                case ModificationType.Acceleration:
                    level = Mathf.Clamp(_saveData.purchasedCars[carConfigKey].accelerationLevel + 1, 0,
                         carConfig.accelerationLevels.Count - 1);

                    var priceA = new Price(carConfig.accelerationLevels[level].Price, CurrencyType.Soft);
                    if (!IsThereEnoughMoney(priceA))
                        return false;
                    DecreaseCurrency(priceA.CurrencyType, priceA.Value);

                    _saveData.purchasedCars[carConfigKey].accelerationLevel = level;
                    break;

                case ModificationType.Turn:
                    level = Mathf.Clamp(_saveData.purchasedCars[carConfigKey].turnLevel + 1, 0,
                        carConfig.turnLevels.Count - 1);

                    var priceT = new Price(carConfig.turnLevels[level].Price, CurrencyType.Soft);
                    if (!IsThereEnoughMoney(priceT))
                        return false;
                    DecreaseCurrency(priceT.CurrencyType, priceT.Value);

                    _saveData.purchasedCars[carConfigKey].turnLevel = level;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(modificationType), modificationType, null);
            }

            Debug.Log($"{carConfigKey} upgrade {modificationType}, lvl: {level}");

            if (_saveData.currentCar.configKey == carConfigKey)
                _saveData.currentCar = _saveData.purchasedCars[carConfigKey];

            Save();

            return true;
        }

        public int GetEquippedCarCharacteristicLevel(ModificationType type)
        {
            switch (type)
            {
                case ModificationType.MaxSpeed:
                    return _saveData.purchasedCars[_saveData.currentCar.configKey].maxSpeedLevel;

                case ModificationType.Turn:
                    return _saveData.purchasedCars[_saveData.currentCar.configKey].turnLevel;

                case ModificationType.Acceleration:
                    return _saveData.purchasedCars[_saveData.currentCar.configKey].accelerationLevel;

                default:
                    throw new Exception("No modification type");
            }
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
            OnPlayerCarChange?.Invoke(_saveData.currentCar);
        }

        public IReadOnlyList<CarData> GetPurchasedCars()
        {
            var cars = _saveData.purchasedCars.Values.ToList();
            return cars;
        }

        public bool IsThereEnoughMoney(Price price)
        {
            return price.CurrencyType == CurrencyType.Soft
                ? _saveData.regularCurrency >= price.Value
                : _saveData.premiumCurrency >= price.Value;
        }

        public bool TryGetPurchasedCar(string key, out CarData data)
        {
            if(_saveData.purchasedCars.TryGetValue(key, out data))
                return true;

            data = null;
            return false;
        }

        public CarData GetCurrentCar()
        {
            if (_saveData == null)
                Load();

            return _saveData.currentCar;
        }

        public int GetCurrency(CurrencyType currencyType)
        {
            return currencyType == CurrencyType.Soft ? _saveData.regularCurrency : _saveData.premiumCurrency;
        }

        public void DecreaseCurrency(CurrencyType currencyType, int value)
        {
            var currentValue = GetCurrency(currencyType); 
            SetCurrency(currencyType,  Mathf.Clamp(currentValue - value, 0, currentValue));   
        }
        
        public void IncreaseCurrency(CurrencyType currencyType, int value)
        {
            var currentValue = GetCurrency(currencyType); 
            SetCurrency(currencyType, currentValue + value);   
        }
        
        private void SetCurrency(CurrencyType currencyType, int newValue)
        {
            switch (currencyType)
            {
                case CurrencyType.Soft:
                    _saveData.regularCurrency = newValue;
                    break;
                case CurrencyType.Hard:
                    _saveData.premiumCurrency = newValue;
                    break;
            }
            
            OnCurrencyChange?.Invoke(currencyType, newValue);
            Save();
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

    public enum CurrencyType
    {
        Soft,
        Hard
    }
}

[Serializable]
public class PlayerData
{
    [JsonProperty("RegularCurrency")]
    public int regularCurrency;
    
    [JsonProperty("premiumCurrency")]
    public int premiumCurrency;

    [JsonProperty("CurrentCar")]
    public CarData currentCar;
    [JsonProperty("PurchasedCars")]
    public Dictionary<string, CarData> purchasedCars = new();

    [JsonConstructor]
    private PlayerData() { }

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
    private CarData() { }

    public CarData(string configKey) =>
        this.configKey = configKey;
}

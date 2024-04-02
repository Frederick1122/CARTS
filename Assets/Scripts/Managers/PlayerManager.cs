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

        #region CarWork
        public void PurchaseDefaultCar() => TryToPurchaseCar(_defaultCar.configKey);

        public IReadOnlyList<CarData> GetPurchasedCars()
        {
            var cars = _saveData.purchasedCars.Values.ToList();
            return cars;
        }

        public bool TryGetPurchasedCarData(string key, out CarData data)
        {
            if (_saveData.purchasedCars.TryGetValue(key, out data))
                return true;

            data = null;
            return false;
        }

        public bool TryToPurchaseCar(string carConfigKey)
        {
            if (_saveData.purchasedCars.ContainsKey(carConfigKey))
                return false;

            var carConfig = CarLibrary.Instance.GetConfig(carConfigKey);

            if (!IsEnoughMoney(carConfig.price))
                return false;    
            DecreaseCurrency(carConfig.price.CurrencyType, carConfig.price.Value);
            
            _saveData.purchasedCars.Add(carConfigKey, new CarData(carConfigKey));
            Save();
            return true;
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

        public CarData GetCurrentCar()
        {
            if (_saveData == null)
                Load();

            return _saveData.currentCar;
        }
        #endregion

        #region ModoficationWork
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
                    if (!IsEnoughMoney(priceS))
                        return false;
                    DecreaseCurrency(priceS.CurrencyType, priceS.Value);

                    _saveData.purchasedCars[carConfigKey].maxSpeedLevel = level;
                    break;

                case ModificationType.Acceleration:
                    level = Mathf.Clamp(_saveData.purchasedCars[carConfigKey].accelerationLevel + 1, 0,
                         carConfig.accelerationLevels.Count - 1);

                    var priceA = new Price(carConfig.accelerationLevels[level].Price, CurrencyType.Soft);
                    if (!IsEnoughMoney(priceA))
                        return false;
                    DecreaseCurrency(priceA.CurrencyType, priceA.Value);

                    _saveData.purchasedCars[carConfigKey].accelerationLevel = level;
                    break;

                case ModificationType.Turn:
                    level = Mathf.Clamp(_saveData.purchasedCars[carConfigKey].turnLevel + 1, 0,
                        carConfig.turnLevels.Count - 1);

                    var priceT = new Price(carConfig.turnLevels[level].Price, CurrencyType.Soft);
                    if (!IsEnoughMoney(priceT))
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
        #endregion

        #region CurrencyWork
        public int GetCurrency(CurrencyType currencyType)
        {
            return currencyType == CurrencyType.Soft ? _saveData.regularCurrency : _saveData.premiumCurrency;
        }

        public bool IsEnoughMoney(Price price) => 
            IsEnoughMoney(price.CurrencyType, price.Value);

        public bool IsEnoughMoney(CurrencyType currencyType, int value)
        {
            return currencyType == CurrencyType.Soft
                ? _saveData.regularCurrency >= value
                : _saveData.premiumCurrency >= value;
        }

        public void DecreaseCurrency(Price price) => 
            DecreaseCurrency(price.CurrencyType, price.Value);

        public void DecreaseCurrency(CurrencyType currencyType, int value)
        {
            var currentValue = GetCurrency(currencyType); 
            SetCurrency(currencyType,  Mathf.Clamp(currentValue - value, 0, currentValue));   
        }

        public void IncreaseCurrency(Price price) => 
            IncreaseCurrency(price.CurrencyType, price.Value);

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
        #endregion

        protected override void Load()
        {
            base.Load();
            _saveData ??= new PlayerData(_defaultCar);

            var carConfigKeys = CarLibrary.Instance.GetAllConfigs().Select(car => car.configKey);
            var validConfigs = new Dictionary<string, CarData>();
            foreach (var purchasedCar in _saveData.purchasedCars)
                if (carConfigKeys.Contains(purchasedCar.Key))
                    validConfigs.Add(purchasedCar.Key, purchasedCar.Value);
            
            if (validConfigs.Count == 0)
                validConfigs.Add(_defaultCar.configKey, _defaultCar);

            if (!validConfigs.ContainsKey(_saveData.currentCar.configKey))
                _saveData.currentCar = _defaultCar;
            
            _saveData.purchasedCars = validConfigs;

            Save();
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

using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Managers
{
    public class CarLibrary : Singleton<CarLibrary>
    {
        private const string CAR_CONFIG_PATH = "Configs";
        private Dictionary<string, CarConfig> _allCars = new();

        protected override void Awake()
        {
            base.Awake();

            var allCars = Resources.LoadAll(CAR_CONFIG_PATH, typeof(CarConfig));

            foreach (var car in allCars)
            {
                var castCar = (CarConfig) car;
                if (_allCars.ContainsKey(castCar.configKey))
                {
                    Debug.LogAssertion($"{castCar.configKey} is duplicated. Check {castCar.name} and {_allCars[castCar.configKey].name}");
                    return;
                }    
                
                _allCars.Add(castCar.configKey, castCar);
            }
        }

        public CarConfig GetCar(string configKey)
        {
            var car = _allCars[configKey];

            if (car == null) 
                Debug.LogAssertion($"CarLibrary not founded config with this key: {configKey}");

            return car;
        }
    }
}
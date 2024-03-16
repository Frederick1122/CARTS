using ConfigScripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers.Libraries
{
    public class CarLibrary : BaseLibrary<CarConfig>
    {
        private const string CAR_CONFIG_PATH = "Configs/Cars";

        protected override void Awake()
        {
            _path = CAR_CONFIG_PATH;
            base.Awake();
        }

        public IReadOnlyList<CarConfig> GetConfigsCertainClass(Rarity carClass)
        {
            var classConfigs = new List<CarConfig>();
            if (carClass != Rarity.Default)
                classConfigs = GetAllConfigs().Where(car => car.CarClass == carClass).ToList();
            else
                classConfigs = GetAllConfigs().ToList();
            return classConfigs;
        }

        public IReadOnlyList<CarConfig> GetRandomConfigsCertainClass(Rarity carClass, int count)
        {
            var classConfigs =  new List<CarConfig>();
            if (carClass != Rarity.Default)
                classConfigs = GetAllConfigs().Where(car => car.CarClass == carClass).ToList();
            else
                classConfigs = GetAllConfigs().ToList();

            var configs = new List<CarConfig>();
            for (int i = 0; i < count; i++)
            {
                configs.Add(classConfigs[Random.Range(0, classConfigs.Count)]);
            }

            return configs;
        }

        public IReadOnlyList<CarConfig> GetConfigsWithoutAI()
        {
            var allConfigs = GetAllConfigs();
            var freeList = new List<CarConfig>();
            foreach (var config in allConfigs)
            {
                if (config.isOnlyForAi)
                    continue;

                freeList.Add(config);
            }

            return freeList;
        }
    }
}
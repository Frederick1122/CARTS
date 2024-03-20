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

        public CarConfig GetRandomConfigByRarity(Rarity rarity)
        {
            var configs = GetConfigsByRarity(rarity);
            var rndNum = Random.Range(0, configs.Count);

            return configs[rndNum];
        }

        public IReadOnlyList<CarConfig> GetConfigsByRarity(Rarity rarity)
        {
            var classConfigs = new List<CarConfig>();
            if (rarity != Rarity.Default)
                classConfigs = GetAllConfigs().Where(car => car.Rarity == rarity).ToList();
            else
                classConfigs = GetAllConfigs().ToList();
            return classConfigs;
        }

        public IReadOnlyList<CarConfig> GetConfigsByRarity(Rarity rarity, int count)
        {
            var rarityConfigs = GetConfigsByRarity(rarity);

            var configs = new List<CarConfig>();
            for (int i = 0; i < count; i++)
                configs.Add(rarityConfigs[Random.Range(0, rarityConfigs.Count)]);

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
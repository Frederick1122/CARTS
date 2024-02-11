using ConfigScripts;
using System.Collections.Generic;

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
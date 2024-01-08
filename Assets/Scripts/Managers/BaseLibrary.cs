using System.Collections.Generic;
using Base;
using ConfigScripts;
using UnityEngine;

namespace Managers
{
    public class BaseLibrary<T> : Singleton<BaseLibrary<T>> where T : BaseConfig
    {
        protected string _path = "";
        private Dictionary<string, T> _allConfigs = new();
        
        protected override void Awake()
        {
            base.Awake();

            var allConfigs = Resources.LoadAll(_path, typeof(T));

            foreach (var config in allConfigs)
            {
                var castConfig = (T) config;
                if (_allConfigs.ContainsKey(castConfig.configKey))
                {
                    Debug.LogAssertion($"{castConfig.configKey} is duplicated. Check {castConfig.name} and {_allConfigs[castConfig.configKey].name}");
                    return;
                }    
                
                _allConfigs.Add(castConfig.configKey, castConfig);
            }
        }

        public T GetConfig(string configKey)
        {
            var config = _allConfigs[configKey];

            if (config == null) 
                Debug.LogAssertion($"CarLibrary not founded config with this key: {configKey}");

            return config;
        }
    }
}
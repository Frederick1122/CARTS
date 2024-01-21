﻿using System.Collections.Generic;
using System.Linq;
using Base;
using ConfigScripts;
using UnityEngine;

namespace Managers.Libraries
{
    public class BaseLibrary<T> : Singleton<BaseLibrary<T>> where T : BaseConfig
    {
        protected string _path = "";
        protected Dictionary<string, T> _allConfigs = new();
        
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

        public T GetRandomConfig()
        {
            return _allConfigs.ElementAt(Random.Range(0, _allConfigs.Count)).Value;
        }

        public List<T> GetRandomsConfigs(int count, bool withoutDuplicate = false)
        {
            var randomConfigs = new List<T>();
            
            if (withoutDuplicate)
            {
                if (_allConfigs.Count < count)
                {
                    Debug.LogError($"{this} can't execute GetRandomConfig. all configs - {_allConfigs.Count}, need configs - {count}. Add new configs or change parameter 'without duplicates'");
                }   
                else if (_allConfigs.Count == count)
                {
                    randomConfigs.AddRange(_allConfigs.Values);
                }
                else
                {
                    var allConfigCopy = _allConfigs;
                    while (randomConfigs.Count < count)
                    {
                        var randomConfig = allConfigCopy.ElementAt(Random.Range(0, allConfigCopy.Count));
                        randomConfigs.Add(randomConfig.Value);
                        allConfigCopy.Remove(randomConfig.Key);
                    }
                } 
            }
            else
                while (randomConfigs.Count < count)
                    randomConfigs.Add(GetRandomConfig());

            return randomConfigs;
        }

        public Dictionary<string, T> GetAllConfigs()
        {
            return _allConfigs;
        }
    }
}
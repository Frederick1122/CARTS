using System;
using Cars;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "Configs/Car Config")]
    public class CarConfig : BaseConfig
    {
        public Sprite CarIcon;
        public CarPrefabData prefab;
        public bool isOnlyForAi;
        public Price price = new();

        [field: SerializeField] public CarClass CarClass { get; private set; } = CarClass.Default;
        
        [Header("Characteristic")] 
        public List<float> maxSpeedLevels = new() { 100 };
        public List<float> accelerationLevels = new() { 10 };
        public List<float> turnLevels = new() { 3 };
        public float gravity = 7f;
        public float downforce = 5f;
        public bool airControl = false;

        [Header("Physics")] public AnimationCurve frictionCurve;
        public AnimationCurve turnCurve;
        public PhysicMaterial frictionMaterial;

        [Header("Visual")]
        [Range(0, 10)] public float bodyTilt;
    }

    [Serializable]
    public class Price
    {
        public int value = 0;
        public CurrencyType currencyType = CurrencyType.Regular;
    }

    public enum CarClass
    {
        Default = 0,
        Common = 1, 
        Rare = 2, 
        Epic = 3
    }
}
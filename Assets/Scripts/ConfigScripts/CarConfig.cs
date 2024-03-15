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
        public Price price = new(0, CurrencyType.Soft);

        [field: SerializeField] public CarClass CarClass { get; private set; } = CarClass.Default;

        [Header("Characteristic")]
        public List<CharacteristicLevel> maxSpeedLevels = new() { new CharacteristicLevel(100, 5) };
        public List<CharacteristicLevel> accelerationLevels = new() { new CharacteristicLevel(10, 5) };
        public List<CharacteristicLevel> turnLevels = new() { new CharacteristicLevel(3, 5) };
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
        [field:SerializeField] public int Value { get; private set; } = 0;
        [field: SerializeField] public CurrencyType CurrencyType { get; private set; } = CurrencyType.Soft;

        public Price(int value, CurrencyType currencyType)
        {
            Value = value;
            CurrencyType = currencyType;
        }
    }

    [Serializable] 
    public class CharacteristicLevel
    {
        [field:SerializeField] public float Value { get; private set; } = 0;
        [field: SerializeField] public int Price { get; private set; } = -1;

        public CharacteristicLevel(float value, int price)
        {
            Value = value;
            Price = price;
        }   
    }

    public enum CarClass
    {
        Default = 0,
        Common = 1, 
        Rare = 2, 
        Epic = 3,
        Legendary = 4
    }
}
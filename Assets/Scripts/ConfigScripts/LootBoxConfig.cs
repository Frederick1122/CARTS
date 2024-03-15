using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "LootBoxConfig", menuName = "Configs/LootBox")]
    public class LootBoxConfig : BaseConfig
    {
        [field: Header("Base")]
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField, Range(1, 100)] public float DropChance { get; private set; } = 100f;

        [Header("Loot")]
        [SerializeField] private List<Loot> _loot = new();

        public IReadOnlyList<Loot> GetAllLoot() { return _loot; }

        public CarClass GetCarClassByChance(float chance)
        {
            if (chance <= 0 || chance > 100)
                return CarClass.Default;

            _loot = _loot.OrderByDescending(loot => loot.DropChance).ToList();
            foreach (var loot in _loot)
            {
                if (loot.DropChance >= chance)
                    return loot.CarClass;

                chance -= loot.DropChance;
            }

            return CarClass.Default;
        }
    }

    [Serializable]
    public class Loot
    {
        [field:SerializeField, Range(1, 100)] public float DropChance { get; private set; } = 100f;
        [field:SerializeField] public CarClass CarClass { get; private set; } = CarClass.Default;
    }
}

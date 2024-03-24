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
        [field: SerializeField] public Rarity Rarity { get; private set; } = Rarity.Default;

        [Header("Loot")]
        [SerializeField] private List<Loot> _loot = new();

        public IReadOnlyList<Loot> GetAllLoot() { return _loot; }
        private float _totalChance => _loot.Sum(x => x.DropChance);

        public Rarity OpenLootBoxRandom()
        {
            var chance = UnityEngine.Random.Range(1f, _totalChance);
            return OpenLootBoxWithChance(chance);
        }

        public Rarity OpenLootBoxWithChance(float chance)
        {
            if (chance <= 0 || chance > _totalChance)
                return Rarity.Default;

            _loot = _loot.OrderByDescending(loot => loot.DropChance).ToList();
            foreach (var loot in _loot)
            {
                if (loot.DropChance >= chance)
                    return loot.CarClass;

                chance -= loot.DropChance;
            }

            return Rarity.Default;
        }
    }

    [Serializable]
    public class Loot
    {
        [field:SerializeField, Range(1, 100)] public float DropChance { get; private set; } = 100f;
        [field:SerializeField] public Rarity CarClass { get; private set; } = Rarity.Default;
    }
}

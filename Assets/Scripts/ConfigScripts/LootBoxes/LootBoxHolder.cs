using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Lobby.Gacha.LootBoxManager;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "LootBoxHolderConfig", menuName = "Configs/LootBoxHolder")]
    public class LootBoxHolder : BaseConfig
    {
        [SerializeField] private List<LootBoxByRarity> _lootBoxByRarity = new();
        [Header("Drop parametres")]
        [SerializeField] private List<LootBoxPermanentDrop> _lootBoxPermanentDrop = new();

        public IReadOnlyList<LootBoxByRarity> LootBoxByRarity => _lootBoxByRarity;
        public IReadOnlyList<LootBoxPermanentDrop> LootBoxPermanentDrop => _lootBoxPermanentDrop;

        public void ReorderLootBoxesByRarity()
        {
            _lootBoxByRarity = _lootBoxByRarity.OrderByDescending(box => box.DropChance).ToList();
        }

        public List<LootBoxConfig> GetLootBoxesByRarity(Rarity rarity)
        {
            foreach (var lootBoxes in _lootBoxByRarity)
            {
                if (lootBoxes.LootBoxRarity == rarity)
                    return lootBoxes.LootBoxes.ToList();
            }

            return null;
        }

        public int GetPermanentDropByRarity(Rarity rarity)
        {
            foreach (var drop in _lootBoxPermanentDrop)
            {
                if (drop.Rarity == rarity)
                    return drop.OpenOtherToDrop;
            }

            return -1;
        }
    }

    [Serializable]
    public class LootBoxByRarity
    {
        [field: SerializeField] public Rarity LootBoxRarity { get; private set; }
        [field: SerializeField, Range(1, 100)] public float DropChance { get; private set; } = 100f;
        [SerializeField] private List<LootBoxConfig> _lootBoxes = new();

        public IReadOnlyList<LootBoxConfig> LootBoxes => _lootBoxes;
    }

    [Serializable]
    public class LootBoxPermanentDrop
    {
        [field: SerializeField] public Rarity Rarity { get; private set; } = Rarity.Default;
        [field: SerializeField] public int OpenOtherToDrop { get; private set; } = 10;
    }
}

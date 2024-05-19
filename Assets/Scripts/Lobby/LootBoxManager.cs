using ConfigScripts;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lobby.Gacha
{
    public class LootBoxManager : MonoBehaviour
    {
        public int SlotCount { get; } = 1;
        public int Cost => _lootBoxHolder.Cost;
        public CurrencyType CurrencyType => _lootBoxHolder.CurrencyType;

        [SerializeField] private LootBoxHolder _lootBoxHolder;

        public IReadOnlyList<LootBoxConfig> Slots => _slots;
        private List<LootBoxConfig> _slots;

        private readonly Dictionary<Rarity, int> _permanentDropRarity = new();

        public void Init()
        {
            _slots = new List<LootBoxConfig>(SlotCount);
            for (int i = 0; i < SlotCount; i++)
                _slots.Add(null);

            foreach (var dropChance in _lootBoxHolder.LootBoxPermanentDrop)
                _permanentDropRarity.Add(dropChance.Rarity, dropChance.OpenOtherToDrop);

            _lootBoxHolder.ReorderLootBoxesByRarity();
        }

        public int AddLootBoxToSlotRandom()
        {
            foreach (var dropRarity in _permanentDropRarity)
            {
                if (dropRarity.Value <= 0)
                {
                    var lootBoxesByRarity = _lootBoxHolder.GetLootBoxesByRarity(dropRarity.Key);
                    LootBoxConfig lootBoxConfig = lootBoxesByRarity[UnityEngine.Random.Range(0, lootBoxesByRarity.Count)];
                    _permanentDropRarity[dropRarity.Key] = _lootBoxHolder.GetPermanentDropByRarity(dropRarity.Key);
                    return AddLootBoxToSlot(lootBoxConfig, dropRarity.Key);
                }
            }

            var rarity = GetRandomRarity();
            return AddLootBoxToSlot(GetRandomLootBox(rarity), rarity);
        }

        public int AddLootBoxToSlot(LootBoxConfig lootBox, Rarity rarity)
        {
            for (int i = 0; i < SlotCount; i++)
            {
                var slot = _slots[i];
                if (slot != null)
                    continue;

                _slots[i] = lootBox;
                if (_permanentDropRarity.ContainsKey(rarity))
                    _permanentDropRarity[rarity]--;
                return i;
            }

            return -1;
        }

        public Rarity OpenLootBoxSlot(int num)
        {
            if (num < 0 || num >= SlotCount)
                return Rarity.Default;

            if (_slots[num] == null)
                return Rarity.Default;

            var lootBox = _slots[num];
            _slots[num] = null;
            return lootBox.OpenLootBoxRandom();
        }

        private Rarity GetRandomRarity()
        {
            var totalChance = _lootBoxHolder.LootBoxByRarity.Sum(x => x.DropChance);
            var chance = UnityEngine.Random.Range(1f, totalChance);
            foreach (var lootBox in _lootBoxHolder.LootBoxByRarity)
            {
                if (lootBox.DropChance >= chance)
                    return lootBox.LootBoxRarity;

                chance -= lootBox.DropChance;
            }

            return Rarity.Default;
        }

        private LootBoxConfig GetRandomLootBox(Rarity rarity)
        {
            List<LootBoxConfig> lootBoxes = _lootBoxHolder.GetLootBoxesByRarity(rarity);

            return lootBoxes[UnityEngine.Random.Range(0, lootBoxes.Count)];
        }
    }
}

using ConfigScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lobby.Gacha
{
    public class LootBoxManager : MonoBehaviour
    {
        public int SlotCount { get; } = 1;

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
            foreach (var item in _permanentDropRarity)
            {
                if (item.Value <= 0)
                {
                    var lootBoxesByRarity = _lootBoxHolder.GetLootBoxesByRarity(item.Key);
                    LootBoxConfig lootBoxConfig = lootBoxesByRarity[UnityEngine.Random.Range(0, lootBoxesByRarity.Count)];
                    _permanentDropRarity[item.Key] = _lootBoxHolder.GetPermanentDropByRarity(item.Key);
                    return AddLootBoxToSlot(lootBoxConfig);
                }
            }

            return AddLootBoxToSlot(GetRandomLootBox());
        }

        public int AddLootBoxToSlot(LootBoxConfig lootBox)
        {
            for (int i = 0; i < SlotCount; i++)
            {
                var slot = _slots[i];
                if (slot != null)
                    continue;

                _slots[i] = lootBox;
                var rarity = lootBox.Rarity;
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

        private LootBoxConfig GetRandomLootBox()
        {
            List<LootBoxConfig> lootBoxes = _lootBoxHolder.GetLootBoxesByRarity(GetRandomRarity());

            return lootBoxes[UnityEngine.Random.Range(0, lootBoxes.Count)];
        }
    }
}

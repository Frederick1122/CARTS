using ConfigScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lobby.Gacha
{
    public class LootBoxManager : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private int _slotCount = 3;
        [SerializeField] private List<LootBoxConfig> _lootBoxConfigs = new();

        [Header("Drop parametres")]
        [SerializeField] private List<LootBoxDropChance> _lootBoxDropChance = new();

        public IReadOnlyList<LootBoxConfig> Slots => _slots;
        private List<LootBoxConfig> _slots;

        private readonly Dictionary<Rarity, int> _permanentDropRarity = new();
        private readonly Dictionary<Rarity, int> _permanentDropRarityPermanent = new();

        public void Init()
        {
            _slots = new List<LootBoxConfig>(_slotCount) { null };
            for (int i = 0; i < _slotCount; i++)
                _slots[i] = null;

            foreach (var dropChance in _lootBoxDropChance)
                _permanentDropRarity.Add(dropChance.Rarity, dropChance.OpenOtherToDrop);

            foreach (var dropChance in _lootBoxDropChance)
                _permanentDropRarityPermanent.Add(dropChance.Rarity, dropChance.OpenOtherToDrop);

            _lootBoxConfigs = _lootBoxConfigs.OrderByDescending(box => box.DropChance).ToList();
        }

        public int AddLootBoxToSlotRandom()
        {
            LootBoxConfig lootBoxConfig = null;

            foreach (var item in _permanentDropRarity)
            {
                if (item.Value <= 0)
                {
                    var lootBoxesByRarity = _lootBoxConfigs.FindAll(lootBox => lootBox.Rarity == item.Key).ToArray();
                    lootBoxConfig = lootBoxesByRarity[UnityEngine.Random.Range(0, lootBoxesByRarity.Length)];
                    _permanentDropRarity[item.Key] = _permanentDropRarityPermanent[item.Key];
                    break;
                }
            }

            if (lootBoxConfig == null)
                lootBoxConfig = GetRandomLootBox();

            return AddLootBoxToSlot(lootBoxConfig);
        }

        public int AddLootBoxToSlot(LootBoxConfig lootBox)
        {
            for (int i = 0; i < _slotCount; i++)
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
            if (num < 0 || num >= _slotCount)
                return Rarity.Default;

            if (_slots[num] == null)
                return Rarity.Default;

            var lootBox = _slots[num];
            return lootBox.OpenLootBoxRandom();
        }

        private LootBoxConfig GetRandomLootBox()
        {
            var totalChance = _lootBoxConfigs.Sum(x => x.DropChance);
            var chance = UnityEngine.Random.Range(1f, totalChance);
            foreach (var lootBox in _lootBoxConfigs)
            {
                if (lootBox.DropChance >= chance)
                    return lootBox;

                chance -= lootBox.DropChance;
            }

            return _lootBoxConfigs[UnityEngine.Random.Range(0, _lootBoxConfigs.Count)];
        }


        [Serializable]
        public class LootBoxDropChance
        {
            [field: SerializeField] public int OpenOtherToDrop = 10;
            [field: SerializeField] public Rarity Rarity = Rarity.Default;
        }
    }
}

using ConfigScripts;
using Lobby.Gacha;
using Managers;
using Managers.Libraries;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UI.Windows.Shop.Sections.Gacha
{
    public class GachaWindowController : UIController
    {
        [SerializeField] private LootboxRewardWindowController _lootboxRewardController;

        [Header("Loot box Cost")]
        [SerializeField] private int _cost;
        [SerializeField] private CurrencyType _currencyType;

        private GachaWindowView _castView;
        private LootBoxManager _lootBoxManager => LobbyManager.Instance.LootBoxManager;

        private GachaWindowModel _model;
        private Price _price;

        public override void Init()
        {
            _model = new(_cost);
            _price = new(_cost, _currencyType);

            _lootboxRewardController.Init();
            _lootboxRewardController.Hide();
            //_lootboxRewardController.OnOpen += TurnOffSlotButtons;
            //_lootboxRewardController.OnClose += TurnOnSlotButtons;

            base.Init();
            _castView = GetView<GachaWindowView>();

            _castView.OnBuyLootBox += BuyLootBox;
            //_castView.OnOpenLootBox += OpenLootBox;
        }

        private void OnDestroy()
        {
            if (_castView == null)
                return;

            _castView.OnBuyLootBox -= BuyLootBox;
            //_castView.OnOpenLootBox -= OpenLootBox;
        }

        public override void Show()
        {
            base.Show();
            _castView.ChangeBuyButtonCondition(PlayerManager.Instance.IsEnoughMoney(_price));
            //UpdateSlotsImage();
        }

        protected override UIModel GetViewData() { return _model; }

        private void BuyLootBox()
        {
            if (!PlayerManager.Instance.IsEnoughMoney(_price))
                return;

            int slotNum = _lootBoxManager.AddLootBoxToSlotRandom();

            if (slotNum < 0)
                return;

            PlayerManager.Instance.DecreaseCurrency(_price);
            //_castView.UpdateLootBoxImage(slotNum, _lootBoxManager.Slots[slotNum].Icon);

            _castView.ChangeBuyButtonCondition(PlayerManager.Instance.IsEnoughMoney(_price));
            OpenLootBox(0);
        }

        private void OpenLootBox(int slotNum)
        {
            var rarity = _lootBoxManager.OpenLootBoxSlot(slotNum);

            if (rarity == Rarity.Default)
                return;

            var config = ((CarLibrary)CarLibrary.Instance).GetRandomConfigByRarity(rarity);

            _lootboxRewardController.OpenLootBox(config);

            if (PlayerManager.Instance.TryGetPurchasedCarData(config.configKey, out CarData _))
                PlayerManager.Instance.IncreaseCurrency(config.dublicatePrice);
            else
                PlayerManager.Instance.TryToPurchaseCar(config.configKey);

            //_castView.UpdateLootBoxImage(slotNum, null);
            _castView.ChangeBuyButtonCondition(PlayerManager.Instance.IsEnoughMoney(_price));
        }

        //private void UpdateSlotsImage()
        //{
        //    for (int slotNum = 0; slotNum < _lootBoxManager.SlotCount; slotNum++)
        //    {
        //        var lootbox = _lootBoxManager.Slots[slotNum];
        //        Sprite icon = null;
        //        if (lootbox != null)
        //            icon = lootbox.Icon;
        //        _castView.UpdateLootBoxImage(slotNum, icon);
        //    }
        //}

        //private void TurnOnSlotButtons() => GetView<GachaWindowView>().TryChangeSlotButtonsCondition(true);
        //private void TurnOffSlotButtons() => GetView<GachaWindowView>().ChangeSlotButtonsCondition(false);
    }
}

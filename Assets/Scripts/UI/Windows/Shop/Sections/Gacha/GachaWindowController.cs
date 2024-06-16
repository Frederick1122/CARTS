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

        private GachaWindowView _castView;
        private LootBoxManager _lootBoxManager => LobbyManager.Instance.LootBoxManager;

        private GachaWindowModel _model;
        private Price _price;

        public override void Init()
        {
            _lootboxRewardController.Init();
            _lootboxRewardController.Hide();

            base.Init();

            _castView = GetView<GachaWindowView>();
            _castView.OnBuyLootBox += BuyLootBox;
        }

        private void OnDestroy()
        {
            if (_castView == null)
                return;

            _castView.OnBuyLootBox -= BuyLootBox;
        }

        public override void Show()
        {
            UpdateView();
            base.Show();
        }

        public override void UpdateView()
        {
            _model = new(_lootBoxManager.Cost);
            _price = new(_lootBoxManager.Cost, _lootBoxManager.CurrencyType);
            _castView.ChangeBuyButtonCondition(PlayerManager.Instance.IsEnoughMoney(_price));
            base.UpdateView();
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

            _castView.ChangeBuyButtonCondition(PlayerManager.Instance.IsEnoughMoney(_price));
        }
    }
}

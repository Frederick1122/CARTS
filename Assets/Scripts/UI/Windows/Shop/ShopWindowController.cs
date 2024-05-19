using Managers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using UI.Widgets.CurrencyWidget;
using UI.Windows.Shop.Sections.BattlePass;
using UI.Windows.Shop.Sections.Gacha;
using UI.Windows.Shop.Sections.SaleShop;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopWindowController : UIController
    {
        public event Action OnOpenLobby = delegate { };

        [Header("Sections")]
        [SerializeField] private GachaWindowController _gachaWindowController;
        [SerializeField] private SaleShopWindowController _saleShopWindowController;
        [SerializeField] private BattlePassWindowController _battlePassWindowController;

        private readonly List<UIController> _allSectionWindows = new();

        public override void Init()
        {
            _view.Init(GetViewData());

            _gachaWindowController.Init();
            _saleShopWindowController.Init();
            _battlePassWindowController.Init();

            _allSectionWindows.Add(_gachaWindowController);
            _allSectionWindows.Add(_saleShopWindowController);
            _allSectionWindows.Add(_battlePassWindowController);

            var castView = GetView<ShopWindowView>();
            castView.OnOpenLobby += RequestToOpenLobby;
            castView.OnOpenBattlePass += RequestToOpenBattlePass;
            castView.OnOpenGacha += RequestToOpenGacha;
            castView.OnOpenSaleShop += RequestToOpenSaleShop;
        }

        private void OnDestroy()
        {
            var castView = GetView<ShopWindowView>();
            castView.OnOpenLobby -= RequestToOpenLobby;
            castView.OnOpenBattlePass -= RequestToOpenBattlePass;
            castView.OnOpenGacha -= RequestToOpenGacha;
            castView.OnOpenSaleShop -= RequestToOpenSaleShop;
        }

        public override void Show()
        {
            base.Show();
            ShowStartSection();
            //temp
            //BuyAllCarDev();
            UIManager.Instance.GetWidgetUI().ShowWindow(typeof(CurrencyWidgetController), false);
        }

        public override void Hide()
        {
            base.Hide();
            UIManager.Instance.GetWidgetUI().HideWindow(typeof(CurrencyWidgetController));
        }

        protected override UIModel GetViewData() { return new ShopWindowModel(); }

        private void RequestToOpenLobby() => OnOpenLobby?.Invoke();

        private void ShowStartSection()
        {
            HideAllSectionWindows();
            _gachaWindowController.Show();
            GetView<ShopWindowView>().OpenStartSection();
        }

        private void HideAllSectionWindows()
        {
            foreach (var window in _allSectionWindows)
                window.Hide();
        }

        private void RequestToOpenGacha()
        {
            HideAllSectionWindows();
            _gachaWindowController.Show();
        }

        private void RequestToOpenSaleShop()
        {
            HideAllSectionWindows();
            _saleShopWindowController.Show();
        }

        private void RequestToOpenBattlePass()
        {
            HideAllSectionWindows();
            _battlePassWindowController.Show();
        }

        [ContextMenu("BuyAllCar")]
        private void BuyAllCarDev()
        {
            foreach (var config in ((CarLibrary)CarLibrary.Instance).GetConfigsWithoutAI())
            {
                PlayerManager.Instance.TryToPurchaseCar(config.configKey);
                PlayerManager.Instance.SetCurrentCar(config.configKey);
            }
        }
    }
}
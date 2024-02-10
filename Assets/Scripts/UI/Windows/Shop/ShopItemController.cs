using System;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopItemController : UIController
    {
        public event Action<ShopItemModel> OnSelectCarAction;

        private ShopItemModel _uiModel = new();

        public override void Init()
        {
            GetView<ShopItemView>().OnSelectCarAction += SelectCar;
            base.Init();
        }

        public override void Show()
        {
            UpdateView(GetViewData());
            base.Show();
        }

        public override void UpdateView(UIModel uiModel)
        {
            _uiModel = (ShopItemModel)uiModel;
            base.UpdateView(uiModel);
        }

        protected override UIModel GetViewData()
        {
            return _uiModel;
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            GetView<ShopItemView>().OnSelectCarAction -= SelectCar;
        }

        public void SetUpModel(string configKey, bool isSelectedCar, bool purchased, Sprite icon, int price) =>
            _uiModel = new ShopItemModel(configKey, isSelectedCar, purchased, icon, price);

        private void SelectCar() =>
            OnSelectCarAction?.Invoke(_uiModel);
    }
}
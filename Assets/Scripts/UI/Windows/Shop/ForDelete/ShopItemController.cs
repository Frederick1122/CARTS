using Managers;
using System;

namespace UI.Windows.Shop
{
    public class ShopItemController : UIController
    {
        public event Action OnCarBuy = delegate { };

        private ShopItemModel _model = new();

        public override void Init()
        {
            GetView<ShopItemView>().OnCarBuy += BuyCar;
            base.Init();
        }

        private void OnDestroy() => 
            GetView<ShopItemView>().OnCarBuy -= BuyCar;

        public void UpdateInfo(CarData data)
        {
            var purchased = PlayerManager.Instance.TryGetPurchasedCarData(data.configKey, out CarData _);
            _model = new(data.configKey, purchased, 0);

            UpdateView();
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }

        private void BuyCar() =>
            OnCarBuy?.Invoke();
    }
}
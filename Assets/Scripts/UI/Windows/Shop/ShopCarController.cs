using System;

namespace UI.Windows.Shop
{
    public class ShopCarController : UIController
    {
        public event Action<ShopCarModel> OnSelectCarAction;

        private ShopCarModel _uiModel = new();

        public override void Init()
        {
            GetView<ShopCarView>().OnSelectCarAction += SelectCar;
            base.Init();
        }

        public override void UpdateView(UIModel uiModel)
        {
            _uiModel = (ShopCarModel)uiModel;
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

            GetView<ShopCarView>().OnSelectCarAction -= SelectCar;
        }

        private void SelectCar() =>
            OnSelectCarAction?.Invoke(_uiModel);
    }
}
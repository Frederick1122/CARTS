using System;

namespace UI.Windows.Shop
{
    public class ShopCarController : UIController<ShopCarView, ShopCarModel>
    {
        public event Action<ShopCarModel> OnSelectCarAction;

        private ShopCarModel _uiModel = new();

        public override void Init()
        {
            _view.OnSelectCarAction += SelectCar;
            base.Init();
        }

        public override void UpdateView(ShopCarModel uiModel)
        {
            _uiModel = uiModel;
            base.UpdateView(uiModel);
        }

        protected override ShopCarModel GetViewData()
        {
            return _uiModel;
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            _view.OnSelectCarAction -= SelectCar;
        }

        private void SelectCar()
        {
            OnSelectCarAction?.Invoke(_uiModel);
        }
    }
}
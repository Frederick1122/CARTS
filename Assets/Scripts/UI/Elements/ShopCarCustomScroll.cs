using System;
using UI.Windows.Shop;

namespace UI.Elements
{
    public sealed class ShopCarCustomScroll : CustomScroll<ShopCarController, ShopCarView, ShopCarModel>
    {
        public event Action<ShopCarModel> OnSelectCarAction;

        public override void AddElement(ShopCarModel uiModel)
        {
            var isNewController = _hidingControllers.Count == 0;
            base.AddElement(uiModel);

            if (isNewController)
                _activeControllers[^1].OnSelectCarAction += SelectCar;
        }

        private void OnDestroy()
        {
            foreach (var activeController in _activeControllers)
            {
                if (activeController == null)
                    continue;

                activeController.OnSelectCarAction -= SelectCar;
            }

            foreach (var hidingController in _hidingControllers)
            {
                if (hidingController == null)
                    continue;

                hidingController.OnSelectCarAction -= SelectCar;
            }
        }

        private void SelectCar(ShopCarModel uiModel)
        {
            OnSelectCarAction?.Invoke(uiModel);
        }
    }
}
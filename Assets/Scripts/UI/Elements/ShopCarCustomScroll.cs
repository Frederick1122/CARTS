using System;
using UI.Windows.Shop;

namespace UI.Elements
{
    public sealed class ShopCarCustomScroll : CustomScroll<ShopItemController, ShopItemModel>
    {
        //public event Action<ShopItemModel> OnSelectCarAction;

        //public override void AddElement(ShopItemModel uiModel)
        //{
        //    var isNewController = _hidingControllers.Count == 0;
        //    base.AddElement(uiModel);

        //    if (isNewController)
        //        _activeControllers[^1].OnSelectCarAction += SelectCar;
        //}

        //private void OnDestroy()
        //{
        //    foreach (var activeController in _activeControllers)
        //    {
        //        if (activeController == null)
        //            continue;

        //        activeController.OnSelectCarAction -= SelectCar;
        //    }

        //    foreach (var hidingController in _hidingControllers)
        //    {
        //        if (hidingController == null)
        //            continue;

        //        hidingController.OnSelectCarAction -= SelectCar;
        //    }
        //}

        //private void SelectCar(ShopItemModel uiModel) =>
        //    OnSelectCarAction?.Invoke(uiModel);
    }
}
namespace UI.Windows.Shop
{
    public class ShopWindowController : UIController<ShopWindowView, ShopWindowModel>
    {
        protected override ShopWindowModel GetViewData()
        {
            return new ShopWindowModel();
        }
    }
}
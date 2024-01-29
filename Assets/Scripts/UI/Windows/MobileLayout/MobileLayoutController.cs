using Cars.InputSystem.Player;

namespace UI.Windows.MobileLayout
{
    public class MobileLayoutController : UIController<MobileLayoutView, MobileLayoutModel>
    {
        protected override MobileLayoutModel GetViewData()
        {
            return new MobileLayoutModel();
        }

        public IButtonsInput GetButtons()
        {
            return _view;
        }
    }
}

using Cars.InputSystem.Player;

namespace UI.Windows.MobileLayout
{
    public class MobileLayoutController : UIController
    {
        protected override UIModel GetViewData()
        {
            return new MobileLayoutModel();
        }

        public IButtonsInput GetButtons()
        {
            return GetView<MobileLayoutView>();
        }
    }
}

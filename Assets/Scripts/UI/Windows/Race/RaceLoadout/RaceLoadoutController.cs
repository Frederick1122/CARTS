using Cars.InputSystem.Player;

namespace UI.Windows.RaceLoadOut
{
    public class RaceLoadoutController : UIController<RaceLoadOutView, RaceLoadOutModel>
    {
        protected override RaceLoadOutModel GetViewData()
        {
            return new RaceLoadOutModel();
        }

        public IButtonsInput GetButtons()
        {
            return _view;
        }
    }
}

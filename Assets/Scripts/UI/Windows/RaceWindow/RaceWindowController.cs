
namespace UI.Windows.Race
{
    public class RaceWindowController : UIController
    {
        protected RaceWindowModel _model;

        protected override UIModel GetViewData()
        {
            return _model;
        }
    }

    public class RaceWindowView : UIView { }

    public class RaceWindowModel : UIModel { }
}

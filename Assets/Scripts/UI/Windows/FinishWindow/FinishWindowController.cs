namespace UI.Windows.Finish
{
    public class FinishWindowController : UIController
    {
        protected FinishWindowModel _model;

        protected override UIModel GetViewData()
        {
            return _model;
        }
    }

    public class FinishWindowView : UIView { }

    public class FinishWindowModel : UIModel { }
}

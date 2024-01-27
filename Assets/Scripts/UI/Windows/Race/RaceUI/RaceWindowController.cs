namespace UI.Windows.Race.RaceUI
{
    public class RaceWindowController : UIController<RaceWindowView, RaceWindowModel>
    {
        private RaceWindowModel _model;
     
        
        protected override RaceWindowModel GetViewData()
        {
            return _model;
        }
    }
}
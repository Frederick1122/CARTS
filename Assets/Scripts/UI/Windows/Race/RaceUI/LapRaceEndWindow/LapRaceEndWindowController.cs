namespace UI.Windows.LapRace.LapRaceEndWindow
{
    public class LapRaceEndWindowController : UIController<LapRaceEndWindowView, LapRaceEndWindowModel>
    {
        private LapRaceEndWindowModel _lapRaceEndWindowModel;
            
        
        
        protected override LapRaceEndWindowModel GetViewData()
        {
            return _lapRaceEndWindowModel;
        }
    }
}
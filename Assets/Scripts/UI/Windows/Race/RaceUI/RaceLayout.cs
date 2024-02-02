using Race.RaceManagers;

namespace UI.Windows
{
    public class RaceLayout : UIController
    {
        public virtual RaceType raceType { get; }
        
        protected override UIModel GetViewData()
        {
            throw new System.NotImplementedException();
        }
    }
}
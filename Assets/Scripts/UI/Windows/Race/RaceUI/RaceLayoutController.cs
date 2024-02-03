using Race.RaceManagers;
using UI.Windows.LapRace;

namespace UI.Windows
{
    public abstract class RaceLayoutController : UIController
    {
        public virtual RaceType raceType { get; }
        
        protected override UIModel GetViewData()
        {
            return new RaceLayoutModel();
        }
    }
}
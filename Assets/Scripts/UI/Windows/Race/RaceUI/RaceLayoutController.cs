using Race.RaceManagers;

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
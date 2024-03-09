using Race.RaceManagers;
using UI.Windows.LapRace;

namespace UI.Windows
{
    public abstract class RaceLayoutController : UIController
    {
        public virtual RaceType raceType { get; }

        public void SetStartDelay(int delay)
        {
            if (delay == 0)
                return;

            var model = new RaceLayoutModel(delay); 
            _view.UpdateView(model);
        }
         
        protected override UIModel GetViewData()
        {
            return new RaceLayoutModel();
        }
    }
}
using Race.RaceManagers;

namespace UI.Windows
{
    public abstract class RaceLayoutController : UIController
    {
        public virtual RaceType raceType { get; }

        public override void Show()
        {
            base.Show();
        }

        protected override UIModel GetViewData()
        {
            return new RaceLayoutModel();
        }
    }
}
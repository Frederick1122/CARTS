using Race.RaceManagers;
using UI.Windows.Finish;

namespace UI.Windows.RaceUI.LapRace
{
    public class LapRaceFinishWindowController : FinishWindowController
    {
        private LapRaceFinishWindowModel _lapRaceFinishWindowModel = new ();

        public override void Init()
        {
            base.Init();
            GetView<LapRaceFinishWindowView>().OnGoToMainMenuAction += GoToMainMenu;
        }

        private void OnDestroy()
        {
            GetView<LapRaceFinishWindowView>().OnGoToMainMenuAction -= GoToMainMenu;
        }
        
        public override void Show()
        {
            base.Show();
            _lapRaceFinishWindowModel.maxPosition = RaceManager.Instance.GetState<LapRaceState>().GetMaxPositions();
            _lapRaceFinishWindowModel.currentPosition = RaceManager.Instance.GetState<LapRaceState>().GetPlayerPosition();
            _lapRaceFinishWindowModel.passTime = RaceManager.Instance.GetState<LapRaceState>().GetPassTime();
            UpdateView(_lapRaceFinishWindowModel);
        }

        protected override UIModel GetViewData()
        {
            return _lapRaceFinishWindowModel;
        }
    }
}
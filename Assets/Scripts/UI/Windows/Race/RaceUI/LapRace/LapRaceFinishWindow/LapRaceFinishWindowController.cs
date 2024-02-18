using Race.RaceManagers;
using UI.Windows.Finish;

namespace UI.Windows.RaceUI.LapRace
{
    public class LapRaceFinishWindowController : FinishWindowController
    {
        private LapRaceFinishWindowModel _lapRaceFinishWindowModel = new ();
        
        private LapRaceState _lapRaceState;

        public override void Init()
        {
            _lapRaceState = (LapRaceState)RaceManager.Instance.GetState(RaceType.LAP_RACE);
            
            GetView<LapRaceFinishWindowView>().OnGoToMainMenuAction += GoToMainMenu;
            base.Init();
        }

        private void OnDestroy()
        {
            GetView<LapRaceFinishWindowView>().OnGoToMainMenuAction -= GoToMainMenu;
        }

        public override void Show()
        {
            base.Show();
            _lapRaceFinishWindowModel.result = RaceManager.Instance.GetResult();
            _lapRaceFinishWindowModel.maxPosition = _lapRaceState.GetMaxPositions();
            _lapRaceFinishWindowModel.currentPosition = _lapRaceState.GetPlayerPosition();
            _lapRaceFinishWindowModel.passTime = _lapRaceState.GetPassTime();
            UpdateView(_lapRaceFinishWindowModel);
        }
        
        protected override UIModel GetViewData()
        {
            return _lapRaceFinishWindowModel;
        }
    }
}
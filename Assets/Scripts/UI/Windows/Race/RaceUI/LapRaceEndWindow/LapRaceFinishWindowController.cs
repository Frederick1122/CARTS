using System;
using Race.RaceManagers;
using UI.Windows.Finish;

namespace UI.Windows.LapRace.LapRaceEndWindow
{
    public class LapRaceFinishWindowController : FinishWindowController
    {
        private LapRaceEndWindowModel _lapRaceEndWindowModel = new ();

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
            _lapRaceEndWindowModel.maxPosition = RaceManager.Instance.GetState<LapRaceState>().GetMaxPositions();
            _lapRaceEndWindowModel.currentPosition = RaceManager.Instance.GetState<LapRaceState>().GetPlayerPosition();
            _lapRaceEndWindowModel.passTime = RaceManager.Instance.GetState<LapRaceState>().GetPassTime();
            UpdateView(_lapRaceEndWindowModel);
        }

        protected override UIModel GetViewData()
        {
            return _lapRaceEndWindowModel;
        }
    }
}
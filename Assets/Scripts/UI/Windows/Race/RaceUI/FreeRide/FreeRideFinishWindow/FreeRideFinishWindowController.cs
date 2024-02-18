using Race.RaceManagers;
using UI.Windows.Finish;

namespace UI.Windows.Race.RaceUI.FreeRide
{
    public class FreeRideFinishWindowController : FinishWindowController
    {
        private FreeRideFinishWindowModel _freeRideFinishWindowModel = new();

        private FreeRideState _freeRideState;

        public override void Init()
        {
            base.Init();
            _freeRideState = (FreeRideState)RaceManager.Instance.GetState(RaceType.FREE_RIDE);

            GetView<FreeRideFinishWindowView>().OnGoToMainMenuAction += GoToMainMenu;
        }

        private void OnDestroy()
        {
            GetView<FreeRideFinishWindowView>().OnGoToMainMenuAction -= GoToMainMenu;
        }
        
        public override void Show()
        {
            base.Show();
            _freeRideFinishWindowModel.result = RaceManager.Instance.GetResult();
            _freeRideFinishWindowModel.score = _freeRideState.GetScore();
            _freeRideFinishWindowModel.passTime = _freeRideState.GetPassTime();
            UpdateView(_freeRideFinishWindowModel);
        }

        protected override UIModel GetViewData()
        {
            return _freeRideFinishWindowModel;
        }
    }
}
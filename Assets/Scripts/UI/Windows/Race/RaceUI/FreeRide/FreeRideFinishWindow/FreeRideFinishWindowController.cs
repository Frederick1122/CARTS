using Race.RaceManagers;
using UI.Windows.Finish;

namespace UI.Windows.Race.RaceUI.FreeRide
{
    public class FreeRideFinishWindowController : FinishWindowController
    {
        private FreeRideFinishWindowModel _freeRideFinishWindowModel = new();
        
        public override void Init()
        {
            base.Init();
            GetView<FreeRideFinishWindowView>().OnGoToMainMenuAction += GoToMainMenu;
        }

        private void OnDestroy()
        {
            GetView<FreeRideFinishWindowView>().OnGoToMainMenuAction -= GoToMainMenu;
        }
        
        public override void Show()
        {
            base.Show();
            var freeRideState = (FreeRideState)RaceManager.Instance.GetState(RaceType.FREE_RIDE);
            
            _freeRideFinishWindowModel.score = freeRideState.GetResult();
            _freeRideFinishWindowModel.passTime = freeRideState.GetPassTime();
            UpdateView(_freeRideFinishWindowModel);
        }

        protected override UIModel GetViewData()
        {
            return _freeRideFinishWindowModel;
        }
    }
}
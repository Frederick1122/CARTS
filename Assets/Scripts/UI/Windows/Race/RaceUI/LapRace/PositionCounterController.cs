using Race.RaceManagers;

namespace UI.Windows.LapRace
{
    public class PositionCounterController : UIController
    {
        private readonly CounterModel _counterModel = new();
        private LapRaceState _lapRaceState;

        public override void Init()
        {
            _lapRaceState = (LapRaceState)RaceManager.Instance.GetState(RaceType.LAP_RACE);
            _lapRaceState.OnStartAction += ResetCounter;
            _lapRaceState.OnPlayerChangePositionAction += ChangePosition;
            base.Init();
        }
        
        private void OnDestroy()
        {
            if (_lapRaceState == null)
                return;

            _lapRaceState.OnStartAction -= ResetCounter;
            _lapRaceState.OnPlayerChangePositionAction -= ChangePosition;
        }

        private void ResetCounter()
        {
            _counterModel.maxCount = _lapRaceState.GetMaxPositions();
            UpdateView(_counterModel);
        }

        private void ChangePosition(int position)
        {
            _counterModel.count = position;
            UpdateView(_counterModel);
        }

        protected override UIModel GetViewData()
        {
            return _counterModel;
        }
    }
}
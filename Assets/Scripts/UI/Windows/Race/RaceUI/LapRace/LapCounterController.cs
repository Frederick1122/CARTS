using Race;
using Race.RaceManagers;
using UnityEngine;

namespace UI.Windows.LapRace
{
    public class LapCounterController : UIController
    {
        private readonly CounterModel _counterModel = new();
        private LapRaceState _lapRaceState;

        public override void Init()
        {
            _lapRaceState = (LapRaceState)RaceManager.Instance.GetState(RaceType.LAP_RACE);

            _lapRaceState.OnStartAction += ResetCounter;
            _lapRaceState.OnPlayerEndsLapAction += IncreaseLapCount;
            base.Show();
        }

        private void OnDestroy()
        {
            if (_lapRaceState == null)
                return;

            _lapRaceState.OnStartAction -= ResetCounter;
            _lapRaceState.OnPlayerEndsLapAction -= IncreaseLapCount;
        }
        
        private void ResetCounter()
        {
            _counterModel.maxCount = _lapRaceState.GetMaxLapCount();
            _counterModel.count = 1;
            UpdateView(_counterModel);
        }

        private void IncreaseLapCount()
        {
            _counterModel.count = Mathf.Clamp(_counterModel.count + 1, 1, _counterModel.maxCount);
            UpdateView(_counterModel);
        }

        protected override UIModel GetViewData()
        {
            return _counterModel;
        }
    }
}
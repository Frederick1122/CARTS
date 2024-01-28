using Race;
using UnityEngine;

namespace UI.Windows.Race.RaceUI
{
    public class LapCounterController : UIController<CounterView, CounterModel>
    {
        private CounterModel _counterModel = new ();

        public override void Show()
        {
            if (_counterModel.maxCount == 0)
            {
                _counterModel.maxCount = ((LapRaceManager)LapRaceManager.Instance).GetMaxLapCount();
                _counterModel.count = 1;   
            }
            ((LapRaceManager) LapRaceManager.Instance).OnPlayerEndsLapAction += IncreaseLapCount;
            UpdateView(_counterModel);
            base.Show();
        }

        public override void Hide()
        {
            if (LapRaceManager.Instance != null)
                ((LapRaceManager) LapRaceManager.Instance).OnPlayerEndsLapAction -= IncreaseLapCount;
            base.Hide();
        }

        private void OnDestroy()
        {
            if (LapRaceManager.Instance != null)
                ((LapRaceManager) LapRaceManager.Instance).OnPlayerEndsLapAction -= IncreaseLapCount;
        }

        private void IncreaseLapCount()
        {
            _counterModel.count = Mathf.Clamp(_counterModel.count + 1, 1, _counterModel.maxCount);
            UpdateView(_counterModel);
        }
        
        protected override CounterModel GetViewData()
        {
            return _counterModel;
        }
    }
}
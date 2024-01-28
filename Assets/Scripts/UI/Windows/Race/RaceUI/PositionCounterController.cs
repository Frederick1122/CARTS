using Race;

namespace UI.Windows.Race.RaceUI
{
    public class PositionCounterController : UIController<CounterView, CounterModel>
    {
        private CounterModel _counterModel = new ();

        public override void Show()
        {
            base.Show();
            _counterModel.maxCount = ((LapRaceManager) LapRaceManager.Instance).GetMaxPositions();
            _counterModel.count = ((LapRaceManager) LapRaceManager.Instance).GetPlayerPosition();
            ((LapRaceManager) LapRaceManager.Instance).OnPlayerChangePositionAction += ChangePosition;
            UpdateView(_counterModel);
        }

        public override void Hide()
        {
            if (LapRaceManager.Instance != null)
                ((LapRaceManager) LapRaceManager.Instance).OnPlayerChangePositionAction -= ChangePosition;
            base.Hide();
        }

        private void OnDestroy()
        {
            if (LapRaceManager.Instance != null)
                ((LapRaceManager) LapRaceManager.Instance).OnPlayerChangePositionAction -= ChangePosition;
        }

        private void ChangePosition(int position)
        {
            _counterModel.count = position;
            UpdateView(_counterModel);   
        }

        protected override CounterModel GetViewData() => _counterModel;
    }
}
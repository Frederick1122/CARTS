namespace UI.Windows.Race.StartDelay
{
    public class StartDelayController : UIController
    {
        private StartDelayModel _model = new StartDelayModel() {delay = 0};

        public void SetDelay(int delay)
        {
            _model.delay = delay;
            UpdateView();
        }
        
        protected override UIModel GetViewData()
        {
            return _model;
        }
    }
}
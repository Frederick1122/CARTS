namespace UI.Widgets.HintWidget
{
    public class HintWidgetController : UIController
    {
        private HintWidgetModel _model = new();

        public void SetHint(string hintText)
        {
            _model.text = hintText;
            UpdateView();
        }
        
        public override void Hide()
        {
            base.Hide();
            _model.text = "";
        }
        
        protected override UIModel GetViewData()
        {
            return _model;
        }
    }
}
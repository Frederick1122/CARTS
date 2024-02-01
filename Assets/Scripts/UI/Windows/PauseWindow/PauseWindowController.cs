using UnityEngine;

namespace UI.Windows.Pause
{
    public class PauseWindowController : UIController
    {
        protected PauseWindowModel _model;

        public override void Show()
        {
            base.Show();
            Time.timeScale = 0;
        }

        public override void Hide()
        {
            base.Hide();
            Time.timeScale = 1;
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }
    }

    public class PauseWindowModel : UIModel { }
}

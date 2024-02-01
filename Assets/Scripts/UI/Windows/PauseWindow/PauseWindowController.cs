using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Pause
{
    public class PauseWindowController : UIController<PauseWindowView, PauseWindowModel>
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

        protected override PauseWindowModel GetViewData()
        {
            return _model;
        }
    }

    public class PauseWindowView : UIView<PauseWindowModel> { }

    public class PauseWindowModel : UIModel { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Finish
{
    public class FinishWindowController : UIController<FinishWindowView, FinishWindowModel>
    {
        protected FinishWindowModel _model;

        protected override FinishWindowModel GetViewData()
        {
            return _model;
        }
    }

    public class FinishWindowView : UIView<FinishWindowModel> { }

    public class FinishWindowModel : UIModel { }
}

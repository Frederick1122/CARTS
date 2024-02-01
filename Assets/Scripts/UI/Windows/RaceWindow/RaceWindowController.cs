using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Race
{
    public class RaceWindowController : UIController<RaceWindowView, RaceWindowModel>
    {
        protected RaceWindowModel _model;

        protected override RaceWindowModel GetViewData()
        {
            return _model;
        }
    }

    public class RaceWindowView : UIView<RaceWindowModel> { }

    public class RaceWindowModel : UIModel { }
}

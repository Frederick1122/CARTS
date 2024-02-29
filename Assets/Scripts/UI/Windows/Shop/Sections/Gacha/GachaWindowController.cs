using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Shop.Sections.Gacha
{
    public class GachaWindowController : UIController
    {
        protected override UIModel GetViewData() { return new GachaWindowModel(); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UI.Windows.MobileLayout;
using UnityEngine;

namespace UI
{
    public class MobileLayoutUI : WindowManager
    {
        [SerializeField] private MobileLayoutController _mobileLayoutController;

        protected override void AddControllers() =>
            _controllers.Add(_mobileLayoutController.GetType(), _mobileLayoutController);
    }
}

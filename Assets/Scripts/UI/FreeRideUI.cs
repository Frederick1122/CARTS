using UI.Windows.FreeRide;
using UnityEngine;

namespace UI
{
    public class FreeRideUI : WindowManager
    {
        [SerializeField] private FreeRideWindowController _freeRideWindowController;

        protected override void AddControllers() =>
            _controllers.Add(_freeRideWindowController.GetType(), _freeRideWindowController);
    }
}

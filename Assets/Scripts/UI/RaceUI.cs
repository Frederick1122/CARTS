using UI.Windows.LapRace;
using UnityEngine;

namespace UI
{
    public class RaceUI : WindowManager
    {
        [SerializeField] private Windows.LapRace.RaceWindowController _raceWindowController;

        protected override void AddControllers() =>
            _controllers.Add(_raceWindowController.GetType(), _raceWindowController);
    }
}

using UI.Windows.Race.RaceUI;
using UI.Windows.RaceLoadOut;
using UnityEngine;

namespace UI
{
    public class RaceUI : WindowManager
    {
        [SerializeField] private RaceLoadoutController _raceLoadoutController;
        [SerializeField] private RaceWindowController _raceWindowController;

        [ContextMenu("Init")]
        public override void Init()
        {
            base.Init();
            _raceLoadoutController.Init();
            _raceWindowController.Init();
        }

        protected override void AddControllers()
        {
            _controllers.Add(_raceLoadoutController.GetType(), _raceLoadoutController);
            _controllers.Add(_raceWindowController.GetType(), _raceWindowController);

        }
    }
}

using UnityEngine;

namespace UI.Windows.LapRace
{
    public class RaceWindowController : UIController
    {
        private readonly RaceWindowModel _model = new();
        [SerializeField] private LapCounterController _lapCounterController;
        [SerializeField] private PositionCounterController _positionCounterController;

        public override void Init()
        {
            _lapCounterController.Init();
            _positionCounterController.Init();
            base.Init();
        }

        public override void Show()
        {
            _lapCounterController.Show();
            _positionCounterController.Show();
            base.Show();
        }

        public override void Hide()
        {
            _lapCounterController.Hide();
            _positionCounterController.Hide();
            base.Hide();
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }
    }
}
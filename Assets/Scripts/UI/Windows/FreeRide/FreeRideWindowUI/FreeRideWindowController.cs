using System.Collections;
using System.Collections.Generic;
using UI.Windows.LapRace;
using UnityEngine;

namespace UI.Windows.FreeRide
{
    public class FreeRideWindowController : RaceLayout
    {
        [Header("Own controllers")]
        [SerializeField] private ScoreController _scoreController;

        private readonly FreeRideWindowModel _model = new();

        public override void Init() =>
            _scoreController.Init();

        public override void Show()
        {
            _scoreController.Show();
            base.Show();
        }

        public override void Hide()
        {
            _scoreController.Hide();
            base.Hide();
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }
    }
}

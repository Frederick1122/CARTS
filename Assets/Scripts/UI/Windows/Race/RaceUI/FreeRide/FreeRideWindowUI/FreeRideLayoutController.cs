using System.Collections;
using System.Collections.Generic;
using Race.RaceManagers;
using UI.Windows.LapRace;
using UnityEngine;

namespace UI.Windows.FreeRide
{
    public class FreeRideLayoutController : RaceLayout
    {
        public override RaceType raceType 
        {
            get { return RaceType.FREE_RIDE; }
        }

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

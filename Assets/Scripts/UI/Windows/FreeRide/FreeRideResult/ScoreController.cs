using FreeRide;
using Race;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.FreeRide
{
    public class ScoreController : UIController<ScoreView, ScoreModel>
    {
        private readonly ScoreModel _model = new();

        public override void Init()
        {
            _view.Init(_model);
        }

        private void OnDestroy() =>
            ((FreeRideManager)FreeRideManager.Instance).OnResultUpdate -= UpdateScore;

        public override void Show()
        {
            ((FreeRideManager)FreeRideManager.Instance).OnResultUpdate += UpdateScore;
            UpdateView();
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            if (FreeRideManager.Instance != null)
                ((FreeRideManager)FreeRideManager.Instance).OnResultUpdate -= UpdateScore;
        }

        public override void UpdateView() =>
            _view.UpdateView(_model);

        protected override ScoreModel GetViewData()
        {
            return _model;
        }

        private void UpdateScore(int value)
        {
            _model.Score = value;
            UpdateView();
        }
    }
}

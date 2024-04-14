using FreeRide;
using Race;
using System.Collections;
using System.Collections.Generic;
using Race.RaceManagers;
using UnityEngine;

namespace UI.Windows.FreeRide
{
    public class ScoreController : UIController
    {
        private readonly ScoreModel _model = new();
        private FreeRideState _freeRideState;

        public override void Init()
        {
            _freeRideState = (FreeRideState)RaceManager.Instance.GetState(RaceType.FREE_RIDE);
            _freeRideState.OnResultUpdateAction += UpdateActionScore;
            _freeRideState.OnFinishAction += ResetScore;

            _view.Init(_model);
        }

        private void OnDestroy()
        {
            if (_freeRideState == null)
                return;
            
            _freeRideState.OnResultUpdateAction -= UpdateActionScore;
            _freeRideState.OnFinishAction -= ResetScore;
        }

        public override void Show()
        {
            UpdateView();
            base.Show();
        }

        public override void UpdateView() =>
            _view.UpdateView(_model);

        public void ResetScore()
        {
            _model.Score = 0;
        }
        
        protected override UIModel GetViewData()
        {
            return _model;
        }

        private void UpdateActionScore(int value)
        {
            _model.Score = value;
            UpdateView();
        }
    }
}

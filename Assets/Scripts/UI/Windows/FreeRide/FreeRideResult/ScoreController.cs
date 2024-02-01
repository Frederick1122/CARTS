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
            _freeRideState = RaceManager.Instance.GetState<FreeRideState>();
            _freeRideState.OnResultUpdateAction += UpdateActionScore;
            _freeRideState.OnStartAction += ResetScore;

            _view.Init(_model);
        }

        private void OnDestroy()
        {
            if (_freeRideState == null)
                return;
            
            _freeRideState.OnResultUpdateAction -= UpdateActionScore;
            _freeRideState.OnStartAction -= ResetScore;
        }

        public override void Show()
        {
            UpdateView();
            base.Show();
        }

        public override void UpdateView() =>
            _view.UpdateView(_model);

        protected override UIModel GetViewData()
        {
            return _model;
        }

        private void ResetScore()
        {
            _model.Score = 0;
        }
        
        private void UpdateActionScore(int value)
        {
            _model.Score = value;
            UpdateView();
        }
    }
}

using System;

namespace UI.Windows.MapSelection
{
    public class TrackController : UIController<TrackView, TrackModel>
    {
        public event Action<TrackModel> OnSelectTrackAction;

        private TrackModel _uiModel = new();

        public override void Init()
        {
            _view.OnSelectTrackAction += SelectTrack;
            base.Init();
        }

        public override void UpdateView(TrackModel uiModel)
        {
            _uiModel = uiModel;
            base.UpdateView(uiModel);
        }
        
        protected override TrackModel GetViewData()
        {
            return _uiModel;
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;
            
            _view.OnSelectTrackAction -= SelectTrack;
        }

        private void SelectTrack()
        {
            OnSelectTrackAction?.Invoke(_uiModel);
        }
    }
}
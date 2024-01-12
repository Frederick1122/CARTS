using System;
using UI.Windows.MapSelection;

namespace UI.Elements
{
    public class TrackCustomScroll : CustomScroll<TrackController, TrackView, TrackModel>
    {
        public event Action<TrackModel> OnSelectTrackAction;

        public override void AddElement(TrackModel uiModel)
        {
            var isNewController = _hidingControllers.Count == 0;
            base.AddElement(uiModel);
            
            if(isNewController)
                _activeControllers[^1].OnSelectTrackAction += SelectTrack;
        }

        private void OnDestroy()
        {
            foreach (var activeController in _activeControllers)
            {
                if (activeController == null)
                    continue;

                activeController.OnSelectTrackAction -= SelectTrack;
            }
            
            foreach (var hidingController in _hidingControllers)
            {
                if (hidingController == null)
                    continue;

                hidingController.OnSelectTrackAction -= SelectTrack;
            }
        }

        private void SelectTrack(TrackModel uiModel)
        {
            OnSelectTrackAction?.Invoke(uiModel);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LapRace
{
    public class LapRaceLayoutView : RaceLayoutView
    {
        public event Action OnNeedToPause = delegate { };

        [SerializeField] private Button _pauseButton;
        [SerializeField] private Slider _raceProgress;

        public override void Init(UIModel uiModel) =>
            _pauseButton.onClick.AddListener(Pause);

        private void OnDestroy() =>
            _pauseButton.onClick.RemoveListener(Pause);

        private void Pause() =>
            OnNeedToPause?.Invoke();

        public override void Show()
        {
            base.Show();
            _raceProgress.gameObject.SetActive(false);
        }

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);
            var castData = uiModel as LapRaceLayoutModel;

            _raceProgress.gameObject.SetActive(castData != null);

            if (castData == null)
                return;

            if (Math.Abs(_raceProgress.maxValue - castData.maxDistance) > 0.1f)
                _raceProgress.maxValue = castData.maxDistance;

            _raceProgress.value = castData.distance;
        }
    }

    public class LapRaceLayoutModel : RaceLayoutModel
    {
        public float maxDistance;
        public float distance;
    }
}

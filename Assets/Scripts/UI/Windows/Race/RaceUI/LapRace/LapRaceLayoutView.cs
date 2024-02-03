using System;using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LapRace
{
    public class LapRaceLayoutView : RaceLayoutView
    {
        public event Action OnNeedToPause = delegate { };

        [SerializeField] Button _pauseButton;

        public override void Init(UIModel uiModel) =>
            _pauseButton.onClick.AddListener(Pause);

        private void OnDestroy() =>
            _pauseButton.onClick.RemoveListener(Pause);

        public void Pause() =>
            OnNeedToPause?.Invoke();
    }

    public class LapRaceLayoutModel : RaceLayoutModel { }
}

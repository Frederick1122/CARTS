using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class RaceLayoutView : UIView
    {
        public event Action OnNeedToPause = delegate { };

        [SerializeField] private Button _pauseButton;
        
        public override void Init(UIModel uiModel) =>
            _pauseButton.onClick.AddListener(Pause);

        private void OnDestroy() =>
            _pauseButton.onClick.RemoveListener(Pause);
        
        private void Pause() =>
            OnNeedToPause?.Invoke();
    }

    public class RaceLayoutModel : UIModel
    {
        
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LapRace.LapRaceEndWindow
{
    public class LapRaceEndWindowView : UIView<LapRaceEndWindowModel>
    {
        public event Action OnGoToMainMenuAction = delegate {  };
        
        [SerializeField] private TMP_Text _currentPosition;
        [SerializeField] private TMP_Text _currentTime;
        [SerializeField] private Button _goToMainMenu;

        public override void Init(LapRaceEndWindowModel uiModel)
        {
            base.Init(uiModel);
            _goToMainMenu.onClick.AddListener(OnGoToMainMenuAction.Invoke); 
        }

        private void OnDestroy()
        {
            if (_goToMainMenu != null)
                _goToMainMenu.onClick.RemoveListener(OnGoToMainMenuAction.Invoke);
        }

        public override void UpdateView(LapRaceEndWindowModel uiModel)
        {
            _currentPosition.text = $"{uiModel.currentPosition} / {uiModel.maxPosition}";
            _currentTime.text = String.Format("{0:00}:{1:00}.{2:00}", uiModel.passTime.Minutes, uiModel.passTime.Seconds, 
                uiModel.passTime.Milliseconds / 10);
            
            base.UpdateView(uiModel);
        }
    }

    public class LapRaceEndWindowModel : UIModel
    {
        public int maxPosition;
        public int currentPosition;
        public TimeSpan passTime;
    }
}
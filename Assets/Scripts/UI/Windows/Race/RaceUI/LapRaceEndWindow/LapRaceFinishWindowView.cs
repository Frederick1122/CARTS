using System;
using TMPro;
using UI.Windows.Finish;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LapRace.LapRaceEndWindow
{
    public class LapRaceFinishWindowView : FinishWindowView
    {
        [SerializeField] private TMP_Text _currentPosition;
        [SerializeField] private TMP_Text _currentTime;
        [SerializeField] private Button _goToMainMenu;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            _goToMainMenu.onClick.AddListener(GoToMainMenu); 
        }

        private void OnDestroy()
        {
            if (_goToMainMenu != null)
                _goToMainMenu.onClick.RemoveListener(GoToMainMenu);
        }

        public override void UpdateView(UIModel uiModel)
        {
            var castModel = (LapRaceEndWindowModel) uiModel;
            
            _currentPosition.text = $"{castModel.currentPosition} / {castModel.maxPosition}";
            _currentTime.text = String.Format("{0:00}:{1:00}.{2:00}", castModel.passTime.Minutes, castModel.passTime.Seconds, 
                castModel.passTime.Milliseconds / 10);
            
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
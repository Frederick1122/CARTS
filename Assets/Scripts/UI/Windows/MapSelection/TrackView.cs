using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.MapSelection
{
    public class TrackView : UIView<TrackModel>
    {
        public event Action OnSelectTrackAction;

        [SerializeField] private Button _selectButton;
        [SerializeField] private TMP_Text _trackNameText;
        [SerializeField] private TMP_Text _selectedTrackText;
        
        public override void Init(TrackModel uiModel)
        {
            base.Init(uiModel);
            _selectButton.onClick.AddListener(SelectTrack);
        }

        public override void UpdateView(TrackModel uiModel)
        {
            base.UpdateView(uiModel);
            _trackNameText.text = TrackLibrary.Instance.GetConfig(uiModel.configKey).configName;
            _selectedTrackText.text = uiModel.isSelectedCar ? "x" : "";
        }

        private void OnDestroy()
        {
            _selectButton?.onClick.RemoveListener(SelectTrack);
        }
        
        private void SelectTrack()
        {
            OnSelectTrackAction?.Invoke();;
        }
    }

    public class TrackModel : UIModel
    {
        public bool isSelectedCar = false;
        public string configKey = "";
        
        public TrackModel() {}
        
        public TrackModel(string configKey, bool isSelectedCar)
        {
            this.configKey = configKey;
            this.isSelectedCar = isSelectedCar;
        }
    }
}
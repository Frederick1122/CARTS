using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.MapSelection
{
    public class CustomToggleView : UIView<CustomToggleModel>
    {
        public event Action OnSelectAction;

        [SerializeField] private Button _selectButton;
        [SerializeField] private TMP_Text _trackNameText;
        [SerializeField] private TMP_Text _selectedTrackText;

        public override void Init(CustomToggleModel uiModel)
        {
            base.Init(uiModel);
            _selectButton.onClick.AddListener(Select);
        }

        public override void UpdateView(CustomToggleModel uiModel)
        {
            base.UpdateView(uiModel);
            _trackNameText.text = uiModel.text;
            _selectedTrackText.text = uiModel.isSelected ? "x" : "";
        }

        private void OnDestroy() =>
            _selectButton?.onClick.RemoveListener(Select);

        private void Select() =>
            OnSelectAction?.Invoke();
    }

    public class CustomToggleModel : UIModel
    {
        public bool isSelected = false;
        public string text = "";
        public string key = "";

        public CustomToggleModel() { }

        public CustomToggleModel(string text, bool isSelected)
        {
            this.text = text;
            this.isSelected = isSelected;
        }

        public CustomToggleModel(string text, string key, bool isSelected)
        {
            this.text = text;
            this.key = key;
            this.isSelected = isSelected;
        }
    }
}
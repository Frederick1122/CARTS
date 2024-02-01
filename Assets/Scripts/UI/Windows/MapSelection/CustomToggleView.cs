using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.MapSelection
{
    public class CustomToggleView : UIView
    {
        public event Action OnSelectAction;

        [SerializeField] private Button _selectButton;
        [SerializeField] private TMP_Text _trackNameText;
        [SerializeField] private TMP_Text _selectedTrackText;

        public override void Init(UIModel model)
        {
            base.Init(model);
            _selectButton.onClick.AddListener(Select);
        }

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);

            var castModel = (CustomToggleModel) uiModel;
            
            _trackNameText.text = castModel.text;
            _selectedTrackText.text = castModel.isSelected ? "x" : "";
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
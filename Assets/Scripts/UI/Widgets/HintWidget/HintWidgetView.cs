using TMPro;
using UnityEngine;

namespace UI.Widgets.HintWidget
{
    public class HintWidgetView : UIView
    {
        [SerializeField] private GameObject _hint;
        [SerializeField] private TMP_Text _hintText;
        
        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            UpdateView(uiModel);
        }

        public override void UpdateView(UIModel uiModel)
        {
            var castModel = (HintWidgetModel) uiModel;

            _hintText.text = castModel.text;
            _hint.SetActive(castModel.text != "");
        }
    }

    public class HintWidgetModel : UIModel
    {
        public string text;
    }
}
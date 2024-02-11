using TMPro;
using UnityEngine;

namespace UI.Widgets.CurrencyWidget
{
    public class CurrencyWidgetView : UIView
    {
        [SerializeField] private TMP_Text _regularCurrency;
        [SerializeField] private TMP_Text _premiumCurrency;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            UpdateView(uiModel);
        }

        public override void UpdateView(UIModel uiModel)
        {
            var castModel = (CurrencyWidgetModel) uiModel;

            _regularCurrency.text = castModel.regularCurrency.ToString();
            _premiumCurrency.text = castModel.premiumCurrency.ToString();
        }
    }

    public class CurrencyWidgetModel : UIModel
    {
        public int regularCurrency;
        public int premiumCurrency;
    }
}
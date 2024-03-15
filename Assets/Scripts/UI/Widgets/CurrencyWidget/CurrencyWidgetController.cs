using System;
using Managers;

namespace UI.Widgets.CurrencyWidget
{
    public class CurrencyWidgetController : UIController
    {
        private CurrencyWidgetModel _model = new CurrencyWidgetModel();

        public override void Init()
        {
            _model.regularCurrency = PlayerManager.Instance.GetCurrency(CurrencyType.Soft);
            _model.premiumCurrency = PlayerManager.Instance.GetCurrency(CurrencyType.Hard);

            PlayerManager.Instance.OnCurrencyChange += ChangeCurrency;
            base.Init();
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.OnCurrencyChange -= ChangeCurrency;
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }
        
        private void ChangeCurrency(CurrencyType currencyType, int newValue)
        {
            switch (currencyType)
            {
                case CurrencyType.Soft:
                    _model.regularCurrency = newValue;
                    break;
                case CurrencyType.Hard:
                    _model.premiumCurrency = newValue;
                    break;
            }   
            
            UpdateView();
        }
    }
}
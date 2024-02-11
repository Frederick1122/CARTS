using System;
using Managers;

namespace UI.Widgets.CurrencyWidget
{
    public class CurrencyWidgetController : UIController
    {
        private CurrencyWidgetModel _model = new CurrencyWidgetModel();

        public override void Init()
        {
            _model.regularCurrency = PlayerManager.Instance.GetCurrency(CurrencyType.Regular);
            _model.premiumCurrency = PlayerManager.Instance.GetCurrency(CurrencyType.Premium);

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
                case CurrencyType.Regular:
                    _model.regularCurrency = newValue;
                    break;
                case CurrencyType.Premium:
                    _model.premiumCurrency = newValue;
                    break;
            }   
            
            UpdateView();
        }
    }
}
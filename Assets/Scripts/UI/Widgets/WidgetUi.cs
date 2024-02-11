using UI.Widgets.CurrencyWidget;
using UnityEngine;

namespace UI.Widgets
{
    public class WidgetUi : WindowManager
    {
        [Header("Controllers")]
        [SerializeField] private CurrencyWidgetController _currencyWidgetController;
        
        protected override void AddControllers()
        {
            _controllers.Add(_currencyWidgetController.GetType(), _currencyWidgetController);
        }
    }
}
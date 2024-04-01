using System;

namespace UI.Elements
{
    public class SliderController : UIController
    {
        private SliderView _castView;
        private Func<float> _getStartData;
        private Action<float> _setupNewData;
        
        public void Setup(SliderView view, Func<float> getStartData, Action<float> setupNewData)
        {
            _castView = view;
            _view = view;
            _getStartData = getStartData;
            _setupNewData = setupNewData;
            
            Init();
            _castView.OnValueChanged += _setupNewData;
        }

        private void OnDestroy()
        {
            if (_castView == null)
                return;
            
            _castView.OnValueChanged -= _setupNewData;
        }

        public override void Show()
        {
            base.Show();
            
            _castView.UpdateView(GetViewData());
        }

        protected override UIModel GetViewData()
        {
            var model = new SliderModel()
            {
                value = _getStartData()
            };

            return model;
        }
    }
}
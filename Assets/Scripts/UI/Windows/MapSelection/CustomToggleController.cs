using System;

namespace UI.Windows.MapSelection
{
    public class CustomToggleController : UIController<CustomToggleView, CustomToggleModel>
    {
        public event Action<CustomToggleModel> OnSelectAction;

        private CustomToggleModel _uiModel = new();

        public override void Init()
        {
            _view.OnSelectAction += Select;
            base.Init();
        }

        public override void UpdateView(CustomToggleModel uiModel)
        {
            _uiModel = uiModel;
            base.UpdateView(uiModel);
        }

        public void Select()
        {
            _uiModel.isSelected = true;
            OnSelectAction?.Invoke(_uiModel);
            UpdateView(_uiModel);
        }

        public void Unselect()
        {
            _uiModel.isSelected = false;
            UpdateView(_uiModel);
        }

        protected override CustomToggleModel GetViewData()
        {
            return _uiModel;
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            _view.OnSelectAction -= Select;
        }
    }
}
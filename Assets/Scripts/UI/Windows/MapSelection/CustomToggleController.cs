using System;

namespace UI.Windows.MapSelection
{
    public class CustomToggleController : UIController
    {
        public event Action<CustomToggleModel> OnSelectAction;

        private CustomToggleModel _uiModel = new();

        public override void Init()
        {
            GetView<CustomToggleView>().OnSelectAction += Select;
            base.Init();
        }

        public override void UpdateView(UIModel uiModel)
        {
            _uiModel = (CustomToggleModel)uiModel;
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

        protected override UIModel GetViewData()
        {
            return _uiModel;
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;

            GetView<CustomToggleView>().OnSelectAction -= Select;
        }
    }
}
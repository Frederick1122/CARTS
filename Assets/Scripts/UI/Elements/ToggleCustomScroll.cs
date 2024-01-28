using System;
using UI.Windows.MapSelection;

namespace UI.Elements
{
    public class ToggleCustomScroll : CustomScroll<CustomToggleController, CustomToggleView, CustomToggleModel>
    {
        public event Action<CustomToggleModel> OnSelectAction;

        public override void AddElement(CustomToggleModel uiModel)
        {
            var isNewController = _hidingControllers.Count == 0;
            base.AddElement(uiModel);

            if (isNewController)
                _activeControllers[^1].OnSelectAction += Select;
        }

        private void OnDestroy()
        {
            foreach (var activeController in _activeControllers)
            {
                if (activeController == null)
                    continue;

                activeController.OnSelectAction -= Select;
            }

            foreach (var hidingController in _hidingControllers)
            {
                if (hidingController == null)
                    continue;

                hidingController.OnSelectAction -= Select;
            }
        }

        private void Select(CustomToggleModel uiModel) =>
            OnSelectAction?.Invoke(uiModel);
    }
}
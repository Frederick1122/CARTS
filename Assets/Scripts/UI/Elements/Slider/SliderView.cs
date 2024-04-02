using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class SliderView : UIView
    {
        public event Action<float> OnValueChanged = delegate(float f) {  };
        
        [SerializeField] private Slider _slider;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            
            _slider.onValueChanged.AddListener(delegate (float value) { OnValueChanged?.Invoke(value); });
        }

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);
            var castModel = (SliderModel) uiModel;

            _slider.value = castModel.value;
        }

        public override void Terminate()
        {
            _slider?.onValueChanged.RemoveAllListeners();
            base.Terminate();
        }
    }

    public class SliderModel : UIModel
    {
        public float value;
    }
}
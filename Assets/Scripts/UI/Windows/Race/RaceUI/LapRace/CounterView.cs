using TMPro;
using UnityEngine;

namespace UI.Windows.LapRace
{
    public class CounterView : UIView
    {
        [SerializeField] private TMP_Text _maxCount;
        [SerializeField] private TMP_Text _count;

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);
            
            var castModel = (CounterModel) uiModel;
            
            _maxCount.text = castModel.maxCount.ToString();
            _count.text = castModel.count.ToString();
        }
    }

    public class CounterModel : UIModel
    {
        public int maxCount;
        public int count;
    }
}
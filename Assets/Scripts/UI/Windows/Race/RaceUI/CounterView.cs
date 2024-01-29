using TMPro;
using UnityEngine;

namespace UI.Windows.LapRace
{
    public class CounterView : UIView<CounterModel>
    {
        [SerializeField] private TMP_Text _maxCount;
        [SerializeField] private TMP_Text _count;

        public override void UpdateView(CounterModel uiModel)
        {
            base.UpdateView(uiModel);

            _maxCount.text = uiModel.maxCount.ToString();
            _count.text = uiModel.count.ToString();
        }
    }

    public class CounterModel : UIModel
    {
        public int maxCount;
        public int count;
    }
}
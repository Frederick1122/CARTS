using TMPro;
using UnityEngine;

namespace UI.Windows.FreeRide
{
    public class ScoreView : UIView
    {
        [SerializeField] private TMP_Text _scoreText;

        public override void Init(UIModel model)
        {
            UpdateView(model);
        }

        public override void UpdateView(UIModel model)
        {
            var castModel = (ScoreModel) model;

            UpdateScore(castModel.Score);
        }

        private void UpdateScore(int value)
        {
            var text = $"Score: {value}";
            _scoreText.text = text;
        }
    }

    public class ScoreModel : UIModel
    {
        public int Score { get; set; } = 0;
    }
}



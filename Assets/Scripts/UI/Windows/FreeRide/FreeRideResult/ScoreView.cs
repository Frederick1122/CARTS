using TMPro;
using UnityEngine;

namespace UI.Windows.FreeRide
{
    public class ScoreView : UIView<ScoreModel>
    {
        [SerializeField] private TMP_Text _scoreText;

        public override void Init(ScoreModel uiModel) =>
            UpdateScore(uiModel.Score);

        public override void UpdateView(ScoreModel uiModel) =>
            UpdateScore(uiModel.Score);

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



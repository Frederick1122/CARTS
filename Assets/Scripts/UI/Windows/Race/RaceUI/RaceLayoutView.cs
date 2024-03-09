using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class RaceLayoutView : UIView
    {
        [SerializeField] private TMP_Text _startDelayText;

        private CancellationTokenSource _startDelayCts;

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);
            var castModel = (RaceLayoutModel) uiModel;

            if (castModel.delay == 0)
            {
                _startDelayText.gameObject.SetActive(false);
                return;
            }

            _startDelayCts?.Cancel();
            _startDelayCts = new CancellationTokenSource();

            StartDelayTask(_startDelayCts.Token, castModel.delay).Forget();
        }
        
        private async UniTaskVoid StartDelayTask(CancellationToken token, int delay)
        {
            _startDelayText.gameObject.SetActive(true);

            var i = delay;

            while (i > 0)
            {
                _startDelayText.text = i.ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                i--;
            }

            _startDelayText.text = "GO!";

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

            _startDelayText.gameObject.SetActive(false);
        }
    }

    public class RaceLayoutModel : UIModel
    {
        public RaceLayoutModel(int delay)
        {
            this.delay = delay;
        }
        
        public RaceLayoutModel()
        {
            
        }
        
        public int delay = 0;
    }
}
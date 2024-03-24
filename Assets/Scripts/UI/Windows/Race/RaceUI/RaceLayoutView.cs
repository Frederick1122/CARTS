using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class RaceLayoutView : UIView
    {
        [SerializeField] protected TMP_Text _startDelayText;

        private int _delay = 0;
        private CancellationTokenSource _startDelayCts;

        public override void Show()
        {
            base.Show();
            
            if (_delay > 0) 
                StartDelay();
            else 
                _startDelayText.gameObject.SetActive(false);
        }

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);
            var castModel = (RaceLayoutModel) uiModel;

            if (castModel.delay == 0)
            {
                _startDelayText.gameObject.SetActive(false);
                return;
            }

            _delay = castModel.delay;
            StartDelay();
        }

        private void StartDelay()
        {
            _startDelayCts?.Cancel();
            _startDelayCts = new CancellationTokenSource();

            StartDelayTask(_startDelayCts.Token).Forget();
        }
        
        private async UniTaskVoid StartDelayTask(CancellationToken token)
        {
            _startDelayText.gameObject.SetActive(true);
            
            while (_delay > 0)
            {
                _startDelayText.text = _delay.ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                _delay--;
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
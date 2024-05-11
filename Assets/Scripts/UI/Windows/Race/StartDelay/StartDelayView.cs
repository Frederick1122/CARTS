﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Knot.Localization;
using Managers;
using TMPro;
using UnityEngine;

namespace UI.Windows.Race.StartDelay
{
    public class StartDelayView : UIView
    {
        private const string UID_GO = "GO";
        private const string PRESTART_SOUND = "SFX/Countdown/PreStart";
        private const string START_SOUND = "SFX/Countdown/Start";
        
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

        public override void Hide()
        {
            base.Hide();
            _startDelayCts?.Cancel();
        }

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);
            var castModel = (StartDelayModel) uiModel;

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
                SoundManager.Instance.PlayOneShot(PRESTART_SOUND);
                _startDelayText.text = _delay.ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                _delay--;
            }

            SoundManager.Instance.PlayOneShot(START_SOUND);
            _startDelayText.text = $"{KnotLocalization.GetText(UID_GO)}!";

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

            _startDelayText.gameObject.SetActive(false);
        }
    }

    public class StartDelayModel : UIModel
    {
        public int delay;
    }
}
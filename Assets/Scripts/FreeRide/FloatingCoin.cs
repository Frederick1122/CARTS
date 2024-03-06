using Cars.Controllers;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace FreeRide.Map
{
    public class FloatingCoin : MonoBehaviour
    {
        [SerializeField] private int _price = 1;
        [SerializeField] private CurrencyType _currencyType;

        [Header("Anim")]
        [SerializeField] private float _animSpeed = 2f;

        private PlayerManager _playerManager;

        private CancellationTokenSource _cancellationTokenSource;

        private void Awake() => _playerManager = PlayerManager.Instance;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CarController _))
            {
                _cancellationTokenSource = new CancellationTokenSource();
                Collect(_cancellationTokenSource.Token).Forget();
            }
            else if (other.transform.parent.TryGetComponent(out CarController _))
            {
                _cancellationTokenSource = new CancellationTokenSource();
                Collect(_cancellationTokenSource.Token).Forget();
            }
        }

        private void OnDisable() => DOTween.Kill(transform); 

        private void OnDestroy() => _cancellationTokenSource?.Cancel();

        public void Spawn()
        {
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            transform.DOMoveY(transform.position.y - 0.4f, _animSpeed)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            transform.DORotate(new Vector3(0, 360, 0), _animSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }

        private async UniTaskVoid Collect(CancellationToken cancellationToken)
        {
            _playerManager.IncreaseCurrency(_currencyType, _price);
            var time = _animSpeed * 0.1f;

            DOTween.Kill(transform);
            transform.DORotate(new Vector3(0, 360, 0), time, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            transform.DOScale(Vector3.zero, time);

            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);
            gameObject.SetActive(false);
        }
    }
}

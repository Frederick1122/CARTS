using Cars.Controllers;
using CustomSnapTool;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

namespace FreeRide.Map
{
    public class MapPiece : MonoBehaviour
    {
        public event Action<MapPiece> OnReach = delegate { };
        public event Action<MapPiece> OnGoToDestroy = delegate { };

        [field: Header("Points")]
        [field: SerializeField] public CustomSnapPoint StartPoint { get; private set; }
        [field: SerializeField] public CustomSnapPoint EndPoint { get; private set; }

        [Header("Animation")]
        [SerializeField] private float _shakeStrength = 0.5f;

        private CancellationTokenSource _destroyCancellationTokenSource = new();
        private CancellationTokenSource _shakeCancellationTokenSource = new();

        private float _timeToDestroySec = 5f;
        private Vector3 _spawnPosition;

        public void Init(float destroyTime)
        {
            _timeToDestroySec = destroyTime;
            _spawnPosition = transform.localPosition;
        }

        private void OnDestroy()
        {
            _destroyCancellationTokenSource.Cancel();
            _shakeCancellationTokenSource.Cancel();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out CarController _) && 
                !other.transform.parent.TryGetComponent(out CarController _))
                return;

            OnReach?.Invoke(this);
            _destroyCancellationTokenSource = new CancellationTokenSource();
            _shakeCancellationTokenSource = new CancellationTokenSource();

            var shakeDelay = _timeToDestroySec * 0.3f;
            MakeShake(_timeToDestroySec - shakeDelay, shakeDelay, _shakeCancellationTokenSource.Token).Forget();
            DestroyTask(_destroyCancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid MakeShake(float shakeTime, float shakeDelay, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(shakeDelay), cancellationToken: cancellationToken);
            transform.DOShakePosition(shakeTime, strength: _shakeStrength * Vector3.left, fadeOut: false);
        }

        private async UniTaskVoid DestroyTask(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timeToDestroySec), cancellationToken: cancellationToken);
            var movePos = transform.position;
            movePos.y -= 10;
            transform.DOMove(movePos, _timeToDestroySec / 2);
            await UniTask.Delay(TimeSpan.FromSeconds(_timeToDestroySec / 2), cancellationToken: cancellationToken);
            gameObject.SetActive(false);
            OnGoToDestroy?.Invoke(this);
        }

        public void ResetPiece()
        {
            transform.localPosition = _spawnPosition;
            gameObject.SetActive(true);
        }
    }
}

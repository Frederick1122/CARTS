using Cars.Controllers;
using Cars.Tools;
using CustomSnapTool;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

namespace FreeRide.Map
{
    [RequireComponent(typeof(Collider))]
    public class MapPiece : MonoBehaviour
    {
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
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void Spawn(float destroyTime)
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
            FreeRideCarHelper player;
            if (!other.TryGetComponent(out player) && 
                !other.transform.parent.TryGetComponent(out player))
                return;

            player.SetMinimalY(_collider.bounds.min.y);
            player.ReachPiece();

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

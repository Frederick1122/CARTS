using Cars.Controllers;
using CustomSnapTool;
using Cysharp.Threading.Tasks;
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

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private float _timeToDestroySec = 5f;

        public void Init(float destroyTime) =>
            _timeToDestroySec = destroyTime;

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CarController _))
            {
                OnReach?.Invoke(this);
                DestroyTask(_cancellationTokenSource.Token).Forget();
            }
            else if (other.transform.parent.TryGetComponent(out CarController _))
            {
                OnReach?.Invoke(this);
                DestroyTask(_cancellationTokenSource.Token).Forget();
            }
        }

        private async UniTaskVoid DestroyTask(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timeToDestroySec), cancellationToken: cancellationToken);
            gameObject.SetActive(false);

            OnGoToDestroy?.Invoke(this);
        }

        public void ResetPiece() =>
            gameObject.SetActive(true);
    }
}

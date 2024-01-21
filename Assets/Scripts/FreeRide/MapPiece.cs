using Cars.Controllers;
using CustomSnapTool;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace FreeRide
{
    public class MapPiece : MonoBehaviour, IPoolObject
    {
        public event Action<MapPiece> OnReach = delegate { };
        public event Action<IPoolObject> OnObjectNeededToDeactivate = delegate { };

        [SerializeField] private float _timeToDestroySec = 5f;

        [Header("Points")]
        [SerializeField] private CustomSnapPoint _startPoint;
        [SerializeField] private CustomSnapPoint _endPoint;

        private CancellationToken _brakeToken = new();

        public CustomSnapPoint GetPointForConnect()
        {
            return _endPoint;
        }

        public void ConnectToPoint(CustomSnapPoint to)
        {
            int rndScale = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
            transform.localScale = new Vector3(1, 1, rndScale);
            transform.position = to.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out CarController _))
            {
                OnReach?.Invoke(this);
                DestroyTask(_brakeToken).Forget();
            }
        }

        private async UniTaskVoid DestroyTask(CancellationToken cancToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timeToDestroySec), cancellationToken: cancToken);

            OnObjectNeededToDeactivate?.Invoke(this);
            Destroy(gameObject);
        }

        public void ResetBeforeBackToPool() =>
            transform.localScale = new Vector3(1, 1, 1);
    }
}

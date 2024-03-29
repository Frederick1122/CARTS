using Cars.Controllers;
using CustomSnapTool;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace FreeRide.Map
{
    public class MapPiecesHolder : MonoBehaviour, IPoolObject
    {
        private const float LASTS_TIME = 2f;

        public event Action<IPoolObject> OnObjectNeededToDeactivate = delegate { };

        public int MaxCoinsCount => _coinsPlace.Length;

        [Header("Pieces")]
        [SerializeField] private MapPiece _startPiece;
        [SerializeField] private MapPiece _endPiece;
        [SerializeField] private List<MapPiece> _mapPieces = new();

        [Header("Coins")]
        [SerializeField] private FloatingCoin[] _coinsPlace = new FloatingCoin[1];

        private int[] _coinsOrder;
        private readonly System.Random _rnd = new();

        private int _lastsCount = 0;
        private CancellationTokenSource _cancellationTokenSource = new();

        private void Awake()
        {
            _coinsOrder = Enumerable.Range(0, _coinsPlace.Length).ToArray();

            foreach (MapPiece piece in _mapPieces)
                piece.OnGoToDestroy += PieceDestroy;
        }

        private void OnDestroy()
        {
            foreach (MapPiece piece in _mapPieces)
                piece.OnGoToDestroy -= PieceDestroy;

            _cancellationTokenSource.Cancel();
        }

        public void Spawn(float timeToDestroySec, int coinsCount)
        {
            var count = Mathf.Clamp(coinsCount, 0, _coinsPlace.Length);
            if (count > 0)
            {
                ShuffleOrder();
                for (int i = 0; i < count; i++)
                    _coinsPlace[_coinsOrder[i]].Spawn();
            }

            foreach (MapPiece piece in _mapPieces)
                piece.Spawn(timeToDestroySec);
        }

        public CustomSnapPoint GetConnector()
        {
            return _endPiece.EndPoint;
        }

        public void ConnectToPoint(CustomSnapPoint to)
        {
            int rndScale = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
            transform.localScale = new Vector3(rndScale, 1, 1);
            transform.position = to.transform.position -
                (_startPiece.StartPoint.transform.position - transform.position);
        }

        public void ResetBeforeBackToPool()
        {
            gameObject.SetActive(false);
            transform.localScale = new Vector3(1, 1, 1);

            _lastsCount = 0;
            foreach (var piece in _mapPieces)
                piece.ResetPiece();

            foreach (var coin in _coinsPlace)
                coin.gameObject.SetActive(false);
        }

        private void PieceDestroy(MapPiece _)
        {
            _lastsCount++;
            if (_lastsCount >= _mapPieces.Count)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                DestroyTask(_cancellationTokenSource.Token).Forget();
            }
        }

        private async UniTaskVoid DestroyTask(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(LASTS_TIME), cancellationToken: cancellationToken);
            OnObjectNeededToDeactivate?.Invoke(this);
        }

        private void ShuffleOrder()
        {
            var count = _coinsOrder.Length;

            for (int i = count - 1; i >= 1; i--)
            {
                int j = _rnd.Next(i + 1);
                int tmp = _coinsOrder[j];
                _coinsOrder[j] = _coinsOrder[i];
                _coinsOrder[i] = _coinsOrder[tmp];
            }
        }
    }
}

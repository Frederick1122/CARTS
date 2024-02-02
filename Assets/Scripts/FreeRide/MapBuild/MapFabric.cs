using Base.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FreeRide.Map
{
    public class MapFabric : MonoBehaviour
    {
        public event Action<int> OnResultUpdate;
        public event Action OnFall = delegate { };

        public int Result
        {
            get => _result;
            private set
            {
                _result = value;
                OnResultUpdate?.Invoke(_result);
            }
        }
        
        [SerializeField] private int _startCountOfPeieces = 2;
        [SerializeField] private MapPiecesHolder _startPiece;
        [SerializeField] private MapPiecesHolder[] _piecePrefabs = new MapPiecesHolder[2];

        private int _result = 0;

        private PoolMono<MapPiecesHolder> _piecePool;
        private MapPiecesHolder _lastPiece = null;
        private static readonly int _poolCount = 6;

        private readonly List<MapPiecesHolder> _spawned = new();

        public void Init()
        {
            _piecePool = new PoolMono<MapPiecesHolder>(_piecePrefabs.ToList(), _poolCount);
            _lastPiece = _startPiece;
            _startPiece.gameObject.SetActive(true);

            for (int i = 0; i < _startCountOfPeieces; i++)
                SpawnPiece();
        }

        public void SpawnPiece()
        {
            var piece = _piecePool.GetObject();

            if (_lastPiece != null)
                piece.ConnectToPoint(_lastPiece.GetConnector());

            piece.OnPieceReach += UpdateResult;
            piece.OnFinish += WhenFinishPiece;
            piece.OnFall += StopFabic;
            _lastPiece = piece;

            _spawned.Add(piece);
        }

        private void UpdateResult() =>
            Result++;

        private void WhenFinishPiece(MapPiecesHolder piece)
        {
            Result++;
            piece.OnPieceReach -= UpdateResult;
            piece.OnFinish -= WhenFinishPiece;
            piece.OnFall -= StopFabic;
            _spawned.Remove(piece);

            SpawnPiece();
        }

        private void StopFabic()
        {
            foreach (var piece in _spawned)
            {
                piece.OnFinish -= WhenFinishPiece;
                piece.OnFall -= StopFabic;
            }

            OnFall?.Invoke();
        }
    }
}

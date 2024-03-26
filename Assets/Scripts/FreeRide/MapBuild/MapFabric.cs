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
        
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _mapSpawnPoint;
        
        private int _result = 0;

        private MapFabricData _currentData;

        private PoolMono<MapPiecesHolder> _piecePool;
        private MapPiecesHolder _lastPiece = null;
        private static readonly int _poolCount = 6;

        private readonly List<MapPiecesHolder> _spawned = new();

        public void Init(MapFabricData mapFabricData)
        {
            _currentData = mapFabricData;
            
            _piecePool = new PoolMono<MapPiecesHolder>(_currentData.piecePrefabs.ToList(), _poolCount);

            var startPiece = Instantiate(_currentData.startPiece, _mapSpawnPoint);

            InitPiece(startPiece);
            startPiece.gameObject.SetActive(true);

            for (int i = 0; i < _currentData.startCountOfPieces; i++)
                SpawnPiece();
        }

        public Transform GetPlayerSpawnPoint() => _playerSpawnPoint;

        private void SpawnPiece()
        {
            var piece = _piecePool.GetObject();

            if (_lastPiece != null)
                piece.ConnectToPoint(_lastPiece.GetConnector());

            InitPiece(piece); 
        }

        private void InitPiece(MapPiecesHolder piece)
        {
            piece.OnPieceReach += UpdateResult;
            piece.OnFinish += WhenFinishPiece;
            piece.OnFall += StopFabric;

            _lastPiece = piece;

            var coinsCount = UnityEngine.Random.Range(0, piece.MaxCoinsCount);
            piece.Spawn(_currentData.destroyTime.Evaluate(_result), coinsCount);

            _spawned.Add(piece);
        }

        private void UpdateResult() =>
            Result++;

        private void WhenFinishPiece(MapPiecesHolder piece)
        {
            Result++;
            piece.OnPieceReach -= UpdateResult;
            piece.OnFinish -= WhenFinishPiece;
            piece.OnFall -= StopFabric;
            _spawned.Remove(piece);

            SpawnPiece();
        }

        private void StopFabric()
        {
            foreach (var piece in _spawned)
            {
                piece.OnFinish -= WhenFinishPiece;
                piece.OnFall -= StopFabric;
            }

            OnFall?.Invoke();
        }
    }

    [Serializable]
    public class MapFabricData
    {
        public AnimationCurve destroyTime;
        public MapPiecesHolder startPiece;
        public MapPiecesHolder[] piecePrefabs = new MapPiecesHolder[2];
        public int startCountOfPieces = 2;
    }
}

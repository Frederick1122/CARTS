using Base.Pool;
using Race.RaceManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FreeRide.Map
{
    public class MapFabric : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _mapSpawnPoint;

        private MapFabricData _currentData;

        private PoolMono<MapPiecesHolder> _piecePool;
        private MapPiecesHolder _lastPiece = null;
        private static readonly int _poolCount = 12;

        private int _score => _state.GetResult();
        private FreeRideState _state;

        private readonly List<IPoolObject> _spawned = new();

        public void Init(MapFabricData mapFabricData, FreeRideState state)
        {
            _currentData = mapFabricData;
            _state = state;
            
            _piecePool = new PoolMono<MapPiecesHolder>(_currentData.piecePrefabs.ToList(), _poolCount);

            var startPiece = Instantiate(_currentData.startPiece, _mapSpawnPoint);

            InitPiece(startPiece);
            startPiece.gameObject.SetActive(true);

            for (int i = 0; i < _currentData.startCountOfPieces; i++)
                SpawnPiece(true);
        }

        public Transform GetPlayerSpawnPoint() => _playerSpawnPoint;

        public void SpawnPiece(bool forceSpawn = false)
        {
            if(!forceSpawn)
            {
                if (_spawned.Count >= _currentData.startCountOfPieces * 2)
                    return;
            }

            for (int i = 0; i < 2; i++)
            {
                var piece = _piecePool.GetObject();
                if (_lastPiece != null)
                    piece.ConnectToPoint(_lastPiece.GetConnector());
                InitPiece(piece);
            }
        }

        private void InitPiece(MapPiecesHolder piece)
        {
            _lastPiece = piece;

            var coinsCount = UnityEngine.Random.Range(0, piece.MaxCoinsCount);
            piece.Spawn(_currentData.destroyTime.Evaluate(_score), coinsCount);
            piece.OnObjectNeededToDeactivate += DestroyPiece;

            _spawned.Add(piece);
        }

        private void DestroyPiece(IPoolObject piece)
        {
            piece.OnObjectNeededToDeactivate -= DestroyPiece;
            _spawned.Remove(piece);
        }

        public void StopFabric()
        {
            foreach (var piece in _spawned)
                piece.OnObjectNeededToDeactivate -= DestroyPiece;
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

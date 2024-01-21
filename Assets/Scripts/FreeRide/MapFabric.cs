using Base.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FreeRide
{
    public class MapFabric : MonoBehaviour
    {
        public int Result { get; private set; } = 0; 

        [SerializeField] private int _startCountOfPeieces = 2;
        [SerializeField] private MapPiece[] _piecePrefabs = new MapPiece[2];

        private PoolMono<MapPiece> _piecePool;
        private MapPiece _lastPiece = null;
        private static int _poolCount = 10;

        public void Init()
        {
            _piecePool = new PoolMono<MapPiece>(_piecePrefabs.ToList(), _poolCount);
            for (int i = 0; i < _poolCount; i++)
                SpawnPiece();
        }

        public void SpawnPiece()
        {
            var piece = _piecePool.GetObject();
            if (_lastPiece != null)
                piece.ConnectToPoint(_lastPiece.GetPointForConnect());
            piece.OnReach += WhenReachPiece;
            _lastPiece = piece;
        }

        private void WhenReachPiece(MapPiece piece)
        {
            Result++;
            piece.OnReach -= WhenReachPiece;
            SpawnPiece();
        }
    }
}

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
        [SerializeField] private MapPiecesHolder _startPiece;
        [SerializeField] private MapPiecesHolder[] _piecePrefabs = new MapPiecesHolder[2];

        private PoolMono<MapPiecesHolder> _piecePool;
        private MapPiecesHolder _lastPiece = null;
        private static int _poolCount = 6;

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

            piece.OnFinish += WhenReachPiece;
            _lastPiece = piece;
        }

        private void WhenReachPiece(MapPiecesHolder piece)
        {
            Result++;
            piece.OnFinish -= WhenReachPiece;
            SpawnPiece();
        }
    }
}

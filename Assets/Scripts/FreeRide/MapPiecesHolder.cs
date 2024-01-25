using Cars.Controllers;
using CustomSnapTool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FreeRide
{

    public class MapPiecesHolder : MonoBehaviour, IPoolObject
    {
        public event Action OnFall = delegate { };
        public event Action<MapPiece> OnReach = delegate { };
        public event Action<MapPiecesHolder> OnFinish = delegate { };
        public event Action<IPoolObject> OnObjectNeededToDeactivate = delegate { };

        [SerializeField] private float _timeToDestroySec = 5f;

        [Header("Pieces")]
        [SerializeField] private MapPiece _startPiece;
        [SerializeField] private MapPiece _endPiece;
        [SerializeField] private List<MapPiece> _mapPieces = new();

        private int _lastsCount = 0;
        private int _reachCount = 0;

        private void Awake()
        {
            foreach (MapPiece piece in _mapPieces)
            {
                piece.Init(_timeToDestroySec);
                piece.OnReach += PieceReach;
                piece.OnGoToDestroy += PieceDestroy;
            }

            ResetBeforeBackToPool();
        }

        private void OnDestroy()
        {
            foreach (MapPiece piece in _mapPieces)
            {
                piece.OnReach -= PieceReach;
                piece.OnGoToDestroy -= PieceDestroy;
            }
        }

        public void Spawn() =>
            gameObject.SetActive(true);

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

        private void PieceDestroy(MapPiece _)
        {
            _lastsCount++;
            if (_lastsCount >= _mapPieces.Count)
                OnObjectNeededToDeactivate?.Invoke(this);
        }

        private void PieceReach(MapPiece piece)
        {
            _reachCount++;
            OnReach?.Invoke(piece);
            if (_reachCount >= _mapPieces.Count)
                OnFinish?.Invoke(this);
        }

        public void ResetBeforeBackToPool()
        {
            gameObject.SetActive(false);
            transform.localScale = new Vector3(1, 1, 1);

            _lastsCount = 0;
            _reachCount = 0;
            foreach (var piece in _mapPieces)
                piece.ResetPiece();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CarController _) || other.transform.parent.TryGetComponent(out CarController _))
                OnFall?.Invoke();
        }
    }
}

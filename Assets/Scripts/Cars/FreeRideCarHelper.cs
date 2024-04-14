using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars.Tools
{
    public class FreeRideCarHelper : MonoBehaviour
    {
        private const float FALL_OFFSET = 1f;

        public event Action OnReachPiece = delegate { };
        public event Action OnFall = delegate { };

        private float _minimalY = -10;

        private void Update()
        {
            if(transform.position.y < _minimalY - FALL_OFFSET)
                OnFall?.Invoke();
        }

        public void SetMinimalY(float minimalY) => _minimalY = minimalY;

        public void ReachPiece() => OnReachPiece?.Invoke();
    }
}

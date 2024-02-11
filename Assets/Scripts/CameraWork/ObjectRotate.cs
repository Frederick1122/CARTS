using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManger.Rotate
{
    public class ObjectRotate : MonoBehaviour
    {
        [SerializeField] private RotateDirection _rotateDirection = RotateDirection.Clockwise;
        [SerializeField] private float _speedRotate = 10;

        private bool _isActive = true;

        private void FixedUpdate()
        {
            if (!_isActive)
                return;

            var rotation = CalculateRotation();
            transform.Rotate(rotation * Time.fixedDeltaTime);
        }

        private Vector3 CalculateRotation()
        {
            var angle = new Vector3(0, _speedRotate, 0);
            switch ( _rotateDirection )
            {
                case RotateDirection.Clockwise:
                    return angle;
                case RotateDirection.Counterclockwise: 
                    return -angle;
            }

            return angle;
        }

        public void ChangeRotateCondition(bool condition) => _isActive = condition;
    }

    public enum RotateDirection
    {
        Clockwise = 0,
        Counterclockwise = 1
    }
}

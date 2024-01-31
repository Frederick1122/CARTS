using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManger.Rotate
{
    public class ObjectRotateAroundInput : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [Header("Characteristic")]
        [SerializeField] private float _speedRotate = 10;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _offsetZ = 6;
        [SerializeField] private float _sensitivity = 3;
        [SerializeField] private float _limit = 80;


        private float _x, _y;

        private void Start()
        {
            _limit = Mathf.Abs(_limit);
            _limit = _limit > 90 ? 90 : _limit;
            _offset = new Vector3(_offset.x, _offset.y, -Mathf.Abs(_offsetZ));

            transform.position = _target.position + _offset;
        }

        private void FixedUpdate()
        {
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (!(touch.deltaPosition.x != 0 && touch.deltaPosition.y != 0))
                    transform.RotateAround(_target.position, Vector3.up, _speedRotate * Time.deltaTime);
            }
            else
            {
                transform.RotateAround(_target.position, Vector3.up, _speedRotate * Time.deltaTime);
            }
            if (Input.touchCount <= 0)
                return;
#else
            if (!(Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Mouse Y") != 0))
                transform.RotateAround(_target.position, Vector3.up, _speedRotate * Time.deltaTime);

            if (!Input.GetMouseButton(0))
                return;
#endif
            _limit = Mathf.Abs(_limit); 

            ReadInput();

            transform.localEulerAngles = new Vector3(-_y, _x, 0);
            transform.position = transform.localRotation * _offset + _target.position;
        }

        public void ReadInput()
        {
#if UNITY_ANDROID
            var touch = Input.GetTouch(0);
            _x = transform.localEulerAngles.y + touch.deltaPosition.x * _sensitivity;
            _y += touch.deltaPosition.y * _sensitivity;
            _y = Mathf.Clamp(_y, -_limit, _limit);
#else
            _x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivity;
            _y += Input.GetAxis("Mouse Y") * _sensitivity;
            _y = Mathf.Clamp(_y, -_limit, _limit);
#endif
        }
    }
}

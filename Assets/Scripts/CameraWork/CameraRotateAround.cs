using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManger.Rotate
{
    public class CameraRotateAround : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 10;

        private void Update()
        {
            transform.RotateAround(_target.position, Vector3.up, _speed * Time.deltaTime);
        }
    }
}

using Cars.Controllers;
using System;
using UnityEngine;

namespace Cars.Tools
{
    [RequireComponent(typeof(TrailRenderer), typeof(ParticleSystem))]
    public class CarSkidMarks : MonoBehaviour
    {
        private const int ACTIVATE_VELOCITY = 7;

        private ParticleSystem _smoke;
        private CarController _controller;
        private TrailRenderer _skidMark;

        private float _fadeOutSpeed = 0;

        private void Start()
        {
            _skidMark = GetComponent<TrailRenderer>();
            _smoke = GetComponent<ParticleSystem>();
            _controller = GetComponentInParent<CarController>();

            _skidMark.emitting = false;

            if (_controller == null)
                return;

            _skidMark.startWidth = _controller.SkidWidth;
        }

        private void OnDisable() => _skidMark.enabled = false;

        private void FixedUpdate() => ParticleWork();

        private void ParticleWork()
        {
            if (_skidMark.emitting)
                _smoke.Play();
            else
                _smoke.Stop();

            if (_controller == null)
                return;

            if (!_controller.IsActive)
                return;

            if (_skidMark.enabled == false)
                _skidMark.enabled = true;

            if (_controller.IsGrounded)
            {
                if (Mathf.Abs(_controller.CarVelocity.x) > ACTIVATE_VELOCITY)
                {
                    _fadeOutSpeed = 0f;
                    _skidMark.materials[0].color = Color.black;
                    _skidMark.emitting = true;
                }
                else
                    _skidMark.emitting = false;
            }
            else
                _skidMark.emitting = false;

            if (!_skidMark.emitting)
            {
                _fadeOutSpeed += Time.fixedDeltaTime / 2;
                Color m_color = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), _fadeOutSpeed);
                _skidMark.materials[0].color = m_color;
                if (_fadeOutSpeed > 1)
                    _skidMark.Clear();
            }
        }
    }
}

using Cars.Controllers;
using System;
using UnityEngine;

namespace Cars.Tools
{
    [RequireComponent(typeof(TrailRenderer), typeof(ParticleSystem))]
    public class CarSkidMarks : MonoBehaviour
    {
        public bool IsEmitting => _skidMark.emitting;

        private ParticleSystem _smoke;
        private CarController _controller;
        private TrailRenderer _skidMark;

        public void Init(CarController controller)
        {
            _controller = controller;

            _skidMark = GetComponent<TrailRenderer>();
            _smoke = GetComponent<ParticleSystem>();

            _skidMark.emitting = false;
            _smoke.Stop();

            if (_controller == null)
                return;

            _skidMark.startWidth = _controller.SkidWidth;
        }

        public void SetSkidMarkEnabled(bool condition) => _skidMark.enabled = condition;
        public void SetSkidEmitting(bool condition) => _skidMark.emitting = condition;
        public void SetMaterialColor(int matNumer, Color color) => _skidMark.materials[matNumer].color = color;
        public void ClearSkid() => _skidMark.Clear();

        public void UpdateSmoke()
        {
            if (_skidMark.emitting)
                _smoke.Play();
            else
                _smoke.Stop();
        }
    }
}

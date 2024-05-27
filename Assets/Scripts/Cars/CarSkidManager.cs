using Cars.Controllers;
using UnityEngine;

namespace Cars.Tools
{
    public class CarSkidManager : MonoBehaviour
    {
        private const int ACTIVATE_VELOCITY = 7;

        public bool IsDrifting => _isEmitting;

        [SerializeField] private CarController _carController;
        
        private CarSkidMarks[] _skidMarks;
        private float _fadeOutSpeed = 0;

        // Cashed
        private bool _isEmitting = false;
        private Color _skidMarkColor = Color.black;
        private bool _needClear = false;

        public void Init(CarController carController)
        {
            _carController = carController;
            _skidMarks = GetComponentsInChildren<CarSkidMarks>();

            foreach (var skidMark in _skidMarks)
            {
                skidMark.Init(_carController);
                skidMark.SetSkidMarkEnabled(true);
            }
        }

        private void Update() => ParticleWork();

        private void ParticleWork()
        {
            if (_carController == null)
                return;

            if (!_carController.IsActive)
                return;

            if (_carController.IsGrounded)
            {
                if (Mathf.Abs(_carController.CarVelocity.x) > ACTIVATE_VELOCITY)
                {
                    _fadeOutSpeed = 0f;
                    _skidMarkColor = Color.black;
                    //_skidMark.materials[0].color = Color.black;
                    _isEmitting = true;
                }
                else
                    _isEmitting = false;
            }
            else
                _isEmitting = false;

            if (_isEmitting)
            {
                _fadeOutSpeed += Time.fixedDeltaTime / 2;
                _skidMarkColor = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), _fadeOutSpeed);
                if (_fadeOutSpeed > 1)
                    _needClear = true;
            }

            foreach (var skidMark in _skidMarks)
            {
                if (_needClear)
                {
                    skidMark.ClearSkid();
                    continue;
                }

                skidMark.SetSkidEmitting(_isEmitting);
                skidMark.SetMaterialColor(0, _skidMarkColor);
                skidMark.UpdateSmoke();
            }
        }
    }
}

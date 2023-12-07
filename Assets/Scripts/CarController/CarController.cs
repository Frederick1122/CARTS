using System;
using System.Collections.Generic;
using UnityEngine;

namespace OldCode
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class CarController : MonoBehaviour
    {
        [field: SerializeField] public CarConfig CarConfig { get; private set; }

        [Header("Wheels")] [SerializeField] private Wheel _wheelFL;
        [SerializeField] private Wheel _wheelFR;
        [SerializeField] private Wheel _wheelRL;
        [SerializeField] private Wheel _wheelRR;

        public float CarSpeed { get; private set; }
        public bool IsDrifting { get; private set; }
        public bool IsTractionLocked { get; private set; }
        public float LocalVelocityZ { get; private set; }
        public float LocalVelocityX { get; private set; }

        protected CarInputSystem _carInputSystem;
        protected float _steeringAxis => _carInputSystem.SteeringInput;
        protected float _throttleAxis => _carInputSystem.GasInput;

        protected Rigidbody _rb;
        private float _driftingAxis;
        private bool _deceleratingCar;

        private WheelFrictionCurve _wheelFrictionFL;
        private float _wextremumSlipFL;
        private WheelFrictionCurve _wheelFrictionFR;
        private float _wextremumSlipFR;
        private WheelFrictionCurve _wheelFrictionRL;
        private float _wextremumSlipRL;
        private WheelFrictionCurve _wheelFrictionRR;
        private float _wextremumSlipRR;

        public void Init(List<CheckPoint> checkPoints)
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _rb.centerOfMass = CarConfig.bodyMassCenter;

            _carInputSystem = gameObject.GetComponent<CarInputSystem>();
            if (_carInputSystem == null)
                Debug.LogError("NO CAR INPUT");

            _carInputSystem.Init(checkPoints);
            SetUpWheels();
            _carInputSystem.OnNeedToResetCar += ResetCar;
        }

        private void OnDestroy()
        {
            _carInputSystem.OnNeedToResetCar -= ResetCar;
        }

        protected virtual void FixedUpdate()
        {
            CarSpeed = 2 * Mathf.PI * _wheelFL.WheelCollider.radius * _wheelFL.WheelCollider.rpm * 60 / 1000;
            LocalVelocityX = transform.InverseTransformDirection(_rb.velocity).x;
            LocalVelocityZ = transform.InverseTransformDirection(_rb.velocity).z;

            if (_carInputSystem.IsForwardButton || _carInputSystem.IsBackWardButton)
            {
                CancelInvoke(nameof(DecelerateCar));
                _deceleratingCar = false;
                Move();
            }

            if (_carInputSystem.SteeringInput != 0)
                Steering();

            if (_carInputSystem.IsBraking)
            {
                CancelInvoke(nameof(DecelerateCar));
                _deceleratingCar = false;
                Handbrake();
            }

            if (!_carInputSystem.IsBraking)
                RecoverTraction();

            if (!_carInputSystem.IsBackWardButton && !_carInputSystem.IsForwardButton)
                ThrottleOff();

            if (!_carInputSystem.IsBackWardButton && !_carInputSystem.IsForwardButton && !_carInputSystem.IsBraking &&
                !_deceleratingCar)
            {
                InvokeRepeating(nameof(DecelerateCar), 0f, 0.1f);
                _deceleratingCar = true;
            }

            if (!_carInputSystem.IsLeftButton && !_carInputSystem.IsRightButton && _steeringAxis != 0f)
                Steering();

            ApplyWheelPosition();
        }

        private (WheelFrictionCurve, float) SetUpCurve(Wheel wheel)
        {
            var sidewaysFriction = wheel.WheelCollider.sidewaysFriction;

            WheelFrictionCurve wheelFrictionCurve = new()
            {
                extremumSlip = sidewaysFriction.extremumSlip,
                extremumValue = sidewaysFriction.extremumValue,
                asymptoteSlip = sidewaysFriction.asymptoteSlip,
                asymptoteValue = sidewaysFriction.asymptoteValue,
                stiffness = sidewaysFriction.stiffness
            };

            var extremumSlip = sidewaysFriction.extremumSlip;

            return (wheelFrictionCurve, extremumSlip);
        }

        private void SetUpWheels()
        {
            var wheelSettingsFL = SetUpCurve(_wheelFL);
            _wheelFrictionFL = wheelSettingsFL.Item1;
            _wextremumSlipFL = wheelSettingsFL.Item2;
            var wheelSettingsFR = SetUpCurve(_wheelFR);
            _wheelFrictionFR = wheelSettingsFR.Item1;
            _wextremumSlipFR = wheelSettingsFR.Item2;
            var wheelSettingsRL = SetUpCurve(_wheelRL);
            _wheelFrictionRL = wheelSettingsRL.Item1;
            _wextremumSlipRL = wheelSettingsRL.Item2;
            var wheelSettingsRR = SetUpCurve(_wheelRR);
            _wheelFrictionRR = wheelSettingsRR.Item1;
            _wextremumSlipRR = wheelSettingsRR.Item2;
        }

        private void Steering()
        {
            var steeringAngle = _steeringAxis * CarConfig.maxSteeringAngle;
            _wheelFL.WheelCollider.steerAngle =
                Mathf.Lerp(_wheelFL.WheelCollider.steerAngle, steeringAngle, CarConfig.steeringSpeed);
            _wheelFR.WheelCollider.steerAngle =
                Mathf.Lerp(_wheelFR.WheelCollider.steerAngle, steeringAngle, CarConfig.steeringSpeed);
        }

        private void ApplyWheelPosition()
        {
            UpdateWheelPosition(_wheelFL);
            UpdateWheelPosition(_wheelFR);
            UpdateWheelPosition(_wheelRL);
            UpdateWheelPosition(_wheelRR);
        }

        private void UpdateWheelPosition(Wheel wheel)
        {
            wheel.WheelCollider.GetWorldPose(out var position, out var rotation);
            wheel.WheelMesh.transform.position = position;
            wheel.WheelMesh.transform.rotation = rotation;
        }

        private void Move()
        {
            if (_throttleAxis < 0)
                Debug.Log(_throttleAxis);

            IsDrifting = Mathf.Abs(LocalVelocityX) > 2.5f;

            if (LocalVelocityZ < -1f && _carInputSystem.IsForwardButton ||
                LocalVelocityZ > 1f && _carInputSystem.IsBackWardButton)
                Brakes();
            else
            {
                if (Mathf.RoundToInt(CarSpeed) >= CarConfig.maxSpeed)
                    ThrottleOff();
                else
                {
                    var motorTorque = (CarConfig.accelerationMultiplier * 50f) * _throttleAxis;

                    _wheelFL.WheelCollider.brakeTorque = 0;
                    _wheelFL.WheelCollider.motorTorque = motorTorque;

                    _wheelFR.WheelCollider.brakeTorque = 0;
                    _wheelFR.WheelCollider.motorTorque = motorTorque;

                    _wheelRL.WheelCollider.brakeTorque = 0;
                    _wheelRL.WheelCollider.motorTorque = motorTorque;

                    _wheelRR.WheelCollider.brakeTorque = 0;
                    _wheelRR.WheelCollider.motorTorque = motorTorque;
                }
            }
        }

        private void ThrottleOff()
        {
            _wheelFL.WheelCollider.motorTorque = 0;
            _wheelFR.WheelCollider.motorTorque = 0;
            _wheelRL.WheelCollider.motorTorque = 0;
            _wheelRR.WheelCollider.motorTorque = 0;
        }

        private void DecelerateCar()
        {
            IsDrifting = Mathf.Abs(LocalVelocityX) > 2.5f;

            _rb.velocity *= 1f / (1f + 0.025f * CarConfig.decelerationMultiplier);
            ThrottleOff();

            if (!(_rb.velocity.magnitude < 0.25f))
                return;

            _rb.velocity = Vector3.zero;
            CancelInvoke(nameof(DecelerateCar));
        }

        private void Brakes()
        {
            _wheelFL.WheelCollider.brakeTorque = CarConfig.brakeForce;
            _wheelFR.WheelCollider.brakeTorque = CarConfig.brakeForce;
            _wheelRL.WheelCollider.brakeTorque = CarConfig.brakeForce;
            _wheelRR.WheelCollider.brakeTorque = CarConfig.brakeForce;
        }

        private void Handbrake()
        {
            CancelInvoke(nameof(RecoverTraction));

            _driftingAxis += Time.deltaTime;
            float secureStartingPoint = _driftingAxis * _wextremumSlipFL * CarConfig.handbrakeDriftMultiplier;

            Brakes();

            if (secureStartingPoint < _wextremumSlipFL)
            {
                _driftingAxis = _wextremumSlipFL / (_wextremumSlipFL * CarConfig.handbrakeDriftMultiplier);
            }

            if (_driftingAxis > 1f)
            {
                _driftingAxis = 1f;
            }

            if (Mathf.Abs(LocalVelocityX) > 2.5f)
            {
                IsDrifting = true;
            }
            else
            {
                IsDrifting = false;
            }

            if (_driftingAxis < 1f)
            {
                _wheelFrictionFL.extremumSlip = _wextremumSlipFL * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelFL.WheelCollider.sidewaysFriction = _wheelFrictionFL;

                _wheelFrictionFR.extremumSlip = _wextremumSlipFR * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelFR.WheelCollider.sidewaysFriction = _wheelFrictionFR;

                _wheelFrictionRL.extremumSlip = _wextremumSlipRL * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelRL.WheelCollider.sidewaysFriction = _wheelFrictionRL;

                _wheelFrictionRR.extremumSlip = _wextremumSlipRR * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelRR.WheelCollider.sidewaysFriction = _wheelFrictionRR;
            }

            IsTractionLocked = true;
        }

        private void RecoverTraction()
        {
            IsTractionLocked = false;
            _driftingAxis = _driftingAxis - Time.deltaTime / 1.5f < 0f ? 0f : _driftingAxis - Time.deltaTime;

            if (_wheelFrictionFL.extremumSlip > _wextremumSlipFL)
            {
                _wheelFrictionFL.extremumSlip = _wextremumSlipFL * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelFL.WheelCollider.sidewaysFriction = _wheelFrictionFL;

                _wheelFrictionFR.extremumSlip = _wextremumSlipFR * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelFR.WheelCollider.sidewaysFriction = _wheelFrictionFR;

                _wheelFrictionRL.extremumSlip = _wextremumSlipRL * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelRL.WheelCollider.sidewaysFriction = _wheelFrictionRL;

                _wheelFrictionRR.extremumSlip = _wextremumSlipRR * CarConfig.handbrakeDriftMultiplier * _driftingAxis;
                _wheelRR.WheelCollider.sidewaysFriction = _wheelFrictionRR;

                Invoke(nameof(RecoverTraction), Time.deltaTime);
            }
            else if (_wheelFrictionFL.extremumSlip < _wextremumSlipFL)
            {
                _wheelFrictionFL.extremumSlip = _wextremumSlipFL;
                _wheelFL.WheelCollider.sidewaysFriction = _wheelFrictionFL;

                _wheelFrictionFR.extremumSlip = _wextremumSlipFR;
                _wheelFR.WheelCollider.sidewaysFriction = _wheelFrictionFR;

                _wheelFrictionRL.extremumSlip = _wextremumSlipRL;
                _wheelRL.WheelCollider.sidewaysFriction = _wheelFrictionRL;

                _wheelFrictionRR.extremumSlip = _wextremumSlipRR;
                _wheelRR.WheelCollider.sidewaysFriction = _wheelFrictionRR;

                _driftingAxis = 0f;
            }
        }

        protected void ResetCar()
        {
            var checkPoint = _carInputSystem.LastReachedCheckPoint.GetWayPoint();
            gameObject.transform.position = checkPoint.position;
            gameObject.transform.rotation = checkPoint.rotation;
            _rb.velocity = Vector3.zero;
        }
    }

    [Serializable]
    public class Wheel
    {
        [field: SerializeField] public WheelCollider WheelCollider { get; private set; }
        [field: SerializeField] public MeshRenderer WheelMesh { get; private set; }
    }
}
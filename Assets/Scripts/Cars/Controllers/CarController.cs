using Cars.InputSystem;
using Cinemachine;
using ConfigScripts;
using System.Collections.Generic;
using UnityEngine;

namespace Cars.Controllers
{
    public abstract class CarController : MonoBehaviour
    {
        public float SkidWidth { get; set; }
        public float DesiredTurning { get; protected set; }
        public Vector3 CarVelocity { get; protected set; }

        public CarConfig Config { get; protected set; }

        private CarPresetConfig _presetConfig;

        private MovementMode _movementMode;
        private GroundCheck _groundCheck;
        private LayerMask _drivableSurface;

        private Rigidbody _rbSphere;
        private Rigidbody _carBody;

        private Transform _bodyMesh;
        private Transform[] _frontWheels = new Transform[2];
        private Transform[] _rearWheels = new Transform[2];

        protected CinemachineVirtualCamera _camera;

        private IInputSystem _inputSystem;
        private ITargetHolder _targetHolder;

        private RaycastHit _hit;
        private float _radius;
        private readonly Dictionary<Transform, Transform> _wheelsAxel = new();
        private SphereCollider _sphereCollider;
        private bool _isCarActive = false;

        private float _speedModifier = 0;
        private float _accelerationModifier = 0;

        public void StartCar()
        {
            _inputSystem.IsActive = true;
            _isCarActive = true;
        }

        public void StopCar()
        {
            _inputSystem.IsActive = false;
            _isCarActive = false;
        }

        public abstract float GetPassedDistance();

        public virtual void Init(IInputSystem inputSystem, CarConfig carConfig, CarPresetConfig carPresetConfig, ITargetHolder targetHolder = null)
        {
            _targetHolder = targetHolder;
            _inputSystem = inputSystem;
            Config = carConfig;
            _presetConfig = carPresetConfig;

            InitFromConfig(carPresetConfig);
            InitFromCarPrefabData(GetComponent<CarPrefabData>());

            _sphereCollider = _rbSphere.GetComponent<SphereCollider>();
            _radius = _sphereCollider.radius;

            _sphereCollider.material = CopyMaterial(Config.frictionMaterial);

            if (_movementMode == MovementMode.AngularVelocity)
                Physics.defaultMaxAngularSpeed = 100;

            _wheelsAxel.Add(_frontWheels[0], _frontWheels[0].GetChild(0));
            _wheelsAxel.Add(_frontWheels[1], _frontWheels[1].GetChild(0));

            _inputSystem.IsActive = false;
        }

        protected virtual void FixedUpdate()
        {
            if (!_isCarActive)
                return;

            Visual();
            CalculateDesiredAngle();

            Move();
        }

        private PhysicMaterial CopyMaterial(PhysicMaterial mat)
        {
            var frictionMaterial = new PhysicMaterial
            {
                staticFriction = mat.staticFriction,
                dynamicFriction = mat.dynamicFriction,
                frictionCombine = mat.frictionCombine,
                bounciness = mat.bounciness,
                bounceCombine = mat.bounceCombine
            };

            return frictionMaterial;
        }

        protected abstract void CalculateDesiredAngle();

        protected virtual void Move()
        {
            CarVelocity = _carBody.transform.InverseTransformDirection(_carBody.velocity);

            var verticalInput = _inputSystem.VerticalInput;
            var horizontalInput = _inputSystem.HorizontalInput;
            var brakeInput = _inputSystem.BrakeInput;

            var maxSpeed = Config.maxSpeedLevels[0] * (1 + _speedModifier);
            var acceleration = Config.accelerationLevels[0] * (1 + _accelerationModifier);
            var turnSpeed = Config.turnLevels[0];

            //changes friction according to sideways speed of car
            if (Mathf.Abs(CarVelocity.x) > 0)
                _sphereCollider.material.dynamicFriction = Config.frictionCurve.Evaluate(Mathf.Abs(CarVelocity.x / 100));

            if (CheckIfGrounded())
            {
                //turnlogic
                float sign = Mathf.Sign(CarVelocity.z);
                float turnMultiplier = Config.turnCurve.Evaluate(CarVelocity.magnitude / maxSpeed);

                // ????
                if (verticalInput > 0.1f || CarVelocity.z > 1)
                    _carBody.AddTorque(100 * horizontalInput * sign * turnMultiplier * turnSpeed * Vector3.up);
                else if (verticalInput < -0.1f || CarVelocity.z < -1)
                    _carBody.AddTorque(100 * horizontalInput * sign * turnMultiplier * turnSpeed * Vector3.up);


                //brakelogic
                _rbSphere.constraints = brakeInput > 0.1f ? RigidbodyConstraints.FreezeRotationX :
                    RigidbodyConstraints.None;

                //acceleration logic
                switch (_movementMode)
                {
                    case MovementMode.AngularVelocity:
                        if (Mathf.Abs(verticalInput) > 0.1f)
                        {
                            _rbSphere.angularVelocity = Vector3.Lerp(_rbSphere.angularVelocity,
                                maxSpeed * verticalInput * _carBody.transform.right / _radius, acceleration * Time.fixedDeltaTime);
                        }
                        break;

                    case MovementMode.Velocity:
                        if (Mathf.Abs(verticalInput) > 0.1f && brakeInput < 0.1f)
                        {
                            _rbSphere.velocity = Vector3.Lerp(_rbSphere.velocity,
                                maxSpeed * verticalInput * _carBody.transform.forward, acceleration / 10 * Time.fixedDeltaTime);
                        }
                        break;
                }

                // down force
                _rbSphere.AddForce(_rbSphere.mass * Config.downforce * -transform.up);

                //body tilt
                _carBody.MoveRotation(Quaternion.Slerp(_carBody.rotation,
                    Quaternion.FromToRotation(_carBody.transform.up, _hit.normal) * _carBody.transform.rotation, 0.12f));
            }
            else
            {
                if (Config.airControl)
                {
                    //turnlogic
                    float turnMultiplier = Config.turnCurve.Evaluate(CarVelocity.magnitude / maxSpeed);

                    _carBody.AddTorque(100 * horizontalInput * turnMultiplier * turnSpeed * Vector3.up);
                }

                _carBody.MoveRotation(Quaternion.Slerp(_carBody.rotation,
                    Quaternion.FromToRotation(_carBody.transform.up, Vector3.up) * _carBody.transform.rotation, 0.02f));

                _rbSphere.velocity = Vector3.Lerp(_rbSphere.velocity,
                    _rbSphere.velocity + Vector3.down * Config.gravity, Time.fixedDeltaTime * Config.gravity);
            }

        }

        protected virtual void Visual()
        {
            // wheels
            foreach (Transform fw in _frontWheels)
            {
                fw.localRotation = Quaternion.Slerp(fw.localRotation, Quaternion.Euler(fw.localRotation.eulerAngles.x,
                    30 * _inputSystem.HorizontalInput, fw.localRotation.eulerAngles.z), 0.1f);
                _wheelsAxel[fw].localRotation = _rbSphere.transform.localRotation;
            }
            _rearWheels[0].localRotation = _rbSphere.transform.localRotation;
            _rearWheels[1].localRotation = _rbSphere.transform.localRotation;

            //Body
            if (CarVelocity.z > 1)
            {
                _bodyMesh.localRotation = Quaternion.Slerp(_bodyMesh.localRotation,
                    Quaternion.Euler(Mathf.Lerp(0, -5, CarVelocity.z / Config.maxSpeedLevels[0]), _bodyMesh.localRotation.eulerAngles.y,
                        Mathf.Clamp(DesiredTurning * _inputSystem.HorizontalInput, -Config.bodyTilt, Config.bodyTilt)), 0.05f);
            }
            else
                _bodyMesh.localRotation = Quaternion.Slerp(_bodyMesh.localRotation, Quaternion.Euler(0, 0, 0), 0.05f);
        }

        private bool CheckIfGrounded()
        {
            Vector3 origin = _rbSphere.position + _radius * Vector3.up;
            var direction = -transform.up;
            var maxdistance = _radius + 0.2f;

            switch (_groundCheck)
            {
                case GroundCheck.rayCast:
                    if (Physics.Raycast(_rbSphere.position, Vector3.down, out _hit, maxdistance, _drivableSurface))
                        return true;
                    break;

                case GroundCheck.sphereCaste:
                    if (Physics.SphereCast(origin, _radius + 0.1f, direction, out _hit, maxdistance, _drivableSurface))
                        return true;
                    break;
            }

            return false;
        }

        public void IncreaseModifier(float speed, float acceleration)
        {
            _speedModifier += speed;
            _accelerationModifier += acceleration;
        }

        private void InitFromConfig(CarPresetConfig carPresetConfig)
        {
            _movementMode = carPresetConfig.MovementMode;
            _groundCheck = carPresetConfig.GroundCheck;
            _drivableSurface = carPresetConfig.DrivableSurface;
        }

        private void InitFromCarPrefabData(CarPrefabData carData)
        {
            _rbSphere = carData.RbSphere;
            _carBody = carData.CarBody;
            _bodyMesh = carData.BodyMesh;
            _frontWheels = carData.FrontWheels;
            _rearWheels = carData.RearWheels;
            _camera = carData.Camera;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_rbSphere == null)
            {
                Debug.Log("No sphere");
                return;
            }

            if (_sphereCollider == null)
                _sphereCollider = _rbSphere.GetComponent<SphereCollider>();

            //debug gizmos
            _radius = _sphereCollider.radius;
            float width = 0.02f;
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(_rbSphere.transform.position + ((_radius + width) * Vector3.down), new Vector3(2 * _radius, 2 * width, 4 * _radius));
                if (GetComponent<BoxCollider>())
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
                }
            }
        }
#endif
    }

    public enum GroundCheck
    {
        rayCast,
        sphereCaste
    };

    public enum MovementMode
    {
        Velocity,
        AngularVelocity
    };
}
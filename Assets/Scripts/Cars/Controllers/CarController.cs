using Cars.InputSystem;
using Cars.Tools;
using Cinemachine;
using ConfigScripts;
using Core.Tools;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using Obstacles;
using UnityEngine;

namespace Cars.Controllers
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class CarController : MonoBehaviour
    {
        private const int SPEED_STEP_PERCENT = 50;

        public float SkidWidth { get; set; }
        public float DesiredTurning { get; protected set; }
        public Vector3 CarVelocity { get; protected set; }
        public bool IsGrounded { get; protected set; }
        public bool IsActive => _isCarActive;

        public CarConfig Config { get; protected set; }

        private CarPresetConfig _presetConfig;

        private MovementMode _movementMode;
        private GroundCheck _groundCheck;
        private LayerMask _drivableSurface;
        private LayerMask _onCarLayers;

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

        private CarCollisionDetection _collisionDetection;
        private float _carResistanceAfterSpawn;
        private CancellationTokenSource _resistanceToken;
        private int _permanentSpeedModifier;
        private List<SpeedModifier> _speedModifiers = new();

        protected float _baseSpeedModifier = 1;
        protected float _baseAccelerationModifier = 1;

        protected float _maxSpeed = 0;
        protected float _turnSpeed = 0;
        protected float _acceleration = 0;

        private float _lastHorizontalInput = 0;

        private readonly List<Renderer> _onCarRenderer = new();
        
        public void IncreaseModifier(float speed, float acceleration)
        {
            _baseSpeedModifier += speed;
            _baseAccelerationModifier += acceleration;
        }

        public void AddSpeedModifier(SpeedModifier speedModifier, bool isPermanent = false)
        {
            if (isPermanent) 
                _permanentSpeedModifier = speedModifier.isBoost ? 1 : -1;
            else 
                _speedModifiers.Add(speedModifier);
        }

        public void RemovePermanentSpeedModifier() => _permanentSpeedModifier = 0;
        
        public virtual void Init(IInputSystem inputSystem, CarConfig carConfig, 
            CarPresetConfig carPresetConfig, CarCollisionDetection carCollisionDetection, 
            ITargetHolder targetHolder = null)
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

            _collisionDetection = carCollisionDetection;
            _collisionDetection.Init(GetComponent<BoxCollider>(), _onCarLayers);

            var childRenderer = GetComponentsInChildren<Renderer>();
            foreach (var child in childRenderer)
            {
                if (child.TryGetComponent(out ParticleSystem _))
                    continue;

                _onCarRenderer.Add(child);
            }

            SetUpCharacteristic();
        }

        private void OnDestroy()
        {
            _resistanceToken?.Cancel();
        }

        protected virtual void FixedUpdate()
        {
            if (!_isCarActive)
                return;

            CalculateDesiredAngle();

            Move();
            Visual();
        }

        public abstract void SetUpCharacteristic();
        protected abstract void CalculateDesiredAngle();

        private void InitFromConfig(CarPresetConfig carPresetConfig)
        {
            _movementMode = carPresetConfig.MovementMode;
            _groundCheck = carPresetConfig.GroundCheck;
            _drivableSurface = carPresetConfig.DrivableSurface;
            _carResistanceAfterSpawn = carPresetConfig.CarResistanceAfterSpawn;
        }

        private void InitFromCarPrefabData(CarPrefabData carData)
        {
            _rbSphere = carData.RbSphere;
            _carBody = carData.CarBody;
            _bodyMesh = carData.BodyMesh;
            _frontWheels = carData.FrontWheels;
            _rearWheels = carData.RearWheels;
            _camera = carData.Camera;
            _onCarLayers = carData.OnCarLayers;
            SkidWidth = carData.SkidWidth;
        }

        public void TurnEngineOn()
        {
            _inputSystem.IsActive = true;
            _isCarActive = true;

            // test
            MakeResistance(_carResistanceAfterSpawn);
        }

        public void TurnEngineOff()
        {
            _inputSystem.IsActive = false;
            _isCarActive = false;
        }

        public void ResetCar(Vector3 pos, Quaternion rot)
        {
            MakeResistance(_carResistanceAfterSpawn);

            _carBody.velocity = Vector3.zero;
            _bodyMesh.localRotation = Quaternion.Euler(Vector3.zero);

            _rbSphere.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _rbSphere.velocity = Vector3.zero;
            _rbSphere.angularVelocity = Vector3.zero;
            _rbSphere.constraints = RigidbodyConstraints.None;

            foreach (var fwheel in _frontWheels)
                fwheel.localRotation = Quaternion.Euler(Vector3.zero);
            foreach (var rwheel in _rearWheels)
                rwheel.localRotation = Quaternion.Euler(Vector3.zero);

            transform.SetPositionAndRotation(pos, rot);
        }

        public void MakeResistance(float time = -1)
        {
            _resistanceToken = new CancellationTokenSource();
            MakeResistanceCuro(_resistanceToken.Token, time).Forget();
        }

        public void ChangeModifier(float speedModifier, float accelerationModifier)
        {
            _baseSpeedModifier = speedModifier;
            _baseAccelerationModifier = accelerationModifier;
        }

        private async UniTaskVoid MakeResistanceCuro(CancellationToken token, float time = -1)
        {
            var resistanceTime = time == -1 ? _carResistanceAfterSpawn : time;
            _collisionDetection.IsWork = true;
            BeResistance();

            await UniTask.Delay(TimeSpan.FromSeconds(resistanceTime), cancellationToken: token);

            if (_collisionDetection.CollisionCanTurnOn)
                BeUnResistance();
            else
                _collisionDetection.OnNoCollidersIn += BeUnResistance;
        }

        private void BeResistance()
        {
            foreach (var renderer in _onCarRenderer)
            {
                var mat = renderer.material;
                mat.ToFadeMode();
                var color1 = new Color(mat.color.r, mat.color.g, mat.color.b, 0.3f);
                var color2 = new Color(mat.color.r, mat.color.g, mat.color.b, 0.6f);
                mat.color = color2;
                mat.DOColor(color1, 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void BeUnResistance()
        {
            _collisionDetection.OnNoCollidersIn -= BeUnResistance;
            _collisionDetection.IsWork = false;

            foreach (var renderer in _onCarRenderer)
            {
                var mat = renderer.material;
                mat.ToOpaqueMode();
                DOTween.Kill(mat);
                mat.SetOverrideTag("RenderType", "");
                var color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);
                mat.DOColor(color, 0);
            }
        }

        protected virtual void Move()
        {
            CarVelocity = _carBody.transform.InverseTransformDirection(_carBody.velocity) * 2;

            var verticalInput = _inputSystem.VerticalInput;
            var horizontalInput = Mathf.Lerp(_lastHorizontalInput, _inputSystem.HorizontalInput, _turnSpeed * 3 * Time.fixedDeltaTime);
            var brakeInput = _inputSystem.BrakeInput;

            _lastHorizontalInput = horizontalInput;

            var speedModificator = _permanentSpeedModifier;

            foreach (var speedModifier in _speedModifiers)
                speedModificator += speedModifier.isBoost ? 1 : -1;

            if (speedModificator != 0)
            {
                speedModificator = speedModificator < 0 ? -1 : 1; 
                speedModificator *= SPEED_STEP_PERCENT;
            }

            var maxSpeed = _maxSpeed / 100 * (100 + speedModificator) * _baseSpeedModifier;
            var acceleration = _acceleration * _baseAccelerationModifier;
            var turnSpeed = _turnSpeed;

            UpdateSpeedModifiers(Time.fixedDeltaTime);

            //changes friction according to sideways speed of car
            if (Mathf.Abs(CarVelocity.x) > 0)
                _sphereCollider.material.dynamicFriction = Config.frictionCurve.Evaluate(Mathf.Abs(CarVelocity.x / maxSpeed));

            IsGrounded = CheckIfGrounded();

            if (IsGrounded)
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
                    Quaternion.Euler(Mathf.Lerp(0, -5, CarVelocity.z / _maxSpeed), _bodyMesh.localRotation.eulerAngles.y,
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
        
        private void UpdateSpeedModifiers(float elapsedTime)
        {
            for (var i = _speedModifiers.Count - 1; i > -1; i--)
            {
                if (_speedModifiers[i].time - elapsedTime < 0)
                    _speedModifiers.RemoveAt(i);
                else 
                    _speedModifiers[i].time -= elapsedTime;
            }
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
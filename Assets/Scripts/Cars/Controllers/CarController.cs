using System;
using Cars.InputSystem;
using Cars.Tools;
using Cinemachine;
using ConfigScripts;
using Obstacles;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using Knot.Localization;
using Managers;
using UnityEngine;

namespace Cars.Controllers
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class CarController : MonoBehaviour
    {
        private const int SPEED_STEP_PERCENT = 50;
        private const float SOUND_DELAY = 1f;
        
        private const string ENGINE_SOUND_PARAMETER = "Speed";
        private const string ENGINE_SOUND_EVENT = "event:/SFX/Auto/Engine";
        private const string CRASH_SOUND_EVENT = "event:/SFX/Auto/Crash";
        private const string DRIFT_SOUND_EVENT = "event:/SFX/Auto/Drift";
        private const string BRAKE_SOUND_EVENT = "event:/SFX/Auto/Brake";

        private static readonly string[] OBSTACLE_LAYERS = new[] {"AIObstacle", "carbody", "Default"};
        
        public bool IsActive => _isCarActive;

        public float SkidWidth { get; set; }
        public float DesiredTurning { get; protected set; }
        public Vector3 CarVelocity { get; protected set; }
        public bool IsGrounded { get; protected set; }
        public CarConfig Config { get; protected set; }

        private bool _isCarActive = false;

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

        // Car Collision
        private CarCollisionManager _collisionManager;

        // Modifiers
        private int _permanentSpeedModifier;
        private List<SpeedModifier> _speedModifiers = new();
        protected float _baseSpeedModifier = 1;
        protected float _baseAccelerationModifier = 1;

        // 
        protected float _maxSpeed = 0;
        protected float _turnSpeed = 0;
        protected float _acceleration = 0;

        private float _lastHorizontalInput = 0;
        //
        private bool _isBrake = false;
        
        private CancellationTokenSource _playSoundToken = new ();
        private HashSet<EventInstance> _activeEventInstances = new ();
        private EventInstance _engineInstance;
        private EventInstance _crashInstance;
        private EventInstance _driftInstance;
        private EventInstance _brakeInstance;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"{gameObject.name} punch {collision.gameObject.name}");
            var currentLayer = LayerMask.LayerToName(collision.gameObject.layer);
            
            if (Array.Exists(OBSTACLE_LAYERS, s => s == currentLayer))
                PlaySoundTask(_crashInstance, _playSoundToken.Token);
        }

        public virtual void Init(IInputSystem inputSystem, CarConfig carConfig, 
            CarPresetConfig carPresetConfig, CarCollisionDetection collisionDetection, 
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

            _collisionManager = gameObject.AddComponent<CarCollisionManager>();
            _collisionManager.Init(collisionDetection, carPresetConfig.CarResistanceAfterSpawn, _onCarLayers);
            
            _engineInstance = RuntimeManager.CreateInstance(ENGINE_SOUND_EVENT);
            _crashInstance = RuntimeManager.CreateInstance(CRASH_SOUND_EVENT);
            _driftInstance = RuntimeManager.CreateInstance(DRIFT_SOUND_EVENT);
            _brakeInstance = RuntimeManager.CreateInstance(BRAKE_SOUND_EVENT);

            _engineInstance.start();
            
            SetUpCharacteristic();
        }

        private void OnDestroy()
        {
            _playSoundToken.Dispose();
            
            _engineInstance.release();
            _crashInstance.release();
            _driftInstance.release();
            _brakeInstance.release();
        }

        protected virtual void FixedUpdate()
        {
            if (!_isCarActive)
                return;

            CalculateDesiredAngle();

            Move();
            Visual();
        }

        private void Update()
        {
            _engineInstance.set3DAttributes(gameObject.To3DAttributes());
            _engineInstance.setParameterByName(ENGINE_SOUND_PARAMETER, CarVelocity.magnitude);
        }

        public abstract void SetUpCharacteristic();
        
        protected abstract void CalculateDesiredAngle();

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
            _onCarLayers = carData.OnCarLayers;
            SkidWidth = carData.SkidWidth;
        }

        public void MakeResistance(float time = -1) => _collisionManager.MakeResistance(time);

        public void TurnEngineOn()
        {
            _inputSystem.IsActive = true;
            _isCarActive = true;

            // test
            MakeResistance();
        }

        public void TurnEngineOff()
        {
            _inputSystem.IsActive = false;
            _isCarActive = false;
        }

        public void ResetCar(Vector3 pos, Quaternion rot)
        {
            MakeResistance();

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

        public void ChangeModifier(float speedModifier, float accelerationModifier)
        {
            _baseSpeedModifier = speedModifier;
            _baseAccelerationModifier = accelerationModifier;
        }

        protected virtual int GetAdditionalSpeedModifier() { return 0; }

        protected virtual void Move()
        {
            CarVelocity = _carBody.transform.InverseTransformDirection(_carBody.velocity) * 2;

            var verticalInput = _inputSystem.VerticalInput;

            if (verticalInput < 0 && !_isBrake)
            {
                PlaySoundTask(_brakeInstance, _playSoundToken.Token);
                _isBrake = true;
            }
            else if (verticalInput > 0 && _isBrake)
            {
                _isBrake = false;
            }
            
            var horizontalInput = Mathf.Lerp(_lastHorizontalInput, _inputSystem.HorizontalInput, _turnSpeed * 3 * Time.fixedDeltaTime);
            var brakeInput = _inputSystem.BrakeInput;

            _lastHorizontalInput = horizontalInput;

            var currentSpeedModifier = _permanentSpeedModifier;

            foreach (var speedModifier in _speedModifiers)
                currentSpeedModifier += speedModifier.isBoost ? 1 : -1;

            if (currentSpeedModifier != 0)
            {
                currentSpeedModifier = currentSpeedModifier < 0 ? -1 : 1; 
                currentSpeedModifier *= SPEED_STEP_PERCENT;
            }

            currentSpeedModifier += GetAdditionalSpeedModifier();
            var maxSpeed = _maxSpeed / 100 * (100 + currentSpeedModifier) * _baseSpeedModifier;
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
        
        private async UniTaskVoid PlaySoundTask(EventInstance instance, CancellationToken token)
        {
            if (_activeEventInstances.Contains(instance))
            {
                return;
            }

            _activeEventInstances.Add(instance);
            instance.set3DAttributes(gameObject.To3DAttributes());
            instance.start();
            
            await UniTask.Delay(TimeSpan.FromSeconds(SOUND_DELAY), cancellationToken: token);

            _activeEventInstances.Remove(instance);
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
using System.Collections.Generic;
using ConfigScripts;
using UnityEngine;

public abstract class CarController : MonoBehaviour
{
    [field: SerializeField] public CarConfig Config { get; protected set; }
    
    [Space(10)]
    [Header("Movement Type")]
    [SerializeField] private MovementMode _movementMode;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private LayerMask _drivableSurface;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _rbSphere;
    [SerializeField] private Rigidbody _carBody;

    [Header("Visual")]
    [SerializeField] private Transform _bodyMesh;
    [SerializeField] private Transform[] _frontWheels = new Transform[2];
    [SerializeField] private Transform[] _rearWheels = new Transform[2];

    public float SkidWidth { get; set; }
    public float DesiredTurning { get; protected set; }
    public Vector3 CarVelocity { get; protected set; }

    private IInputSystem _inputSystem;
    private ITargetHolder _targetHolder;

    private RaycastHit _hit;
    private float _radius;
    private Dictionary<Transform, Transform> _wheelsAxel = new();
    private SphereCollider _sphereCollider;
    private bool _isCarActive = false;
    
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

    public virtual void Init(IInputSystem inputSystem, ITargetHolder targetHolder = null)
    {
        _targetHolder = targetHolder;
        _inputSystem = inputSystem;
        _sphereCollider = _rbSphere.GetComponent<SphereCollider>();
        _radius = _sphereCollider.radius;

        if (_movementMode == MovementMode.AngularVelocity)
            Physics.defaultMaxAngularSpeed = 100;

        _wheelsAxel.Add(_frontWheels[0], _frontWheels[0].GetChild(0));
        _wheelsAxel.Add(_frontWheels[1], _frontWheels[1].GetChild(0));

        _inputSystem.IsActive = false;
    }

    protected virtual void Update()
    {
        if (!_isCarActive)
            return;
        
        Visual();
        CalculateDesiredAngle();
    }

    protected abstract void CalculateDesiredAngle();


    protected virtual void FixedUpdate() => Move();

    protected virtual void Move()
    {
        CarVelocity = _carBody.transform.InverseTransformDirection(_carBody.velocity);

        var verticalInput = _inputSystem.VerticalInput;
        var horizontalInput = _inputSystem.HorizontalInput;
        var brakeInput = _inputSystem.BrakeInput;

        //changes friction according to sideways speed of car
        if (Mathf.Abs(CarVelocity.x) > 0)
            Config.frictionMaterial.dynamicFriction = Config.frictionCurve.Evaluate(Mathf.Abs(CarVelocity.x / 100));

        if(CheckIfGrounded())
        {
            //turnlogic
            float sign = Mathf.Sign(CarVelocity.z);
            float turnMultiplyer = Config.turnCurve.Evaluate(CarVelocity.magnitude / Config.maxSpeedLevels[0]);

            // ????
            if (verticalInput > 0.1f || CarVelocity.z > 1)
                _carBody.AddTorque(Vector3.up * horizontalInput * sign * Config.turnLevels[0] * 100 * turnMultiplyer);
            else if (verticalInput < -0.1f || CarVelocity.z < -1)
                _carBody.AddTorque(Vector3.up * horizontalInput * sign * Config.turnLevels[0] * 100 * turnMultiplyer);
            

            //brakelogic
            if (brakeInput > 0.1f)
                _rbSphere.constraints = RigidbodyConstraints.FreezeRotationX;
            else
                _rbSphere.constraints = RigidbodyConstraints.None;

            //accelaration logic
            switch (_movementMode)
            {
                case MovementMode.AngularVelocity:
                    if (Mathf.Abs(verticalInput) > 0.1f)
                    {
                        _rbSphere.angularVelocity = Vector3.Lerp(_rbSphere.angularVelocity,
                            _carBody.transform.right * verticalInput * Config.maxSpeedLevels[0] / _radius, Config.accelerationLevels[0] * Time.deltaTime);
                    }
                    break;

                case MovementMode.Velocity:
                    if (Mathf.Abs(verticalInput) > 0.1f && brakeInput < 0.1f)
                    {
                        _rbSphere.velocity = Vector3.Lerp(_rbSphere.velocity,
                            _carBody.transform.forward * verticalInput * Config.maxSpeedLevels[0], Config.accelerationLevels[0] / 10 * Time.deltaTime);
                    }
                    break;
            }
            
            // down froce
            _rbSphere.AddForce(-transform.up * Config.downforce * _rbSphere.mass);

            //body tilt
            _carBody.MoveRotation(Quaternion.Slerp(_carBody.rotation, 
                Quaternion.FromToRotation(_carBody.transform.up, _hit.normal) * _carBody.transform.rotation, 0.12f));
        }
        else
        {
            if (Config.airControl)
            {
                //turnlogic
                float TurnMultiplyer = Config.turnCurve.Evaluate(CarVelocity.magnitude / Config.maxSpeedLevels[0]);

                _carBody.AddTorque(Vector3.up * horizontalInput * Config.turnLevels[0] * 100 * TurnMultiplyer);
            }

            _carBody.MoveRotation(Quaternion.Slerp(_carBody.rotation, 
                Quaternion.FromToRotation(_carBody.transform.up, Vector3.up) * _carBody.transform.rotation, 0.02f));

            _rbSphere.velocity = Vector3.Lerp(_rbSphere.velocity, 
                _rbSphere.velocity + Vector3.down * Config.gravity, Time.deltaTime * Config.gravity);
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

    protected bool CheckIfGrounded()
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

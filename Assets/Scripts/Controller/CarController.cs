using System.Collections;
using System.Collections.Generic;
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

    private RaycastHit _hit;
    private float _radius;
    private Dictionary<Transform, Transform> _wheelsAxel = new();
    private SphereCollider _sphereCollider;

    public virtual void Init(IInputSystem inputSystem)
    {
        _inputSystem = inputSystem;
        _sphereCollider = _rbSphere.GetComponent<SphereCollider>();
        _radius = _sphereCollider.radius;

        if (_movementMode == MovementMode.AngularVelocity)
            Physics.defaultMaxAngularSpeed = 100;

        _wheelsAxel.Add(_frontWheels[0], _frontWheels[0].GetChild(0));
        _wheelsAxel.Add(_frontWheels[1], _frontWheels[1].GetChild(0));
    }

    protected virtual void Update()
    {
        Visual();

        CalculateDesiredAngle();
    }

    protected abstract void CalculateDesiredAngle();


    protected virtual void FixedUpdate() =>
        Move();

    protected virtual void Move()
    {
        CarVelocity = _carBody.transform.InverseTransformDirection(_carBody.velocity);

        var verticalInput = _inputSystem.VerticalInput;
        var horizontalInput = _inputSystem.HorizontalInput;
        var brakeInput = _inputSystem.BrakeInput;

        //changes friction according to sideways speed of car
        if (Mathf.Abs(CarVelocity.x) > 0)
            Config.FrictionMaterial.dynamicFriction = Config.FrictionCurve.Evaluate(Mathf.Abs(CarVelocity.x / 100));

        if(CheckIfGrounded())
        {
            //turnlogic
            float sign = Mathf.Sign(CarVelocity.z);
            float turnMultiplyer = Config.TurnCurve.Evaluate(CarVelocity.magnitude / Config.MaxSpeed);

            // ????
            if (verticalInput > 0.1f || CarVelocity.z > 1)
                _carBody.AddTorque(Vector3.up * horizontalInput * sign * Config.Turn * 100 * turnMultiplyer);
            else if (verticalInput < -0.1f || CarVelocity.z < -1)
                _carBody.AddTorque(Vector3.up * horizontalInput * sign * Config.Turn * 100 * turnMultiplyer);
            

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
                            _carBody.transform.right * verticalInput * Config.MaxSpeed / _radius, Config.Accelaration * Time.deltaTime);
                    }
                    break;

                case MovementMode.Velocity:
                    if (Mathf.Abs(verticalInput) > 0.1f && brakeInput < 0.1f)
                    {
                        _rbSphere.velocity = Vector3.Lerp(_rbSphere.velocity,
                            _carBody.transform.forward * verticalInput * Config.MaxSpeed, Config.Accelaration / 10 * Time.deltaTime);
                    }
                    break;
            }
            
            // down froce
            _rbSphere.AddForce(-transform.up * Config.Downforce * _rbSphere.mass);

            //body tilt
            _carBody.MoveRotation(Quaternion.Slerp(_carBody.rotation, 
                Quaternion.FromToRotation(_carBody.transform.up, _hit.normal) * _carBody.transform.rotation, 0.12f));
        }
        else
        {
            if (Config.AirControl)
            {
                //turnlogic
                float TurnMultiplyer = Config.TurnCurve.Evaluate(CarVelocity.magnitude / Config.MaxSpeed);

                _carBody.AddTorque(Vector3.up * horizontalInput * Config.Turn * 100 * TurnMultiplyer);
            }

            _carBody.MoveRotation(Quaternion.Slerp(_carBody.rotation, 
                Quaternion.FromToRotation(_carBody.transform.up, Vector3.up) * _carBody.transform.rotation, 0.02f));

            _rbSphere.velocity = Vector3.Lerp(_rbSphere.velocity, 
                _rbSphere.velocity + Vector3.down * Config.Gravity, Time.deltaTime * Config.Gravity);
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
                Quaternion.Euler(Mathf.Lerp(0, -5, CarVelocity.z / Config.MaxSpeed), _bodyMesh.localRotation.eulerAngles.y,
                Mathf.Clamp(DesiredTurning * _inputSystem.HorizontalInput, -Config.BodyTilt, Config.BodyTilt)), 0.05f);
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
        //debug gizmos
        _radius = _rbSphere.GetComponent<SphereCollider>().radius;
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

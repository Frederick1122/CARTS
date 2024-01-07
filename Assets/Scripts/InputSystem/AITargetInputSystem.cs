using UnityEngine;

[RequireComponent(typeof(ITargetHolder))]
public class AITargetInputSystem : MonoBehaviour, IInputSystem
{
    public bool IsActive { get; set; }
    public float VerticalInput => _vertInp;
    public float HorizontalInput => _horInp;
    public float BrakeInput => _brInp;

    private float brakeAngle = 30f;

    private CarController _controller;
    private CarConfig _config;

    private float _vertInp;
    private float _horInp;
    private float _brInp;
    private Transform _target;


    private void Start()
    {
        _controller = GetComponent<CarController>();
        _target = GetComponent<ITargetHolder>().Target;

        _config = _controller.Config;
    }

    private void Update()
    {
        if (!IsActive)
            return;

        ReadInput();
    }

    public void ReadInput()
    {
        float reachedTargetDistance = 1f;
        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        Vector3 dirToMovePosition = (_target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToMovePosition);
        float angleToMove = Vector3.Angle(transform.forward, dirToMovePosition);
        if (angleToMove > brakeAngle)
        {
            if (_controller.CarVelocity.z > 15)
                _brInp = 1;
            else
                _brInp = 0;
        }
        else
            _brInp = 0;

        if (distanceToTarget > reachedTargetDistance)
        {

            if (dot > 0)
            {
                _vertInp = 1f;

                float stoppingDistance = 5f;
                if (distanceToTarget < stoppingDistance)
                    _brInp = 1;
                else
                    _brInp = 0;
            }
            else
            {
                float reverseDistance = 5f;
                if (distanceToTarget > reverseDistance)
                    _vertInp = 1f;
                else
                    _vertInp = -1f;
            }

            float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

            if (angleToDir > 0)
                _horInp = 1f * _config.turnCurve.Evaluate(_controller.DesiredTurning / 90);
            else
                _horInp = -1f * _config.turnCurve.Evaluate(_controller.DesiredTurning / 90);
        }
        else
        {
            if (_controller.CarVelocity.z > 1f)
                _brInp = -1f;
            else
                _brInp = 0f;

            _horInp = 0f;
        }
    }
}

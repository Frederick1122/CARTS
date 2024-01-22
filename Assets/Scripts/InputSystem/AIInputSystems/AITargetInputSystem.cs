using Cars;
using Cars.Controllers;
using ConfigScripts;
using UnityEngine;

public class AITargetInputSystem : MonoBehaviour, IInputSystem
{
    public bool IsActive { get; set; }
    public float VerticalInput => _vertInp;
    public float HorizontalInput => _horInp;
    public float BrakeInput => _brInp;

    protected float brakeAngle = 30f;

    protected CarController _controller;
    protected CarConfig _config;

    protected float _vertInp;
    protected float _horInp;
    protected float _brInp;
    protected Transform _target;

    protected virtual void Start()
    {
        _controller = GetComponent<CarController>();
        _target = GetComponent<ITargetHolder>().Target;

        _config = _controller.Config;
    }

    protected void Update()
    {
        if (!IsActive)
            return;

        ReadInput();
    }

    public virtual void Init(CarPresetConfig presetConfig, CarPrefabData prefabData)
    {
        
    }

    public virtual void ReadInput()
    {
        var reachedTargetDistance = 1f;
        var distanceToTarget = Vector3.Distance(transform.position, _target.position);
        var dirToMovePosition = (_target.position - transform.position).normalized;
        var dot = Vector3.Dot(transform.forward, dirToMovePosition);
        var angleToMove = Vector3.Angle(transform.forward, dirToMovePosition);
        
        if (angleToMove > brakeAngle)
            _brInp = _controller.CarVelocity.z > 15 ? 1 : 0;
        else
            _brInp = 0;

        if (distanceToTarget > reachedTargetDistance)
        {
            if (dot > 0)
            {
                _vertInp = 1f;

                var stoppingDistance = 5f;
                if (distanceToTarget < stoppingDistance)
                    _brInp = 1;
                else
                    _brInp = 0;
            }
            else
            {
                var reverseDistance = 5f;
                if (distanceToTarget > reverseDistance)
                    _vertInp = 1f;
                else
                    _vertInp = -1f;
            }

            var angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);
            if (angleToDir > 0)
                _horInp = 1f * _config.turnCurve.Evaluate(_controller.DesiredTurning / 90);
            else
                _horInp = -1f * _config.turnCurve.Evaluate(_controller.DesiredTurning / 90);
        }
        else
        {
            _brInp = _controller.CarVelocity.z > 1f ? -1f : 0f;
            _horInp = 0f;
        }
    }
}

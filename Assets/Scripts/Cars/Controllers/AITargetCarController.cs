using Cars.Controllers;
using Cars.InputSystem;
using ConfigScripts;
using UnityEngine;

public class AITargetCarController : CarController
{
    private Transform _target;
    private WaypointProgressTracker _waypointProgressTracker;

    public override float GetPassedDistance() => _waypointProgressTracker.GetPassedDistance();

    public override void Init(IInputSystem inputSystem, CarConfig carConfig, CarPresetConfig carPresetConfig,
        ITargetHolder targetHolder)
    {
        base.Init(inputSystem, carConfig, carPresetConfig, targetHolder);

        _target = targetHolder.Target;
        _waypointProgressTracker = targetHolder as WaypointProgressTracker;
    }

    protected override void CalculateDesiredAngle()
    {
        Vector3 aimedPoint = _target.position;
        aimedPoint.y = transform.position.y;

        Vector3 aimedDir = (aimedPoint - transform.position).normalized;
        Vector3 myDir = transform.forward;
        myDir.Normalize();

        DesiredTurning = Mathf.Abs(Vector3.Angle(myDir, Vector3.ProjectOnPlane(aimedDir, transform.up)));
    }
}

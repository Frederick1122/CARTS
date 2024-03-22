using Cars.Controllers;
using Cars.InputSystem;
using Cars.Tools;
using ConfigScripts;
using Managers;
using UnityEngine;

public class AITargetCarController : CarController
{
    private Transform _target;
    private float _additionalModifier;

    public override void Init(IInputSystem inputSystem, CarConfig carConfig, 
        CarPresetConfig carPresetConfig, CarCollisionDetection carCollisionDetection, 
        ITargetHolder targetHolder = null)
    {
        base.Init(inputSystem, carConfig, carPresetConfig, carCollisionDetection, targetHolder);
        _target = targetHolder.Target;
    }

    public override void SetUpCharacteristic()
    {
        _maxSpeed = Config.maxSpeedLevels[0].Value;
        _acceleration = Config.accelerationLevels[0].Value;
        _turnSpeed = Config.turnLevels[0].Value;
    }

    public void SetAdditionalModifier(float additionalModifier)
    {
        _additionalModifier = additionalModifier;
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

    protected override int GetAdditionalSpeedModifier()
    {
        return (int)_additionalModifier;
    }
}
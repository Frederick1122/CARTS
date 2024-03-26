using Cars.Controllers;
using Cars.InputSystem;
using Cars.Tools;
using ConfigScripts;
using Managers;
using UnityEngine;

public class PlayerCarController : CarController
{
    public override void Init(IInputSystem inputSystem, CarConfig carConfig, 
        CarPresetConfig carPresetConfig, CarCollisionDetection collisionDetection, 
        ITargetHolder targetHolder = null)
    {
        base.Init(inputSystem, carConfig, carPresetConfig, collisionDetection, targetHolder);
        _camera.gameObject.SetActive(true);
    }

    public override void SetUpCharacteristic()
    {
        var speedLvl = PlayerManager.Instance.GetEquippedCarCharacteristicLevel(ModificationType.MaxSpeed);
        var turnLvl = PlayerManager.Instance.GetEquippedCarCharacteristicLevel(ModificationType.Turn);
        var accelerationLvl = PlayerManager.Instance.GetEquippedCarCharacteristicLevel(ModificationType.Acceleration);

        _maxSpeed = Config.maxSpeedLevels[speedLvl].Value;
        _turnSpeed = Config.turnLevels[turnLvl].Value;
        _acceleration = Config.accelerationLevels[accelerationLvl].Value;
    }
    
    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"{CarVelocity.magnitude}");
    }

    protected override void CalculateDesiredAngle() =>
        DesiredTurning = Config.bodyTilt;
}

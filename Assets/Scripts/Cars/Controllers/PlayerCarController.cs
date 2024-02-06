using Cars.Controllers;
using Cars.InputSystem;
using ConfigScripts;
using Managers;

public class PlayerCarController : CarController
{
    private WaypointProgressTracker _waypointProgressTracker;

    public override float GetPassedDistance()
    {
        if (_waypointProgressTracker != null)
            return _waypointProgressTracker.GetPassedDistance();

        return 0;
    }

    public override void Init(IInputSystem inputSystem, CarConfig carConfig, CarPresetConfig carPresetConfig,
        ITargetHolder targetHolder = null)
    {
        base.Init(inputSystem, carConfig, carPresetConfig, targetHolder);
        _waypointProgressTracker = targetHolder as WaypointProgressTracker;
        _camera.gameObject.SetActive(true);
    }

    public override void SetUpCharacteristic()
    {
        var speedLvl = PlayerManager.Instance.GetEquippedCarCharacteristicLevel(ModificationType.MaxSpeed);
        var turnLvl = PlayerManager.Instance.GetEquippedCarCharacteristicLevel(ModificationType.Turn);
        var accelerationLvl = PlayerManager.Instance.GetEquippedCarCharacteristicLevel(ModificationType.Acceleration);

        _maxSpeed = Config.maxSpeedLevels[speedLvl];
        _turnSpeed = Config.turnLevels[speedLvl];
        _acceleration = Config.accelerationLevels[speedLvl];
    }

    protected override void CalculateDesiredAngle() =>
        DesiredTurning = Config.bodyTilt;
}

using Cars.Controllers;
using ConfigScripts;

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

    protected override void CalculateDesiredAngle()
    {
        DesiredTurning = Config.bodyTilt;
    }
}

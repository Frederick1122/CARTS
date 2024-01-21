using Cars.Controllers;
using ConfigScripts;

public class PlayerCarController : CarController
{
    public override void Init(IInputSystem inputSystem, CarConfig carConfig, CarPresetConfig carPresetConfig,
        ITargetHolder targetHolder = null)
    {
        base.Init(inputSystem, carConfig, carPresetConfig, targetHolder);
        _camera.gameObject.SetActive(true);
    }

    protected override void CalculateDesiredAngle()
    {
        DesiredTurning = Config.bodyTilt;
    }
}

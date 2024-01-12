public class PlayerCarController : CarController
{
    protected override void CalculateDesiredAngle()
    {
        DesiredTurning = Config.bodyTilt;
    }
}

public class FreeRideInputSystem : PlayerInputSystem
{
    private void Start()
    {
        _vertInput = 1;
    }

    protected override void OnVerticalAxisChange(float value) =>
            _vertInput = 1;

    protected override void OnHandBrakeAxisChange(float value) =>
        _handBrInput = 0;
}

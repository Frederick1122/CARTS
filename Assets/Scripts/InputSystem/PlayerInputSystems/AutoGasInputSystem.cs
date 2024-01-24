public class AutoGasInputSystem : PlayerInputSystem
{
    private void Start()
    {
        _vertInput = 1;
    }

    protected override void OnVerticalAxisChange(float value)
    {
        if (value >= 0)
            _vertInput = 1;
        else
            _vertInput = value;
    }

    protected override void OnHandBrakeAxisChange(float value) =>
        _handBrInput = 0;
}

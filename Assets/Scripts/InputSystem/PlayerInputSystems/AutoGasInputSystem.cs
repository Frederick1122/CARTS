namespace Cars.InputSystem.Player
{
    public class AutoGasInputSystem : PlayerInputSystem
    {
        private void Start() =>
            _vertInput = 1;

        protected override void OnVerticalAxisChange(float value) =>
            _vertInput = value >= 0 ? 1 : value;

        protected override void OnHandBrakeAxisChange(float value) =>
            _handBrInput = 0;
    }
}

namespace Cars.InputSystem.Player
{
    public interface IButtonsInput
    {
        public RaceButton ForwardButton { get; }
        public RaceButton HandBrakeButton { get; }

        public RaceButton RightButton { get; }
        public RaceButton LeftButton { get; }
        public RaceButton BackwardButton { get; }
    }
}

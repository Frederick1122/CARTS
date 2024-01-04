public interface IInputSystem
{
    public bool IsActive { get; set; }

    public float VerticalInput { get; }
    public float HorizontalInput { get;}
    public float BrakeInput { get; }

    void ReadInput();
}

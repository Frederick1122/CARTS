using Cars;
using ConfigScripts;

public interface IInputSystem
{
    public bool IsActive { get; set; }

    public void Init(CarPresetConfig presetConfig, CarPrefabData prefabData) { }

    public float VerticalInput { get; }
    public float HorizontalInput { get; }
    public float BrakeInput { get; }
}

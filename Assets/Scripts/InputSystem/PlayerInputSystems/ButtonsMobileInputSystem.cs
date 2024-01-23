using UI;
using UI.Windows.RaceLoadOut;
using UnityEngine;

public class ButtonsMobileInputSystem : MonoBehaviour, IInputSystem
{
    public bool IsActive { get; set; }
    public float VerticalInput => _vertInp;
    public float HorizontalInput => _horInp;
    public float BrakeInput => _brInp;

    private float _vertInp;
    private float _horInp;
    private float _brInp;
    private IButtonsInput _buttonController;

    private void Start()
    {
        var controller = (RaceLoadoutController)RaceUIManager.Instance.ShowWindow(typeof(RaceLoadoutController));
        Init(controller.GetButtons());
    }

    public void Init(IButtonsInput buttonController)
    {
        _buttonController = buttonController;
        _buttonController.RightButton.OnDown += TurnRight;
        _buttonController.LeftButton.OnDown += TurnLeft;
        _buttonController.BrakeButton.OnDown += Brake;

        _buttonController.RightButton.OnUp += StopTurn;
        _buttonController.LeftButton.OnUp += StopTurn;
        _buttonController.BrakeButton.OnUp += StopBrake;
    }

    private void Update()
    {
        if (!IsActive)
            return;

        ReadInput();
    }

    private void OnDestroy()
    {
        if (_buttonController == null)
            return;

        _buttonController.RightButton.OnDown -= TurnRight;
        _buttonController.LeftButton.OnDown -= TurnLeft;
        _buttonController.BrakeButton.OnDown -= Brake;

        _buttonController.RightButton.OnUp -= StopTurn;
        _buttonController.LeftButton.OnUp -= StopTurn;
        _buttonController.BrakeButton.OnUp -= StopBrake;
    }

    public void ReadInput()
    {
        if (_vertInp >= 0)
            _vertInp = 1;
    }

    public void TurnRight() =>
        _horInp = 1;

    public void TurnLeft() =>
        _horInp = -1;

    public void StopTurn() =>
        _horInp = 0;

    public void Brake() =>
        _vertInp = -1;

    public void StopBrake() =>
        _vertInp = 1;

}

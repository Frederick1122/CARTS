using UnityEngine;

public class MobileAutoGasInputSystem : MonoBehaviour, IInputSystem
{
    public bool IsActive { get; set; }

    public float VerticalInput => _vertInp;
    public float HorizontalInput => _horInp;
    public float BrakeInput => _brInp;

    private float _vertInp;
    private float _horInp;
    private float _brInp;

    private bool _backInput = false;

    public void ReadInput()
    {
        if (!IsActive)
            return;

        if (_backInput)
            _vertInp = -1;
        else
            _vertInp = 1;
    }

    public void ChangeHorizontalInput(float input) =>
        _horInp = Mathf.Clamp(input, - 1, 1);

    public void BackInput(bool needBreak) =>
        _backInput = needBreak;
}

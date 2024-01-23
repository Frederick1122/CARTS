using UnityEngine;

public class KeyBoardInputSystem : MonoBehaviour, IInputSystem
{
    private const string VERTICAL_AXIS = "Vertical";
    private const string HORIZONTAl_AXIS = "Horizontal";
    private const string BRAKE_AXIS = "Jump";

    [SerializeField] private bool _autoGas = true;

    public bool IsActive { get; set; }
    public float VerticalInput => _vertInp;
    public float HorizontalInput => _horInp;
    public float BrakeInput => _brInp;

    private float _vertInp;
    private float _horInp;
    private float _brInp;

    private void Update()
    {
        if (!IsActive)
            return;

        ReadInput();
    }

    public void ReadInput()
    {
        _vertInp = Input.GetAxis(VERTICAL_AXIS);
        _horInp = Input.GetAxis(HORIZONTAl_AXIS);
        _brInp = Input.GetAxis(BRAKE_AXIS);

        if (_autoGas && _vertInp >= 0)
            _vertInp = 1;
    }
}

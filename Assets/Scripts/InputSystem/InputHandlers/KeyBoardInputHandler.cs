using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInputHandler : MonoBehaviour, IInputHandler
{
    private const string VERTICAL_AXIS = "Vertical";
    private const string HORIZONTAl_AXIS = "Horizontal";
    private const string BRAKE_AXIS = "Jump";

    public bool IsActive { get; set; } = false;

    public event Action<float> OnVerticalAxisChange = delegate { };
    public event Action<float> OnHorizontalAxisChange = delegate { };
    public event Action<float> OnHandBrakeAxisChange = delegate { };

    private float _vertA = 0;
    private float _horA = 0;
    private float _brA = 0;

    private void Update()
    {
        if (!IsActive)
            return;

        var vertA = Input.GetAxis(VERTICAL_AXIS);
        var horA = Input.GetAxis(HORIZONTAl_AXIS);
        var brA = Input.GetAxis(BRAKE_AXIS);

        if (_vertA != vertA)
        {
            _vertA = vertA;
            OnVerticalAxisChange?.Invoke(_vertA);
        }

        if (_horA != horA)
        {
            _horA = horA;
            OnHorizontalAxisChange?.Invoke(_horA);
        }

        if (_brA != brA)
        {
            _brA = brA;
            OnHandBrakeAxisChange?.Invoke(_brA);
        }
    }
}

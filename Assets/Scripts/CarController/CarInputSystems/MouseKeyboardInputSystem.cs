using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class MouseKeyboardInputSystem : CarInputSystem
{
    private const string HORIZONAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    [SerializeField] private bool _permanentGas = true;

    protected override void CheckInput()
    {
        float gasInput = Input.GetAxis(VERTICAL_AXIS);
        float steeringInput = Input.GetAxis(HORIZONAL_AXIS);

        float g = Input.GetAxisRaw(VERTICAL_AXIS);
        float s = Input.GetAxisRaw(HORIZONAL_AXIS);

        IsForwardButton = g > 0;
        IsBackWardButton = g < 0;
        IsRightButton = s > 0;
        IsLeftButton = s < 0;
        IsBraking = Input.GetKey(KeyCode.Space);

        if (gasInput >= 0 && _permanentGas)
        {
            gasInput = 1;
            IsForwardButton = true;
        }
        if (IsBackWardButton)
        {
            gasInput = -1;
        }

        GasInput = gasInput;
        SteeringInput = steeringInput;

        if (Input.GetKeyDown(KeyCode.R))
            ResetCarCall();
        
    }
}

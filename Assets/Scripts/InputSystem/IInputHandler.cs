using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    public bool IsActive { get; set; }

    public event Action<float> OnVerticalAxisChange;
    public event Action<float> OnHorizontalAxisChange;

    public event Action<float> OnHandBrakeAxisChange;
}

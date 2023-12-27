using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputSystem
{
    public bool IsActive { get; set; }

    public float VerticalInput { get; }
    public float HorizontalInput { get;}
    public float BrakeInput { get; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : CarController
{
    protected override void CalculateDesiredAngle()
    {
        DesiredTurning = Config.BodyTilt;
    }
}

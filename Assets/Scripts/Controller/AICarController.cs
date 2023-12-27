using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : CarController
{
    [SerializeField] private Transform _target;

    protected override void CalculateDesiredAngle()
    {
        Vector3 aimedPoint = _target.position;
        aimedPoint.y = transform.position.y;
        Vector3 aimedDir = (aimedPoint - transform.position).normalized;
        Vector3 myDir = transform.forward;
        myDir.Normalize();
        DesiredTurning = Mathf.Abs(Vector3.Angle(myDir, Vector3.ProjectOnPlane(aimedDir, transform.up)));
    }
}

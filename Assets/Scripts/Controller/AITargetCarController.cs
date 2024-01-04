using UnityEngine;

[RequireComponent(typeof(ITargetHolder))]
public class AITargetCarController : CarController
{
    private Transform _target;

    public override void Init(IInputSystem inputSystem)
    {
        base.Init(inputSystem);

        _target = GetComponent<ITargetHolder>().Target;
    }

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

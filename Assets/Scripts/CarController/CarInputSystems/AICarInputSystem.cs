using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AICarInputSystem : CarInputSystem
{
    [SerializeField] private float _maxCheckPointOffset = 2;

    private Transform _currentWayPoint;
    private bool _isInBrakeZone;
    private CarController _carController;
    private CarConfig _carConfig;

    private Vector3 _rndWayPointOffset = Vector3.zero;

    public override void Init(List<CheckPoint> checkPoints)
    {
        base.Init(checkPoints);
        _currentWayPoint = _checkPoints[_curCheckPoint].GetWayPoint();
        _carController = GetComponent<CarController>();
        _carConfig = _carController.CarConfig;
    }

    private void OnDestroy()
    {
        UnSubOnCheckPoints();
    }

    protected override void ChangeCheckPoint(CheckPoint cur, Collider col)
    {
        base.ChangeCheckPoint(cur, col);

        if (col.gameObject != gameObject)
            return;

        if (_curCheckPoint >= _checkPoints.Count)
            return;

        _currentWayPoint = _checkPoints[_curCheckPoint].GetWayPoint();
        _rndWayPointOffset = new Vector3(Random.Range(-_maxCheckPointOffset, _maxCheckPointOffset), 0,
                                            Random.Range(-_maxCheckPointOffset, _maxCheckPointOffset));
    }

    public bool BrakeZoneInteract(bool cond) =>
        _isInBrakeZone = cond;

    protected override void CheckInput()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        float currentAngle = Vector3.SignedAngle(fwd, (_currentWayPoint.position + _rndWayPointOffset) - transform.position, Vector3.up);

        float gasInput = Mathf.Clamp01(1f - Mathf.Abs(_carController.CarSpeed * 0.02f * currentAngle) / 
            _carConfig.maxSteeringAngle);
        currentAngle = Mathf.Clamp(currentAngle / _carConfig.maxSteeringAngle, -1, 1);

        if (_isInBrakeZone)
        {
            gasInput = -gasInput * (Mathf.Clamp01(_carController.CarSpeed / _carConfig.maxSpeed) * 2 - 1f);
        }
        
        GasInput = gasInput;
        SteeringInput = currentAngle;

        IsForwardButton = GasInput > 0;
        IsBackWardButton = GasInput < 0;
        IsRightButton = SteeringInput > 0;
        IsLeftButton = SteeringInput < 0;
    }
}

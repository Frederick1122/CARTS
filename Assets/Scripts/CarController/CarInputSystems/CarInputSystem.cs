using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarInputSystem : MonoBehaviour
{
    public event Action OnNeedToResetCar;

    public float GasInput { get; protected set; } = 0;
    public float SteeringInput { get; protected set; } = 0;
    public bool IsBraking { get; protected set; } = false;

    public bool IsLeftButton { get; protected set; } = false;   
    public bool IsRightButton { get; protected set; } = false;   
    public bool IsForwardButton { get; protected set; } = false;   
    public bool IsBackWardButton { get; protected set; } = false;     

    public CheckPoint LastReachedCheckPoint { get; protected set; }

    [SerializeField] protected List<CheckPoint> _checkPoints = new();
    protected int _curCheckPoint = 0;

    public virtual void Init(List<CheckPoint> checkPoints)
    {
        for (int i = 0; i < checkPoints.Count; i++)
            _checkPoints.Add(checkPoints[i]);

        SubOnCheckPoints();
    }

    protected abstract void CheckInput();

    protected virtual void Update() =>
        CheckInput();

    protected virtual void SubOnCheckPoints()
    {
        for (int i = 0; i < _checkPoints.Count; i++)
            _checkPoints[i].OnCheckPointReached += ChangeCheckPoint;
    }

    protected virtual void UnSubOnCheckPoints()
    {
        for (int i = 0; i < _checkPoints.Count; i++)
            _checkPoints[i].OnCheckPointReached -= ChangeCheckPoint;
    }

    protected virtual void ChangeCheckPoint(CheckPoint cur, Collider other)
    {
        if (other.gameObject != gameObject)
            return;

        if (_curCheckPoint >= _checkPoints.Count)
            return;

        if (cur.ID == _checkPoints[_curCheckPoint].ID)
        {
            LastReachedCheckPoint = cur;
            _curCheckPoint++;
        }
    }

    protected void ResetCarCall()
    {
        GasInput = 0;
        SteeringInput = 0;
        IsBraking = false;

        IsLeftButton = false;
        IsRightButton = false;
        IsForwardButton = false;
        IsBackWardButton = false;

        OnNeedToResetCar?.Invoke();
    }
}

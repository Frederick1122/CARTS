using System;
using System.Collections.Generic;
using UnityEngine;

public class ProAITargetInputSystem : AITargetInputSystem
{
    [SerializeField] private Transform[] _rayPoses = new Transform[4];
    [SerializeField] private float _rayLength = 5;
    [SerializeField] private float _backKoef = 0.1f;

    private RaycastHit _hitL1, _hitL2, _hitR2, _hitR1;
    private Ray _rayL1, _rayL2, _rayR2, _rayR1;

    protected override void Start()
    {
        base.Start();
    }

    public override void ReadInput()
    {
        base.ReadInput();

        ReadRayInput();
    }


    private void ReadRayInput()
    {
        float mindist = _rayLength;
        float horInpAfter = _horInp;

        _rayL1 = new(_rayPoses[0].position, _rayPoses[0].forward * _rayLength);
        if (Physics.Raycast(_rayL1, out _hitL1, _rayLength))
        {
            if (mindist >= _hitL1.distance)
            {
                horInpAfter = 1;
                mindist = _hitL1.distance;
            }
        }

        _rayL2 = new(_rayPoses[1].position, _rayPoses[1].forward * _rayLength);
        if (Physics.Raycast(_rayL2, out _hitL2, _rayLength))
        {
            if (mindist >= _hitL2.distance)
            {
                horInpAfter = 0.5f;
                mindist = _hitL2.distance;
            }
        }

        _rayR2 = new(_rayPoses[2].position, _rayPoses[2].forward * _rayLength);
        if (Physics.Raycast(_rayR2, out _hitR2, _rayLength))
        {
            if (mindist >= _hitR2.distance)
            {
                horInpAfter = -0.5f;
                mindist = _hitR2.distance;
            }
        }

        _rayR1 = new(_rayPoses[3].position, _rayPoses[3].forward * _rayLength);
        if (Physics.Raycast(_rayR1, out _hitR1, _rayLength))
        {
            if (mindist >= _hitR1.distance)
            {
                horInpAfter = -1;
                mindist = _hitR1.distance;
            }
        }

        _vertInp = InterpolateRayDistance(mindist);
        _horInp = Mathf.Clamp(horInpAfter + _horInp, -1, 1);

    }

    private float InterpolateRayDistance(float dist)
    {
        if (dist <= _backKoef)
            return -1;

        if (dist == _rayLength)
            return _vertInp;

        float res = dist / _rayLength;

        return res;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(_rayPoses[0].position, _rayPoses[0].forward * _rayLength);
            Gizmos.DrawRay(_rayPoses[1].position, _rayPoses[1].forward * _rayLength);
            Gizmos.DrawRay(_rayPoses[2].position, _rayPoses[2].forward * _rayLength);
            Gizmos.DrawRay(_rayPoses[3].position, _rayPoses[3].forward * _rayLength);
        }
    }
#endif
}

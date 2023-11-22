using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointsHolder : MonoBehaviour
{
    public readonly List<CheckPoint> Points = new();


    private void Awake()
    {
        GetAllCheckPoints();
    }

    private void GetAllCheckPoints()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out CheckPoint point))
            {
                point.SetID(Points.Count);
                Points.Add(point);
            }
        }
    }
}

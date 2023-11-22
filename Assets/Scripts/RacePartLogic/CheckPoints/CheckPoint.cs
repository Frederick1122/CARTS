using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public event Action<CheckPoint, Collider> OnCheckPointReached;

    public int ID { get; private set; }

    [SerializeField] private Transform _wayPoint;

    public void SetID(int id) =>
      ID = id;

    public Transform GetWayPoint()
    {
        return _wayPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnCheckPointReached?.Invoke(this, other);
    }
}

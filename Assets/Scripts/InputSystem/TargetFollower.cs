using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// test script
public class TargetFollower : MonoBehaviour, ITargetHolder
{
    [field:SerializeField] public Transform Target { get ; set; }

    [SerializeField] private Transform _follower;

    private void Update()
    {
        Target.position = _follower.position;
    }
}

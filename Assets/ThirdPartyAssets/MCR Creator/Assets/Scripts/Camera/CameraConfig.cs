using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/Camera Config")]
public class CameraConfig : ScriptableObject
{
    public float Distance = 10.0f;
    public float Height = 5.0f;
    public float RotationDamping = 7;
    public float HeightDamping = 2;
}

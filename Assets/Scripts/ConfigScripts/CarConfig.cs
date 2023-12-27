using UnityEngine;

[CreateAssetMenu(fileName = "CarConfig", menuName = "Configs/Car Config")]
public class CarConfig : ScriptableObject
{
    [Header("Characteristic")]
    public float MaxSpeed = 100;
    public float Accelaration = 10;
    public float Turn = 3; 
    public float Gravity = 7f; 
    public float Downforce = 5f;
    public bool AirControl = false;

    [Header("Physics")]
    public AnimationCurve FrictionCurve;
    public AnimationCurve TurnCurve;
    public PhysicMaterial FrictionMaterial;

    [Header("Visual")]
    [Range(0, 10)] public float BodyTilt;
}

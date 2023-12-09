using UnityEngine;

[CreateAssetMenu(fileName = "CarConfig", menuName = "Configs/Car Config")]
public class CarConfig : ScriptableObject
{
    public int weight = 1500;

    [Space(10)]
    [Range(20, 190)]
    public int maxSpeed = 90; //The maximum speed that the car can reach in km/h.
    [Range(10, 120)]
    public int maxReverseSpeed = 45; //The maximum speed that the car can reach while going on reverse in km/h.
    [Range(1, 20)]
    public int accelerationMultiplier = 2; // How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.

    [Space(10)]
    [Range(10, 45)]
    public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
    [Range(0.1f, 1f)]
    public float steeringSpeed = 0.5f; // How fast the steering wheel turns.
    [Range(0.1f, 11)]
    public float speedLossByAngle = 0.5f;

    [Space(10)]
    [Range(100, 1000)]
    public int brakeForce = 350; // The strength of the wheel brakes.
    [Range(1, 10)]
    public int decelerationMultiplier = 2; // How fast the car decelerates when the user is not using the throttle.
    [Range(1, 10)]
    public int handbrakeDriftMultiplier = 5; // How much grip the car loses when the user hit the handbrake.
    [Range(0.1f, 10)]
    public float driftKoef = 1f;

    [Space(10)]
    public Vector3 bodyMassCenter;
}

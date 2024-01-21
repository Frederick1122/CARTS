using Cars.Controllers;
using UnityEngine;

public class StartRacePlace : MonoBehaviour
{
    [SerializeField] private WaypointCircuit _circuit;
    private CarController _car;

    public WaypointCircuit GetWaypointCircuit()
    {
        return _circuit;
    }
}

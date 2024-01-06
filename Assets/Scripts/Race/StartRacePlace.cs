using UnityEngine;

public class StartRacePlace : MonoBehaviour
{
    [SerializeField] private WaypointCircuit _circuit;
    private CarController _car;

    private void Awake()
    {
        if(_circuit == null)
            _circuit = (WaypointCircuit)FindFirstObjectByType(typeof(WaypointCircuit));
    }

    public void Init(CarController car)
    {
        _car = car;

        if (_car.TryGetComponent(out ICircuitHolder holder))
            holder.Circuit = _circuit;
    }
}

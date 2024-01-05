using ArcadeVP;
using UnityEngine;

public class WaypointProgressTracker : MonoBehaviour, ITargetHolder
{
    // This script can be used with any object that is supposed to follow a
    // route marked out by waypoints.
    //public float timeO;

    // This script manages the amount to look ahead along the route,
    // and keeps track of progress and laps.

    // these are public, readable by other objects - i.e. for an AI to know where to head!
    //public WaypointCircuit.RoutePoint TargetPoint { get; private set; }
    //public WaypointCircuit.RoutePoint SpeedPoint { get; private set; }
    public WaypointCircuit.RoutePoint ProgressPoint { get; private set; }

    [field: SerializeField] public Transform Target { get; set; }

    // A reference to the waypoint-based route we should follow
    [SerializeField] private WaypointCircuit _circuit;
    // The offset ahead along the route that the we will aim for
    [SerializeField] private float _lookAheadForTargetOffset = 5;
    // A multiplier adding distance ahead along the route to aim for, based on current speed
    [SerializeField] private float _lookAheadForTargetFactor = .1f;
    // whether to update the position smoothly along the route (good for curved paths) or just when we reach each waypoint.
    [SerializeField] private ProgressStyle _progressStyle = ProgressStyle.SmoothAlongRoute;

    // The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)
    private float _lookAheadForSpeedOffset = 50;
    // A multiplier adding distance ahead along the route for speed adjustments
    private float _lookAheadForSpeedFactor = .2f;
    // proximity to waypoint which must be reached to switch target to next waypoint : only used in PointToPoint mode.
    private float _pointToPointThreshold = 4;

    // The progress round the route, used in smooth mode.
    private float _progressDistance;
    // the current waypoint number, used in point-to-point mode.
    private int _progressNum;
    // Used to calculate current speed (since we may not have a rigidbody component)
    private Vector3 _lastPosition;
    // current speed of this object (calculated from delta since last frame)
    private float _speed;

    private CarController _controller;
    private IInputSystem _inputSystem;

    private void Start()
    {
        Reset();
        _controller = GetComponent<CarController>();
        _inputSystem = GetComponent<IInputSystem>();
    }

    // reset the object to sensible values
    public void Reset()
    {
        _progressDistance = 0;
        _progressNum = 0;
        if (_progressStyle == ProgressStyle.PointToPoint)
        {
            var point = _circuit.Waypoints[_progressNum];
            Target.SetPositionAndRotation(point.position, point.rotation);
        }
    }

    private void Update()
    {
        if (!_inputSystem.IsActive)
            return;

        ProgressPoint = _circuit.GetRoutePoint(_progressDistance);

        // get our current progress along the route
        Vector3 progressDelta = ProgressPoint.Position - transform.position;
        var dot = Vector3.Dot(progressDelta, ProgressPoint.Direction);

        if (_progressStyle == ProgressStyle.SmoothAlongRoute)
        {
            // determine the position we should currently be aiming for
            // (this is different to the current progress position, it is a a certain amount ahead along the route)
            // we use lerp as a simple way of smoothing out the speed over time.
            _speed = _controller.CarVelocity.z;

            WaypointCircuit.RoutePoint routePoint = _circuit.GetRoutePoint(
                _progressDistance + _lookAheadForTargetOffset + _lookAheadForTargetFactor * _speed);
            Target.SetPositionAndRotation(routePoint.Position, Quaternion.LookRotation(routePoint.Direction));

            if (dot < 0)
                _progressDistance += progressDelta.magnitude * 0.5f;
        }
        else
        {
            // point to point mode. Just increase the waypoint if we're close enough:
            Vector3 targetDelta = Target.position - transform.position;
            if (targetDelta.magnitude < _pointToPointThreshold)
                _progressNum = (_progressNum + 1) % _circuit.Waypoints.Length;

            var routePoint = _circuit.Waypoints[_progressNum];
            Target.SetPositionAndRotation(routePoint.position, routePoint.rotation);

            if (dot < 0)
                _progressDistance += progressDelta.magnitude;
        }

        _lastPosition = transform.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, Target.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_circuit.GetRoutePosition(_progressDistance), 0.2f);
            Gizmos.DrawLine(transform.position, _circuit.GetRoutePosition(_progressDistance));
            Gizmos.DrawLine(Target.position, Target.position + Target.forward);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(Target.position, 1);
        }
    }
#endif

    /* public void respawnOnRoad()
     {
         if ( Vector3.Distance( transform.position, circuit.GetRoutePosition(progressDistance)) > 15)
         {
             timeO += Time.deltaTime;
         }
         else
         {
             timeO = 0f;
         }
         if (timeO > 3)
         {
             transform.position = target.position + new Vector3(0, 1.5f, 0);
             timeO = 0f;
         }

     }*/
}


public enum ProgressStyle
{
    SmoothAlongRoute,
    PointToPoint,
}

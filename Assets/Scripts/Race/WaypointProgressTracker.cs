using Cars.Controllers;
using Cars.InputSystem;
using System;
using UnityEngine;

public class WaypointProgressTracker : MonoBehaviour, ITargetHolder, ICircuitHolder
{
    public event Action OnLapEndAction = delegate { };

    // Time for respawn
    [SerializeField] private float _timeToRespawn = 3;
    // The offset ahead along the route that the we will aim for
    [SerializeField] private float _lookAheadForTargetOffset = 20;
    // A multiplier adding distance ahead along the route to aim for, based on current speed
    [SerializeField] private float _lookAheadForTargetFactor = .1f;
    // whether to update the position smoothly along the route (good for curved paths) or just when we reach each waypoint.
    [SerializeField] private ProgressStyle _progressStyle = ProgressStyle.SmoothAlongRoute;

    public Transform Target { get; set; }
    public WaypointCircuit Circuit { get; set; }
    public WaypointCircuit.RoutePoint ProgressPoint { get; private set; }

    // The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)
    private float _lookAheadForSpeedOffset = 50;
    // A multiplier adding distance ahead along the route for speed adjustments
    private float _lookAheadForSpeedFactor = .2f;

    // proximity to waypoint which must be reached to switch target to next waypoint : only used in PointToPoint mode.
    private float _pointToPointThreshold = 4;
    // the current waypoint number, used in point-to-point mode.
    private int _progressNum;
    // The progress round the route, used in smooth mode.
    private float _progressDistance;

    private Vector3 _lastPosition;
    private float _speed;
    private float _currentRespawnTime;

    private float _lapDistance;
    private int _lapCount = 1;

    private CarController _controller;
    private IInputSystem _inputSystem;

    public void Init(CarController carController, IInputSystem inputSystem)
    {
        Reset();
        _controller = carController;
        _inputSystem = inputSystem;

        _lapDistance = Circuit.Length;
    }

    // reset the object to sensible values
    public void Reset()
    {
        _progressDistance = 0;
        _progressNum = 0;

        if (Target == null)
        {
            var newTargetObject = new GameObject
            {
                transform =
                {
                    name = "Target",
                    parent = transform.parent
                }
            };

            Target = newTargetObject.transform;
        }

        if (_progressStyle == ProgressStyle.PointToPoint)
        {
            var point = Circuit.Waypoints[_progressNum];
            Target.SetPositionAndRotation(point.position, point.rotation);
        }
    }

    public float GetPassedDistance()
    {
        return _progressDistance;
    }

    private void Update()
    {
        if (!_inputSystem.IsActive)
            return;

        ProgressPoint = Circuit.GetRoutePoint(_progressDistance);

        // get our current progress along the route
        var progressDelta = ProgressPoint.Position - transform.position;
        var dot = Vector3.Dot(progressDelta, ProgressPoint.Direction);

        switch (_progressStyle)
        {
            case ProgressStyle.SmoothAlongRoute:
            {
                // determine the position we should currently be aiming for
                // (this is different to the current progress position, it is a a certain amount ahead along the route)
                // we use lerp as a simple way of smoothing out the speed over time.
                _speed = _controller.CarVelocity.z;

                var routePoint = Circuit.GetRoutePoint(
                    _progressDistance + _lookAheadForTargetOffset + _lookAheadForTargetFactor * _speed);
                Target.SetPositionAndRotation(routePoint.Position, Quaternion.LookRotation(routePoint.Direction));

                if (dot < 0)
                    _progressDistance += progressDelta.magnitude * 0.5f;

                if (_progressDistance > _lapDistance * _lapCount)
                {
                    OnLapEndAction?.Invoke();
                    _lapCount++;
                }
                break;
            }

            case ProgressStyle.PointToPoint:
            {
                // point to point mode. Just increase the waypoint if we're close enough:
                var targetDelta = Target.position - transform.position;
                if (targetDelta.magnitude < _pointToPointThreshold)
                {
                    _progressNum = (_progressNum + 1) % Circuit.Waypoints.Length;
                    if (_progressNum == 0)
                        OnLapEndAction?.Invoke();
                }

                var routePoint = Circuit.Waypoints[_progressNum];
                Target.SetPositionAndRotation(routePoint.position, routePoint.rotation);

                if (dot < 0)
                    _progressDistance += progressDelta.magnitude;
                break;
            }
        }
        
        _lastPosition = transform.position;

        RespawnOnRoad();
    }

    private void RespawnOnRoad()
    {
        if (Vector3.Distance(transform.position, Circuit.GetRoutePosition(_progressDistance)) > 15
            || Mathf.Abs(_speed) <= 0.02f)
            _currentRespawnTime += Time.deltaTime;
        else
            _currentRespawnTime = 0f;

        if (_currentRespawnTime > _timeToRespawn)
        {
            gameObject.SetActive(false);

            Vector3 pos = Target.position;
            pos.y = 2;

            _controller.ResetCar(pos, Target.rotation);
            //transform.SetPositionAndRotation(pos, Target.rotation);
            _currentRespawnTime = 0f;

            gameObject.SetActive(true);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, Target.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Circuit.GetRoutePosition(_progressDistance), 0.2f);
            Gizmos.DrawLine(transform.position, Circuit.GetRoutePosition(_progressDistance));
            Gizmos.DrawLine(Target.position, Target.position + Target.forward);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(Target.position, 1);
        }
    }
#endif
}


public enum ProgressStyle
{
    SmoothAlongRoute,
    PointToPoint,
}
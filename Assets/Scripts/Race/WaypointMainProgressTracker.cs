using System;
using System.Collections.Generic;
using Cars.Controllers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Race
{
    public class WaypointMainProgressTracker : MonoBehaviour
    {
        public event Action<int> OnLapEndAction = delegate { };

        [SerializeField] private WaypointCircuit _circuit;
        [SerializeField] private ProgressStyle _progressStyle = ProgressStyle.SmoothAlongRoute;
        
        private bool _isRaceActive = false;
        private List<MainWaypoint> _mainWaypoints = new();

        public float GetPassedDistance(int idx)
        {
            return _mainWaypoints[idx].GetPassedDistance();
        }
        
        public void StartRace()
        {
            _isRaceActive = true;
        }

        public float GetLapDistance()
        {
            return _circuit.Length;
        }
        
        public void Init(List<CarController> cars)
        {
            var lapDistance = GetLapDistance();

            foreach (var mainWaypoint in _mainWaypoints) 
                mainWaypoint.Destroy();

            _mainWaypoints.Clear();
            
            for (int i = 0; i < cars.Count; i++)
            {
                if (_mainWaypoints.Count == i) 
                    _mainWaypoints.Add(new MainWaypoint());

                _mainWaypoints[i].Init(cars[i], _circuit, lapDistance, _progressStyle);
                var i1 = i;
                _mainWaypoints[i].OnLapEndAction += () => FinishLap(i1);
            }

            _isRaceActive = false;
        }

        public void Update()
        {
            if (!_isRaceActive)
                return;
            
            foreach (var waypointMain in _mainWaypoints) 
                waypointMain.Update();
        }

        private void FinishLap(int idx)
        {
            OnLapEndAction.Invoke(idx);
        }
        
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                foreach (var mainWaypoint in _mainWaypoints) 
                    mainWaypoint.OnDrawGizmos();
            }
        }
#endif
    }


    public class MainWaypoint
    {
        public event Action OnLapEndAction = delegate { };
        
        private WaypointCircuit _circuit;
        private ProgressStyle _progressStyle;
        private Transform _target;
        private CarController _carController;
        
        private int _progressNum;
        private float _progressDistance;
        private readonly float _pointToPointThreshold = 4; 
        private float _lapDistance;
        private int _lapCount = 1;

        public void Init(CarController carController, WaypointCircuit circuit,
            float lapDistance, ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute)
        {
            _carController = carController;
            _progressStyle = progressStyle;
            _circuit = circuit;
            _lapDistance = lapDistance;
            Reset();
        }
        
        public void Update()
        {
            var progressPoint = _circuit.GetRoutePoint(_progressDistance);

            // get our current progress along the route
            var progressDelta = progressPoint.Position - _carController.transform.position;
            var dot = Vector3.Dot(progressDelta, progressPoint.Direction);

            switch (_progressStyle)
            {
                case ProgressStyle.SmoothAlongRoute:
                {
                    // determine the position we should currently be aiming for
                    // (this is different to the current progress position, it is a a certain amount ahead along the route)
                    // we use lerp as a simple way of smoothing out the speed over time.
                    var routePoint = _circuit.GetRoutePoint(
                        _progressDistance);
                    _target.SetPositionAndRotation(routePoint.Position, Quaternion.LookRotation(routePoint.Direction));

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
                    var targetDelta = _target.position - _carController.transform.position;
                    if (targetDelta.magnitude < _pointToPointThreshold)
                    {
                        _progressNum = (_progressNum + 1) % _circuit.Waypoints.Length;
                        if (_progressNum == 0)
                            OnLapEndAction?.Invoke();
                    }

                    var routePoint = _circuit.Waypoints[_progressNum];
                    _target.SetPositionAndRotation(routePoint.position, routePoint.rotation);

                    if (dot < 0)
                        _progressDistance += progressDelta.magnitude;
                    break;
                }
            }
        }

        public void Destroy()
        {
            Object.Destroy(_target.gameObject);  
        }
        
        public void Reset()
        {
            _progressDistance = 0;
            _progressNum = 0;

            if (_target == null)
            {
                var newTargetObject = new GameObject
                {
                    transform =
                    {
                        name = "Target",
                        parent = _carController.transform.parent
                    }
                };

                _target = newTargetObject.transform;
            }

            if (_progressStyle == ProgressStyle.PointToPoint)
            {
                var point = _circuit.Waypoints[_progressNum];
                _target.SetPositionAndRotation(point.position, point.rotation);
            }
        }
        
        public float GetPassedDistance()
        {
            return _progressDistance;
        }
        
        
#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_carController.gameObject.transform.position, _target.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_circuit.GetRoutePosition(_progressDistance), 0.2f);
                Gizmos.DrawLine(_carController.gameObject.transform.position, _circuit.GetRoutePosition(_progressDistance));
                Gizmos.DrawLine(_target.position, _target.position + _target.forward);
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(_target.position, 1);
            }
        }
#endif
    }
}
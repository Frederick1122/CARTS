using System;
using UnityEngine;

public class WaypointCircuit : MonoBehaviour
{
    [SerializeField] private WaypointList _waypointList = new();
    [SerializeField] private bool _smoothRoute = true;
    [SerializeField] private float editorVisualisationSubsteps = 100;

    public Transform[] Waypoints
    {
        get { return _waypointList.Items; }
    }
    public float Length { get; private set; }

    private int _numPoints;
    private Vector3[] _points;
    private float[] _distances;

    //this being here will save GC allocs
    private int _p0n, _p1n, _p2n, _p3n;
    private Vector3 _p0, _p1, _p2, _p3;
    private float _interpolation;

    // Use this for initialization
    private void Awake()
    {
        if (Waypoints.Length > 1)
            CachePositionsAndDistances();

        _numPoints = Waypoints.Length;
    }

    public void SetUpWaypoints(int size, Transform[] points)
    {
        _waypointList.Items = new Transform[size];
        for (int i = 0; i < size; i++)
            _waypointList.Items[i] = points[i];
    }

    public RoutePoint GetRoutePoint(float dist)
    {
        // position and direction
        Vector3 p1 = GetRoutePosition(dist);
        Vector3 p2 = GetRoutePosition(dist + 0.1f);
        Vector3 delta = p2 - p1;
        return new RoutePoint(p1, delta.normalized);
    }


    public Vector3 GetRoutePosition(float dist)
    {
        int point = 0;

        if (Length == 0)
            Length = _distances[_distances.Length - 1];

        dist = Mathf.Repeat(dist, Length);

        while (_distances[point] < dist)
            ++point;

        // get nearest two points, ensuring points wrap-around start & end of circuit
        _p1n = ((point - 1) + _numPoints) % _numPoints;
        _p2n = point;

        // found point numbers, now find interpolation value between the two middle points
        _interpolation = Mathf.InverseLerp(_distances[_p1n], _distances[_p2n], dist);

        if (_smoothRoute)
        {
            // smooth catmull-rom calculation between the two relevant points

            // get indices for the surrounding 2 points, because
            // four points are required by the catmull-rom function
            _p0n = ((point - 2) + _numPoints) % _numPoints;
            _p3n = (point + 1) % _numPoints;

            // 2nd point may have been the 'last' point - a dupe of the first,
            // (to give a value of max track distance instead of zero)
            // but now it must be wrapped back to zero if that was the case.
            _p2n = _p2n % _numPoints;

            _p0 = _points[_p0n];
            _p1 = _points[_p1n];
            _p2 = _points[_p2n];
            _p3 = _points[_p3n];

            return CatmullRom(_p0, _p1, _p2, _p3, _interpolation);
        }
        else
        {
            // simple linear lerp between the two points:

            _p1n = ((point - 1) + _numPoints) % _numPoints;
            _p2n = point;

            return Vector3.Lerp(_points[_p1n], _points[_p2n], _interpolation);
        }
    }


    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
    {
        // comments are no use here... it's the catmull-rom equation.
        // Un-magic this, lord vector!
        return 0.5f *
               ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
    }

    private void CachePositionsAndDistances()
    {
        // transfer the position of each point and distances between points to arrays for
        // speed of lookup at runtime
        _points = new Vector3[Waypoints.Length + 1];
        _distances = new float[Waypoints.Length + 1];

        float accumulateDistance = 0;
        for (int i = 0; i < _points.Length; ++i)
        {
            var t1 = Waypoints[i % Waypoints.Length];
            var t2 = Waypoints[(i + 1) % Waypoints.Length];
            if (t1 != null && t2 != null)
            {
                Vector3 p1 = t1.position;
                Vector3 p2 = t2.position;
                _points[i] = Waypoints[i % Waypoints.Length].position;
                _distances[i] = accumulateDistance;
                accumulateDistance += (p1 - p2).magnitude;
            }
        }
    }

    [Serializable]
    public class WaypointList
    {
        public WaypointCircuit Circuit;
        public Transform[] Items = new Transform[0];
    }

    public readonly struct RoutePoint
    {
        public Vector3 Position { get; }
        public Vector3 Direction { get; }

        public RoutePoint(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() =>
        DrawGizmos(false);

    private void OnDrawGizmosSelected() =>
        DrawGizmos(true);

    private void DrawGizmos(bool selected)
    {
        _waypointList.Circuit = this;
        if (Waypoints.Length > 1)
        {
            _numPoints = Waypoints.Length;

            CachePositionsAndDistances();
            Length = _distances[_distances.Length - 1];

            Gizmos.color = selected ? Color.yellow : Color.yellow;
            Vector3 prev = Waypoints[0].position;
            if (_smoothRoute)
            {
                for (float dist = 0; dist < Length; dist += Length / editorVisualisationSubsteps)
                {
                    Vector3 next = GetRoutePosition(dist + 1);
                    Gizmos.DrawLine(prev, next);
                    prev = next;
                }
                Gizmos.DrawLine(prev, Waypoints[0].position);
            }
            else
            {
                for (int n = 0; n < Waypoints.Length; ++n)
                {
                    Vector3 next = Waypoints[(n + 1) % Waypoints.Length].position;
                    Gizmos.DrawLine(prev, next);
                    prev = next;
                }
            }
        }
        foreach (Transform waypoint in Waypoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(waypoint.position, 1f);
        }
    }
#endif
}

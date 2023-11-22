using System.Collections.Generic;
using UnityEngine;

public class RaceCamera : MonoBehaviour
{
    [SerializeField] private float _lerpTime = 3.5f;
    [SerializeField, Range(1.5f, 6)] private float _forwardDistance = 3;
    [SerializeField] private Vector3 _targetOffset = Vector3.zero;

    private GameObject _attachedVehicle;

    private float _distance = 2;
    private List<Vector2> _cameraPos = new();

    private float _accelerationEffect;

    private int _locationIndicator = 1;

    private Vector3 _newPos;
    private Transform _target;
    private GameObject _focusPoint;
    private RaceCameraSettings _settings;

    public void Init(GameObject attachedVehicle)
    {
        _attachedVehicle = attachedVehicle;
        _settings = _attachedVehicle.GetComponent<RaceCameraSettings>();

        _focusPoint = _attachedVehicle;
        _target = _focusPoint.transform;

        _distance = _settings.Distance;
        foreach (Vector2 pos in _settings.CameraPos)
            _cameraPos.Add(pos);
    }

    private void FixedUpdate()
    {
        UpdateCam();
    }

    private void CycleCamera()
    {
        if (_locationIndicator >= _cameraPos.Count - 1 || _locationIndicator < 0) 
            _locationIndicator = 0;
        else 
            _locationIndicator++;
    }

    private void UpdateCam()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleCamera();
        }

        _newPos = _target.position - (_target.forward * _cameraPos[_locationIndicator].x) + (_target.up * _cameraPos[_locationIndicator].y);

        _accelerationEffect = Mathf.Lerp(_accelerationEffect, 1 * 3.5f, 2 * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, _focusPoint.transform.GetChild(0).transform.position, _lerpTime * Time.deltaTime);

        _distance = Mathf.Pow(Vector3.Distance(transform.position, _newPos), _forwardDistance);

        transform.position = Vector3.MoveTowards(transform.position, _newPos, _distance * Time.deltaTime);

        transform.transform.localRotation = Quaternion.Lerp(transform.transform.localRotation, Quaternion.Euler(-_accelerationEffect, 0, 0), 5 * Time.deltaTime);

        transform.LookAt(_target.transform.position + _targetOffset);
    }
}

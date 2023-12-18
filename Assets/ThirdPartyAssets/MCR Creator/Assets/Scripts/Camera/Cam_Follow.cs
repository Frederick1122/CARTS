// Description : Cam_Follow.cs : use on camera to follow the cars
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Cam_Follow : MonoBehaviour
{
	[SerializeField] private CameraConfig _camConfig;
	[SerializeField] private bool _isPaused = false;
	[SerializeField] private bool _findTargetAutomaticly = false;

	// The target we are following
	private Transform _target;

	// The distance in the x-z plane to the target
	private float _distance = 10.0f;
	// the height we want the camera to be above the target
	private float _height = 5.0f;
	private float _rotationDamping;
	private float _heightDamping;

	// temp
	void Start()
	{
		AutoFindTarget();
		SetUp();
    }

	private void SetUp()
	{
		_distance = _camConfig.Distance;
		_height = _camConfig.Height;
		_rotationDamping = _camConfig.RotationDamping;
		_heightDamping = _camConfig.HeightDamping;
	}

	private void AutoFindTarget()
	{
		if (!_findTargetAutomaticly)
			return;

		GameObject[] arrCars = GameObject.FindGameObjectsWithTag("Car");

		foreach (GameObject carFind in arrCars)
		{                                                       // Put the car in the player order 1,2,3,4 on the array
			if (carFind.TryGetComponent(out CarController carController))
			{
				_target = carController.camTarget.transform;
				return;
			}
		}

	}

	// --> Init camera position when the scene starts
	public void InitCamera(CarController car, bool b_Splitscreen)
	{
		_target = car.camTarget.transform;
		if (b_Splitscreen)
		{
			Camera cam = GetComponent<Camera>();
			cam.rect = new Rect(new Vector2(0, 0), new Vector2(.5f, 1));
		}
	}

	void FixedUpdate()
	{
		if (!_isPaused && _target)
		{
			// Calculate the current rotation angles
			float wantedRotationAngle = _target.eulerAngles.y;
			float wantedHeight = _target.position.y + _height;

			float currentRotationAngle = transform.eulerAngles.y;
			float currentHeight = transform.position.y;


			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, _rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, _heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = _target.position;
			transform.position -= currentRotation * Vector3.forward * _distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

			// Always look at the target
			transform.LookAt(_target);
		}
	}

	// --> Pause the camera in Pause Mode
	public void Pause() =>
		_isPaused = !_isPaused;
	

	// --> Init camera position when the scene starts
	public void InitCameraHorizontal(CarController car, bool b_Splitscreen, string PlayerName)
	{
		_target = car.camTarget.transform;

		if (b_Splitscreen && PlayerName == "P1")
		{
			Camera cam = GetComponent<Camera>();
			cam.rect = new Rect(new Vector2(0, .5f), new Vector2(1, 1));
		}
		if (b_Splitscreen && PlayerName == "P2")
		{
			Camera cam = GetComponent<Camera>();
			cam.rect = new Rect(new Vector2(0, -.5f), new Vector2(1, 1));
		}
	}
}

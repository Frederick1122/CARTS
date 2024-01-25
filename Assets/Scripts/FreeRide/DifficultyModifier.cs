using Cars.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyModifier : MonoBehaviour
{
    [SerializeField] private int _levelToIncreaseDifficulty = 5;

    [Header("Modifiers")]
    [SerializeField, Range(0, 1)] private float _speedModifierByLevel = 0.1f;
    [SerializeField, Range(0, 1)] private float _accelerationModifierByLevel = 0.1f;

    private int _level = 0;

    private CarController _car;
    private FreeRideManager _manager;

    public void Init(CarController car, FreeRideManager manager)
    {
        _car = car;
        _manager = manager;

        _manager.OnResultUpdate += UpDifficulty;
    }

    private void OnDestroy()
    {
        _manager.OnResultUpdate -= UpDifficulty;
    }

    public void UpDifficulty(int res)
    {
        if (res % 5 != 0)
            return;

        _level++;
        _car.IncreaseModifier(_speedModifierByLevel, _accelerationModifierByLevel);
        Debug.Log($"Level up: {_level}");
    }
}

using Cars.Controllers;
using Race.RaceManagers;
using UnityEngine;

namespace FreeRide
{
    public class DifficultyModifier : MonoBehaviour
    {
        [SerializeField] private int _levelToIncreaseDifficulty = 5;

        [Header("Modifiers")]
        [SerializeField, Range(0, 1)] private float _speedModifierByLevel = 0.1f;
        [SerializeField, Range(0, 1)] private float _accelerationModifierByLevel = 0.1f;

        private int _level = 0;

        private CarController _car;
        private FreeRideState _state;

        public void Init(CarController car, FreeRideState state)
        {
            _car = car;
            _state = state;

            _state.OnResultUpdateAction += UpDifficulty;
        }

        private void OnDestroy()
        {
            _state.OnResultUpdateAction -= UpDifficulty;
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
}

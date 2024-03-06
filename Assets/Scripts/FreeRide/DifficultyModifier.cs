using Cars.Controllers;
using Race.RaceManagers;
using UnityEngine;

namespace FreeRide
{
    public class DifficultyModifier : MonoBehaviour
    {
        [Header("Modifiers")]
        [SerializeField] private AnimationCurve _speedModifire;
        [SerializeField] private AnimationCurve _accelerationModifier;

        private int _level = 0;

        private CarController _car;
        private FreeRideState _state;

        public void Init(CarController car, FreeRideState state)
        {
            _car = car;
            _state = state;

            _state.OnResultUpdateAction += UpDifficulty;
        }

        private void OnDestroy() => 
            _state.OnResultUpdateAction -= UpDifficulty;

        public void UpDifficulty(int res)
        {
            _level++;
            var speedMod = _speedModifire.Evaluate(_level);
            var accelerationMod = _accelerationModifier.Evaluate(_level);
            _car.ChangeModifier(speedMod, accelerationMod);
            Debug.Log($"Level up: {_level}");
        }
    }
}

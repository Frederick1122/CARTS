using System;
using Cars.Controllers;
using Race.RaceManagers;
using UnityEngine;

namespace FreeRide
{
    public class DifficultyModifier : MonoBehaviour
    {
        private int _level = 0;

        private FreeRideDifficultyModifierData _data;
        
        private CarController _car;
        private FreeRideState _state;

        public void Init(CarController car, FreeRideState state, FreeRideDifficultyModifierData data)
        {
            _car = car;
            _state = state;
            _data = data;

            _state.OnResultUpdateAction += UpDifficulty;
        }

        private void OnDestroy() => 
            _state.OnResultUpdateAction -= UpDifficulty;

        public void UpDifficulty(int res)
        {
            _level++;
            var speedMod = _data.speedModifier.Evaluate(_level);
            var accelerationMod = _data.accelerationModifier.Evaluate(_level);
            _car.ChangeModifier(speedMod, accelerationMod);
            Debug.Log($"Level up: {_level}");
        }
    }

    [Serializable]
    public class FreeRideDifficultyModifierData
    {
        [Header("Modifiers")]
        public AnimationCurve speedModifier;
        public AnimationCurve accelerationModifier;
    }
}

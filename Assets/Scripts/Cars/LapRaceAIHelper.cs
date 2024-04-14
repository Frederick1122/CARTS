using System.Collections.Generic;
using Cars.Controllers;
using Race;
using UnityEngine;

namespace Cars
{
    public class LapRaceAIHelper : MonoBehaviour
    {
        private float MIN_ALLOWED_DISTANCE = 10;
        private float MAX_ALLOWED_DISTANCE = 100;
        private float SPEED_MODIFIER = 25;

        private float NEGATIVE_MIN_ALLOWED_DISTANCE = 0;
        private float NEGATIVE_MAX_ALLOWED_DISTANCE = 50;
        private float NEGATIVE_SPEED_MODIFIER = 40;
        
        private WaypointMainProgressTracker _waypointMainProgressTracker;
        private List<AITargetCarController> _aiControllers = new();
        private List<float> _aiControllerModifiers = new(); // only for GUI

        private bool _isRaceActive = false;  

        public void Init(WaypointMainProgressTracker waypointMainProgressTracker, List<CarController> carControllers)
        {
            _waypointMainProgressTracker = waypointMainProgressTracker;
            _aiControllers.Clear();
            
            foreach (var carController in carControllers)
                if (carController is AITargetCarController aiController)
                {
                    _aiControllers.Add(aiController);
                    _aiControllerModifiers.Add(0);                    
                }
        }

        public void StartRace()
        {
            _isRaceActive = true;
        }

        private void Update()
        {
            var playerDistance = _waypointMainProgressTracker.GetPassedDistance(0);
            for (var i = 0; i < _aiControllers.Count; i++)
            {
                var aiDistance = _waypointMainProgressTracker.GetPassedDistance(i + 1);
                _aiControllers[i].SetAdditionalModifier(GetAdditionalSpeedModifier(playerDistance, aiDistance));
                _aiControllerModifiers[i] = GetAdditionalSpeedModifier(playerDistance, aiDistance);
            }
        }

        private float GetAdditionalSpeedModifier(float playerDistance, float aiDistance)
        {
            var difference = Mathf.Abs(aiDistance - playerDistance);
            var modifier = aiDistance - playerDistance < 0 ? 1 : -1;
            var result = 0f;

            if (modifier == 1 && difference > MIN_ALLOWED_DISTANCE)
            {
                result = Mathf.InverseLerp(MIN_ALLOWED_DISTANCE, MAX_ALLOWED_DISTANCE, difference) * SPEED_MODIFIER;
            }
            else if (modifier == -1 && difference > NEGATIVE_MIN_ALLOWED_DISTANCE)
            {
                result = Mathf.InverseLerp(NEGATIVE_MIN_ALLOWED_DISTANCE, NEGATIVE_MAX_ALLOWED_DISTANCE, difference) * NEGATIVE_SPEED_MODIFIER;
            }

            return result * modifier;
        }
        
        public void OnGUI()
        {
            for (int i = 0; i < _aiControllerModifiers.Count; i++)
            {
                GUI.Label(new Rect(10, 50 + 10 * i, 200, 100), $"{i} : {_aiControllerModifiers[i]}");
            }
        }
    }
}
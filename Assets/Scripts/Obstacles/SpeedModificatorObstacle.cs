using System;
using Cars.Controllers;
using UnityEngine;

namespace Obstacles
{
    [RequireComponent(typeof(BoxCollider))]
    public class SpeedModificatorObstacle : AbstractObstacle
    {
        [SerializeField] private SpeedModifier _speedModifier;

        [Header("on Validate settings")] [SerializeField]
        private BoxCollider _triggerZone;
        
        private void OnValidate()
        {
            if (_triggerZone == null)
                _triggerZone = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponentInParent<CarController>();
            if (controller == null)
                return;
            
            controller.AddSpeedModifier(_speedModifier, true);
        }

        private void OnTriggerExit(Collider other)
        {
            var controller = other.GetComponentInParent<CarController>();
            if (controller == null)
                return;
            
            controller.RemovePermanentSpeedModifier();
            controller.AddSpeedModifier(_speedModifier);
        }
    }

    [Serializable]
    public class SpeedModifier
    {
        [Range(0, 10f)]
        public float time;
        public bool isBoost;
    }
}
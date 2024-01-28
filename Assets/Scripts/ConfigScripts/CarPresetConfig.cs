using Cars.Controllers;
using Cars.InputSystem.AI;
using System;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "CarPresetConfig", menuName = "Configs/Car Preset Config")]
    public class CarPresetConfig : BaseConfig
    {
        public CarControllerType CarControllerType;
        [HideInInspector]
        public Type CarController
        {
            get
            {
                return CarControllerType switch
                {
                    CarControllerType.player => typeof(PlayerCarController),
                    CarControllerType.baseBot or CarControllerType.proBot => typeof(AITargetCarController),
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
            private set { }
        }

        [HideInInspector]
        public Type InputSystem
        {
            get
            {
                return CarControllerType switch
                {
                    CarControllerType.player => throw new ArgumentOutOfRangeException(),
                    CarControllerType.baseBot => typeof(AITargetInputSystem),
                    CarControllerType.proBot => typeof(ProAITargetInputSystem),
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
            private set { }
        }

        [Header("Movement Type")]
        public MovementMode MovementMode;
        public GroundCheck GroundCheck;

        [Header("Layer Masks")]
        public LayerMask DrivableSurface;
        public LayerMask ObstacleLayer;

        [Header("Rays Settings")]
        public float RayLength = 5;
        [Range(0, 1)] public float BackRatio = 0.1f;
    }

    public enum CarControllerType
    {
        player,
        baseBot,
        proBot
    }
}
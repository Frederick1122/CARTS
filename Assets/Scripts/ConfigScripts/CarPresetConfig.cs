﻿using System;
using Cars.Controllers;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "CarPresetConfig", menuName = "Configs/Car Preset Config")]
    public class CarPresetConfig : BaseConfig
    {
        public CarControllerType CarControllerType;
        [HideInInspector] public Type CarController
        {
            get
            {
                switch (CarControllerType)
                {
                    case CarControllerType.player:
                        return typeof(PlayerCarController);
                    case CarControllerType.baseBot:
                    case CarControllerType.proBot:
                        return typeof(AITargetCarController);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            private set { }
        }

        [HideInInspector] public Type InputSystem 
        {
            get
            {
                switch (CarControllerType)
                {
                    case CarControllerType.player:
#if UNITY_STANDALONE_WIN
                        return typeof(KeyBoardInputSystem);
#elif UNITY_ANDROID
                        return typeof(ButtonsMobileInputSystem);
#endif
                    case CarControllerType.baseBot:
                        return typeof(AITargetInputSystem);
                    case CarControllerType.proBot:
                        return typeof(ProAITargetInputSystem);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
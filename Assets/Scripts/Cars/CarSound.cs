using System;
using System.Collections.Generic;
using System.Threading;
using Cars.Controllers;
using Cars.Tools;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Cars
{
    public class CarSound : MonoBehaviour
    {
        private const float SOUND_DELAY = 1f;
        
        private const string ENGINE_SOUND_PARAMETER = "Speed";
        private const string ENGINE_SOUND_EVENT = "event:/SFX/Auto/Engine";
        private const string HITTING_AN_OBSTACLE_SOUND_EVENT = "event:/SFX/Auto/CrashAboutObjects";
        private const string HITTING_AN_ENEMY_SOUND_EVENT = "event:/SFX/Auto/CrashAboutEnemy";
        private const string DRIFT_SOUND_EVENT = "event:/SFX/Auto/Drift";
        private const string BRAKE_SOUND_EVENT = "event:/SFX/Auto/Brake";

        private static readonly string[] OBSTACLE_LAYERS = new[] {"AIObstacle", "Default"};
        private const string CAR_BODY_LAYER = "carbody";

        private CarSkidManager _skidManager;
        private CarController _carController;
        
        private CancellationTokenSource _playSoundToken = new ();
        private Dictionary<CarSoundType, EventInstance> _carSoundInstances = new();
        private HashSet<EventInstance> _activeEventInstances = new ();
        private EventInstance _engineInstance;
        private EventInstance _hittingAnEnemyInstance;
        private EventInstance _hittingAnObstacleInstance;
        private EventInstance _driftInstance;
        private EventInstance _brakeInstance;
        //
        private bool _isDriftingActive;
        
        public void PlaySoundOnce(CarSoundType type)
        {
            PlaySoundTask(_carSoundInstances[type], _playSoundToken.Token);
        }

        public void Init(CarSkidManager skidManager, CarController carController)
        {
            _skidManager = skidManager;
            _carController = carController;
            
            _engineInstance = RuntimeManager.CreateInstance(ENGINE_SOUND_EVENT);
            _hittingAnEnemyInstance = RuntimeManager.CreateInstance(HITTING_AN_ENEMY_SOUND_EVENT);
            _hittingAnObstacleInstance = RuntimeManager.CreateInstance(HITTING_AN_OBSTACLE_SOUND_EVENT);
            _driftInstance = RuntimeManager.CreateInstance(DRIFT_SOUND_EVENT);
            _brakeInstance = RuntimeManager.CreateInstance(BRAKE_SOUND_EVENT);

            _carSoundInstances.Add(CarSoundType.brake, _brakeInstance);
            _carSoundInstances.Add(CarSoundType.hittingAnEnemy, _hittingAnEnemyInstance);
            _carSoundInstances.Add(CarSoundType.hittingAnObstacle, _hittingAnObstacleInstance);
            
            _engineInstance.start();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"{gameObject.name} punch {collision.gameObject.name}");
            var currentLayer = LayerMask.LayerToName(collision.gameObject.layer);

            if (currentLayer == CAR_BODY_LAYER)
            {
                PlaySoundOnce(CarSoundType.hittingAnEnemy);
            }
            else if (Array.Exists(OBSTACLE_LAYERS, s => s == currentLayer))
            {
                PlaySoundOnce(CarSoundType.hittingAnObstacle);
            }
        }

        private void OnDestroy()
        {
            _playSoundToken.Dispose();
            
            _engineInstance.release();
            _hittingAnEnemyInstance.release();
            _hittingAnObstacleInstance.release();
            _driftInstance.release();
            _brakeInstance.release();
        }

        private void LateUpdate()
        {
            UpdateEngineSound();
            UpdateDriftSound();
        }
        
        private void UpdateEngineSound()
        {
            _engineInstance.setPaused(Time.timeScale == 0);
            
            _engineInstance.set3DAttributes(gameObject.To3DAttributes());
            _engineInstance.setParameterByName(ENGINE_SOUND_PARAMETER, _carController.CarVelocity.magnitude);
        }

        private void UpdateDriftSound()
        {
            if (_skidManager == null)
                return;
            
            _driftInstance.set3DAttributes(gameObject.To3DAttributes());

            if (_skidManager.IsDrifting & !_isDriftingActive)
            {
                _driftInstance.start();
                _isDriftingActive = true;
            }
            else if (!_skidManager.IsDrifting & _isDriftingActive)
            {
                _driftInstance.stop(STOP_MODE.ALLOWFADEOUT);
                _isDriftingActive = false;
            }
        }
        
        private async UniTaskVoid PlaySoundTask(EventInstance instance, CancellationToken token)
        {
            if (_activeEventInstances.Contains(instance))
            {
                return;
            }

            _activeEventInstances.Add(instance);
            instance.set3DAttributes(gameObject.To3DAttributes());
            instance.start();
            await UniTask.Delay(TimeSpan.FromSeconds(SOUND_DELAY), cancellationToken: token);

            _activeEventInstances.Remove(instance);
        }
    }

    public enum CarSoundType
    {
        brake,
        hittingAnEnemy,
        hittingAnObstacle
    }
}
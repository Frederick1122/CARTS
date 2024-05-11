using Core.Tools;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Cars.Tools
{
    public class CarCollisionManager : MonoBehaviour
    {
        private const float MAX_RESISTANCE_TIME = 20;

        public bool IsWork => _collisionDetection.IsWork;
        public bool CollisionCanTurnOn => _collisionDetection;

        private CarCollisionDetection _collisionDetection;

        private float _resistanceAfterSpawn;
        private float _maxResistanceTimer = MAX_RESISTANCE_TIME;

        private readonly List<Renderer> _RenderersOnCar = new();
        private CancellationTokenSource _resistanceToken;

        public void Init(CarCollisionDetection collisionDetection, float resistanceAfterSpawn, LayerMask onCarLayers)
        {
            _collisionDetection = collisionDetection;
            _collisionDetection.Init(GetComponent<BoxCollider>(), onCarLayers);

            _resistanceAfterSpawn = resistanceAfterSpawn;

            var childRenderer = GetComponentsInChildren<Renderer>();
            foreach (var child in childRenderer)
            {
                if (child.TryGetComponent(out ParticleSystem _))
                    continue;

                _RenderersOnCar.Add(child);
            }
        }

        private void OnDestroy() =>
            _resistanceToken?.Cancel();

        private void Update()
        {
            if (_collisionDetection.IsWork)
                _maxResistanceTimer -= Time.deltaTime;

            if (_maxResistanceTimer <= 0)
                MakeResistance(0);
        }

        public void MakeResistance(float time = -1)
        {
            _resistanceToken = new CancellationTokenSource();
            MakeResistanceTask(_resistanceToken.Token, time).Forget();
        }

        private async UniTaskVoid MakeResistanceTask(CancellationToken token, float time = -1)
        {
            var resistanceTime = time == -1 ? _resistanceAfterSpawn : time;
            _maxResistanceTimer = MAX_RESISTANCE_TIME + resistanceTime;
            _collisionDetection.IsWork = true;
            BeResistance();

            await UniTask.Delay(TimeSpan.FromSeconds(resistanceTime), cancellationToken: token);

            if (_collisionDetection.CollisionCanTurnOn)
                BeUnResistance();
            else
                _collisionDetection.OnNoCollidersIn += BeUnResistance;
        }

        private void BeResistance()
        {
            foreach (var renderer in _RenderersOnCar)
            {
                var mat = renderer.material;
                var color = new Color32(100, 100, 100, 255);
                mat.DOColor(color, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void BeUnResistance()
        {
            _collisionDetection.OnNoCollidersIn -= BeUnResistance;
            _collisionDetection.IsWork = false;

            foreach (var renderer in _RenderersOnCar)
            {
                var mat = renderer.material;
                DOTween.Kill(mat);
                mat.SetOverrideTag("RenderType", "");
                var color = new Color32(255, 255, 255, 255);
                mat.DOColor(color, 0);
            }
        }
    }
}

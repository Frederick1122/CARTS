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
        public bool IsWork => _collisionDetection.IsWork;
        public bool CollisionCanTurnOn => _collisionDetection;

        private CarCollisionDetection _collisionDetection;

        private float _resistanceAfterSpawn;

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
            _resistanceToken.Cancel();

        public void MakeResistance(float time = -1)
        {
            _resistanceToken = new CancellationTokenSource();
            MakeResistanceCuro(_resistanceToken.Token, time).Forget();
        }

        private async UniTaskVoid MakeResistanceCuro(CancellationToken token, float time = -1)
        {
            var resistanceTime = time == -1 ? _resistanceAfterSpawn : time;
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
                mat.ToFadeMode();
                var color1 = new Color(mat.color.r, mat.color.g, mat.color.b, 0.3f);
                var color2 = new Color(mat.color.r, mat.color.g, mat.color.b, 0.6f);
                mat.color = color2;
                mat.DOColor(color1, 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void BeUnResistance()
        {
            _collisionDetection.OnNoCollidersIn -= BeUnResistance;
            _collisionDetection.IsWork = false;

            foreach (var renderer in _RenderersOnCar)
            {
                var mat = renderer.material;
                mat.ToOpaqueMode();
                DOTween.Kill(mat);
                mat.SetOverrideTag("RenderType", "");
                var color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);
                mat.DOColor(color, 0);
            }
        }
    }
}

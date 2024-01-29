using ConfigScripts;
using UnityEngine;

namespace Cars.InputSystem.AI
{
    public class ProAITargetInputSystem : AITargetInputSystem
    {
        private Transform[] _rayPoses = new Transform[4];
        private LayerMask _obstacleLayers;
        private float _rayLength = 20;
        private float _backRatio = 0.1f;

        private RaycastHit _hitL1, _hitL2, _hitR2, _hitR1;
        private Ray _rayL1, _rayL2, _rayR2, _rayR1;

        private bool _needToRev = false;

        public override void Init(CarPresetConfig presetConfig, CarPrefabData prefabData)
        {
            base.Init(presetConfig, prefabData);
            _obstacleLayers = presetConfig.ObstacleLayer;
            _rayLength = presetConfig.RayLength;
            _backRatio = presetConfig.BackRatio;

            _rayPoses = prefabData.RayPoses;
        }

        public override void ReadInput()
        {
            base.ReadInput();

            ReadRayInput();
        }

        private void ReadRayInput()
        {
            float mindist = _rayLength;
            float horInpAfter = _horInp;

            _rayL1 = new(_rayPoses[0].position, _rayPoses[0].forward * _rayLength);
            if (Physics.Raycast(_rayL1, out _hitL1, _rayLength, _obstacleLayers))
            {
                if (mindist >= _hitL1.distance)
                {
                    horInpAfter = 1;
                    mindist = _hitL1.distance;
                }
            }

            _rayL2 = new(_rayPoses[1].position, _rayPoses[1].forward * _rayLength);
            if (Physics.Raycast(_rayL2, out _hitL2, _rayLength, _obstacleLayers))
            {
                if (mindist >= _hitL2.distance)
                {
                    horInpAfter = 0.5f;
                    mindist = _hitL2.distance;
                }
            }

            _rayR2 = new(_rayPoses[2].position, _rayPoses[2].forward * _rayLength);
            if (Physics.Raycast(_rayR2, out _hitR2, _rayLength, _obstacleLayers))
            {
                if (mindist >= _hitR2.distance)
                {
                    horInpAfter = -0.5f;
                    mindist = _hitR2.distance;
                }
            }

            _rayR1 = new(_rayPoses[3].position, _rayPoses[3].forward * _rayLength);
            if (Physics.Raycast(_rayR1, out _hitR1, _rayLength, _obstacleLayers))
            {
                if (mindist >= _hitR1.distance)
                {
                    horInpAfter = -1;
                    mindist = _hitR1.distance;
                }
            }

            var newHorInput = Mathf.Clamp(horInpAfter, -1, 1);
            var neededSpeed = Mathf.Clamp(InterpolateRayDistance(mindist), -1, 1) * _config.maxSpeedLevels[0] / 2;

            if (mindist >= _rayLength / 2)
                _needToRev = false;

            _horInp = Mathf.Lerp(_horInp, newHorInput, _config.turnLevels[0] * 10);

            if (_needToRev)
            {
                _vertInp = -1;
                _horInp *= -1;
                return;
            }

            if (neededSpeed < _controller.CarVelocity.z)
                _brInp = 1;
        }

        private float InterpolateRayDistance(float dist)
        {
            if (dist <= _backRatio)
            {
                _needToRev = true;
                return -1;
            }

            if (dist == _rayLength)
                return _vertInp;

            float res = dist / _rayLength;

            return res;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            if (_rayPoses[0] == null || _rayPoses[1] == null || _rayPoses[2] == null || _rayPoses[3] == null)
                return;

            Gizmos.DrawRay(_rayPoses[0].position, _rayPoses[0].forward * _rayLength);
            Gizmos.DrawRay(_rayPoses[1].position, _rayPoses[1].forward * _rayLength);
            Gizmos.DrawRay(_rayPoses[2].position, _rayPoses[2].forward * _rayLength);
            Gizmos.DrawRay(_rayPoses[3].position, _rayPoses[3].forward * _rayLength);

        }
#endif
    }
}

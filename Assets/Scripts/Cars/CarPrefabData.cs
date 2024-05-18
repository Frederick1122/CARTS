using System;
using Cinemachine;
using Managers;
using UnityEngine;

namespace Cars
{
    public class CarPrefabData : MonoBehaviour
    {
        private const float _editorRayLength = 3f;

        
        [field: SerializeField] public float SkidWidth { get; set; } = 0.1f;
        
        [field: Header("Colliders")]

        [field: SerializeField] public BoxCollider CarCollider { get; private set; }

        [field: Header("Rigidbody")]
        [field: SerializeField] public Rigidbody RbSphere { get; private set; }
        [field: SerializeField] public Rigidbody CarBody { get; private set; }

        [field: Header("Visual")]
        [field: SerializeField] public Transform BodyMesh { get; private set; }
        [field: SerializeField] public Transform[] FrontWheels { get; private set; } = new Transform[2];
        [field: SerializeField] public Transform[] RearWheels { get; private set; } = new Transform[2];

        [field: Header("Camera")]
        [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }

        [field: Header("Rays")]
        [field: SerializeField] public Transform[] RayPoses { get; set; } = new Transform[4];

        [field: Header("Car Layers")]
        [field: SerializeField] public LayerMask OnCarLayers { get; private set; }

        [Header("UI Upgrade Points")]
        [SerializeField] private Transform _maxSpeedUpgradePlace;
        [SerializeField] private Transform _accelerationUpgradePlace;
        [SerializeField] private Transform _turnUpgradePlace;

        [Header("Other")]
        [SerializeField] private SphereCollider _sphereCollider;

        public Transform GetModificationPlace(ModificationType mode)
        {
            return mode switch
            {
                ModificationType.MaxSpeed => _maxSpeedUpgradePlace,
                ModificationType.Acceleration => _accelerationUpgradePlace,
                ModificationType.Turn => _turnUpgradePlace,
                _ => transform,
            };
        }

        public Vector3 GetLowestPoint()
        {
            if(_sphereCollider == null)
                _sphereCollider = RbSphere.GetComponent<SphereCollider>();
            var radius = _sphereCollider.radius;
            var delta = Mathf.Abs(transform.position.y) - Mathf.Abs(RbSphere.transform.position.y);
            var point = Vector3.zero;
            point.y = transform.localScale.y * (delta - radius);
            return point;
        }

        private void OnValidate()
        {
            if (CarCollider == null)
            {
                var colliders = GetComponents<BoxCollider>();

                foreach (var collider in colliders)
                {
                    if (collider.isTrigger) 
                        continue;

                    CarCollider = collider;
                    break;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            // Rays
            Gizmos.color = Color.cyan;

            if (RayPoses[0] == null || RayPoses[1] == null || RayPoses[2] == null || RayPoses[3] == null)
                return;

            Gizmos.DrawRay(RayPoses[0].position, RayPoses[0].forward * _editorRayLength);
            Gizmos.DrawRay(RayPoses[1].position, RayPoses[1].forward * _editorRayLength);
            Gizmos.DrawRay(RayPoses[2].position, RayPoses[2].forward * _editorRayLength);
            Gizmos.DrawRay(RayPoses[3].position, RayPoses[3].forward * _editorRayLength);

            if (_maxSpeedUpgradePlace != null)
                Gizmos.DrawSphere(_maxSpeedUpgradePlace.transform.position, 0.1f);

            if (_accelerationUpgradePlace != null)
                Gizmos.DrawSphere(_accelerationUpgradePlace.transform.position, 0.1f);

            if (_turnUpgradePlace != null)
                Gizmos.DrawSphere(_turnUpgradePlace.transform.position, 0.1f);
        }
#endif
    }
}
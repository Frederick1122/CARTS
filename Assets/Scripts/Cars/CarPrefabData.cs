using Cinemachine;
using UnityEngine;

namespace Cars
{
    public class CarPrefabData : MonoBehaviour
    {
        private const float _editorRayLength = 3f;

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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            Gizmos.color = Color.cyan;

            if (RayPoses[0] == null || RayPoses[1] == null || RayPoses[2] == null || RayPoses[3] == null)
                return;

            Gizmos.DrawRay(RayPoses[0].position, RayPoses[0].forward * _editorRayLength);
            Gizmos.DrawRay(RayPoses[1].position, RayPoses[1].forward * _editorRayLength);
            Gizmos.DrawRay(RayPoses[2].position, RayPoses[2].forward * _editorRayLength);
            Gizmos.DrawRay(RayPoses[3].position, RayPoses[3].forward * _editorRayLength);

        }
#endif
    }
}
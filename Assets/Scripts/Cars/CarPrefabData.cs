using Cinemachine;
using UnityEngine;

namespace Cars
{
    public class CarPrefabData : MonoBehaviour
    {
        [field: Header("Rigidbody")]
        [field: SerializeField] public Rigidbody RbSphere { get; private set; }
        [field: SerializeField] public Rigidbody CarBody { get; private set; }

        [field: Header("Visual")]
        [field: SerializeField] public Transform BodyMesh { get; private set; }
        [field: SerializeField] public Transform[] FrontWheels { get; private set; } = new Transform[2];
        [field: SerializeField] public Transform[] RearWheels { get; private set; } = new Transform[2];   
        
        [field: Header("Camera")]
        [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }

    }
}
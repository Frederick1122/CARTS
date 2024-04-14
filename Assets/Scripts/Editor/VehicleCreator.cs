using Cars.Controllers;
using UnityEngine;
using UnityEditor;
using Cars;

public class VehicleCreator : EditorWindow
{
    private GameObject _preset;
    private Transform _vehicleParent;
    private Transform _wheelFL, _wheelFR, _wheelRL, _wheelRR;

    private MeshRenderer _bodyMesh;
    private MeshRenderer _wheelMesh;

    private GameObject _newVehicle;

    [MenuItem("Tools/Vehicle Creator")]
    static void OpenWindow()
    {
        VehicleCreator vehicleCreatorWindow = (VehicleCreator)GetWindow(typeof(VehicleCreator));
        vehicleCreatorWindow.minSize = new Vector2(400, 300);
        vehicleCreatorWindow.Show();
    }

    private void OnGUI()
    {
        var style = new GUIStyle(EditorStyles.boldLabel);
        style.normal.textColor = Color.green;

        GUILayout.Label("Vehicle Creator", style);
        _preset = EditorGUILayout.ObjectField("Vehicle preset", _preset, typeof(GameObject), true) as GameObject;

        GUILayout.Label("Your Vehicle", style);
        _vehicleParent = EditorGUILayout.ObjectField("Vehicle Parent", _vehicleParent, typeof(Transform), true) as Transform;
        _wheelFL = EditorGUILayout.ObjectField("Wheel FL", _wheelFL, typeof(Transform), true) as Transform;
        _wheelFR = EditorGUILayout.ObjectField("Wheel FR", _wheelFR, typeof(Transform), true) as Transform;
        _wheelRL = EditorGUILayout.ObjectField("Wheel RL", _wheelRL, typeof(Transform), true) as Transform;
        _wheelRR = EditorGUILayout.ObjectField("Wheel RR", _wheelRR, typeof(Transform), true) as Transform;

        if (GUILayout.Button("Create Vehicle"))
            CreateVehicle();

        _bodyMesh = EditorGUILayout.ObjectField("Body Mesh", _bodyMesh, typeof(MeshRenderer), true) as MeshRenderer;
        _wheelMesh = EditorGUILayout.ObjectField("Wheel Mesh", _wheelMesh, typeof(MeshRenderer), true) as MeshRenderer;

        if (GUILayout.Button("Adjust Colliders"))
            AdjustColliders();

    }

    private void AdjustColliders()
    {
        var colliders = _newVehicle.GetComponents<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            var collider = colliders[i];
            collider.center = Vector3.zero;
            collider.size = _bodyMesh.bounds.size;
            SetUpRays(collider);

            if (i != 0)
            {
                collider.size += new Vector3(0.1f, 0.1f, 0.1f);
                collider.isTrigger = true;
            }
        }

        if (_newVehicle.TryGetComponent(out BoxCollider boxCollider))
        {
            boxCollider.center = Vector3.zero;
            boxCollider.size = _bodyMesh.bounds.size;

            SetUpRays(boxCollider);
        }

        if (_newVehicle.TryGetComponent(out CapsuleCollider capsuleCollider))
        {
            capsuleCollider.center = Vector3.zero;
            capsuleCollider.height = _bodyMesh.bounds.size.z;
            capsuleCollider.radius = _bodyMesh.bounds.size.x / 2;
        }

        Vector3 SpheareRBOffset = new(_newVehicle.transform.position.x,
                                              _wheelFL.position.y + _bodyMesh.bounds.extents.y - _wheelMesh.bounds.size.y / 2,
                                              _newVehicle.transform.position.z);

        if (_newVehicle.transform.Find("SphereRB"))
        {
            _newVehicle.transform.Find("SphereRB").GetComponent<SphereCollider>().radius = _bodyMesh.bounds.extents.y;
            _newVehicle.transform.Find("SphereRB").position = SpheareRBOffset;
        }

        _newVehicle.GetComponent<CarPrefabData>().SkidWidth = _wheelMesh.bounds.size.x / 2;
        _newVehicle.transform.Find("Skid").Find("Skid marks FL").position = _wheelFL.position - Vector3.up * (_wheelMesh.bounds.size.y / 2 - 0.02f);
        _newVehicle.transform.Find("Skid").Find("Skid marks FR").position = _wheelFR.position - Vector3.up * (_wheelMesh.bounds.size.y / 2 - 0.02f);
        _newVehicle.transform.Find("Skid").Find("Skid marks RL").position = _wheelRL.position - Vector3.up * (_wheelMesh.bounds.size.y / 2 - 0.02f);
        _newVehicle.transform.Find("Skid").Find("Skid marks RR").position = _wheelRR.position - Vector3.up * (_wheelMesh.bounds.size.y / 2 - 0.02f);
    }

    private void CreateVehicle()
    {
        MakeVehicleReadyForSetup();

        _newVehicle = Instantiate(_preset, _bodyMesh.bounds.center, _vehicleParent.rotation);
        _newVehicle.name = _vehicleParent.name;

        DestroyImmediate(_newVehicle.transform.Find("Mesh").Find("Body").GetChild(0).gameObject);
        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL"))
            DestroyImmediate(_newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelFL").Find("WheelFL Axel").GetChild(0).gameObject);

        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR"))
            DestroyImmediate(_newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelFR").Find("WheelFR Axel").GetChild(0).gameObject);

        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL"))
            DestroyImmediate(_newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelRL").Find("WheelRL Axel").GetChild(0).gameObject);

        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR"))
            DestroyImmediate(_newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelRR").Find("WheelRR Axel").GetChild(0).gameObject);


        _vehicleParent.parent = _newVehicle.transform.Find("Mesh").Find("Body");

        _newVehicle.transform.Find("Mesh").transform.Find("Wheels").position = _vehicleParent.position;

        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL"))
        {
            _newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL").position = _wheelFL.position;
            _wheelFL.parent = _newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelFL").Find("WheelFL Axel");
        }

        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR"))
        {
            _newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR").position = _wheelFR.position;
            _wheelFR.parent = _newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelFR").Find("WheelFR Axel");
        }

        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL"))
        {
            _newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL").position = _wheelRL.position;
            _wheelRL.parent = _newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelRL").Find("WheelRL Axel");
        }

        if (_newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR"))
        {
            _newVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR").position = _wheelRR.position;
            _wheelRR.parent = _newVehicle.transform.Find("Mesh").transform.Find("Wheels")
                .Find("WheelRR").Find("WheelRR Axel");
        }
    }

    private void MakeVehicleReadyForSetup()
    {
        _vehicleParent.transform.position = Vector3.zero;

        var AllVehicleColliders = _vehicleParent.GetComponentsInChildren<Collider>();
        foreach (var collider in AllVehicleColliders)
            DestroyImmediate(collider);

        var AllRigidBodies = _vehicleParent.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in AllRigidBodies)
            DestroyImmediate(rb);
    }

    private void SetUpRays(BoxCollider boxCollider)
    {
        var rays = _newVehicle.GetComponent<CarPrefabData>().RayPoses;
        var rayParent = rays[0].parent;

        var frontOffset = -0.05f + boxCollider.bounds.center.z + boxCollider.bounds.size.z / 2;
        rayParent.localPosition = new Vector3(rayParent.localPosition.x, rayParent.localPosition.y, frontOffset);

        var sideOffset = 0.01f  + boxCollider.bounds.center.x + boxCollider.bounds.size.x / 2 ;
        rays[0].localPosition = new Vector3(rays[0].localPosition.x, rays[0].localPosition.y, -sideOffset);
        rays[3].localPosition = new Vector3(rays[3].localPosition.x, rays[3].localPosition.y, sideOffset);

        sideOffset /= 2f;
        rays[1].localPosition = new Vector3(rays[1].localPosition.x, rays[1].localPosition.y, -sideOffset);
        rays[2].localPosition = new Vector3(rays[2].localPosition.x, rays[2].localPosition.y, sideOffset);
    }
}


using Cars.Controllers;
using UnityEngine;
using UnityEditor;

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
        if (_newVehicle.TryGetComponent(out BoxCollider boxCollider))
        {
            boxCollider.center = Vector3.zero;
            boxCollider.size = _bodyMesh.bounds.size;
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

        _newVehicle.GetComponent<CarController>().SkidWidth = _wheelMesh.bounds.size.x / 2;
        if (_newVehicle.transform.Find("SphereRB"))
        {
            _newVehicle.transform.Find("SphereRB").GetComponent<SphereCollider>().radius = _bodyMesh.bounds.extents.y;
            _newVehicle.transform.Find("SphereRB").position = SpheareRBOffset;
        }

        //NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("Skid marks FL").position = wheelFL.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
        //NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("Skid marks FR").position = wheelFR.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
        //NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("Skid marks RL").position = wheelRL.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
        //NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("Skid marks RR").position = wheelRR.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
    }

    private void CreateVehicle()
    {
        MakeVehicleReadyForSetup();

        _newVehicle = Instantiate(_preset, _bodyMesh.bounds.center, _vehicleParent.rotation);
        _newVehicle.name = "Arcade_Ai_" + _vehicleParent.name;

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
        var AllVehicleColliders = _vehicleParent.GetComponentsInChildren<Collider>();
        foreach (var collider in AllVehicleColliders)
            DestroyImmediate(collider);

        var AllRigidBodies = _vehicleParent.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in AllRigidBodies)
            DestroyImmediate(rb);
    }
}


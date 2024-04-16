using System.Collections.Generic;
using CustomSnapTool;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEngine;

[EditorTool("Custom Snap Move", typeof(GameObject))]
public class CustomSnapEditor : EditorTool
{
    public const float MINIMAL_DISTANCE_TO_SNAP = 3f;
    private CustomSnapPoint[] _allPoints;

    private Transform _oldTarget;
    private CustomSnapPoint[] _targetPoints;

    public override GUIContent toolbarIcon =>
        new GUIContent
        {
            image = EditorGUIUtility.IconContent("CustomTool").image,
            text = "Custom Snap Move Tool",
            tooltip = "Custom Snap Move Tool"
        };

    public override void OnToolGUI(EditorWindow window)
    {
        var snapTarget = (GameObject) target;
        var targetTransform = snapTarget.transform;

        if (!snapTarget.TryGetComponent(out CustomSnap trueSnapTarget))
        {
            EditorGUI.BeginChangeCheck();
            var pos = Handles.PositionHandle(targetTransform.position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetTransform, "Move with snap move tool");
                targetTransform.position = pos;
            }

            return;
        }

        if (_oldTarget != targetTransform)
        {
            var prefabStage = PrefabStageUtility.GetPrefabStage(targetTransform.gameObject);
            if (prefabStage != null)
                _allPoints = prefabStage.prefabContentsRoot.GetComponentsInChildren<CustomSnapPoint>();
            else
                _allPoints = FindObjectsOfType<CustomSnapPoint>();
            _targetPoints = targetTransform.GetComponentsInChildren<CustomSnapPoint>();
            _oldTarget = targetTransform;
        }

        SetDopHandles();

        EditorGUI.BeginChangeCheck();

        var style = new GUIStyle();
        style.normal.background = new Texture2D(1, 1);
        Handles.Label(targetTransform.position, "CENTER", style);

        var newPos = Handles.PositionHandle(targetTransform.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetTransform, "Move with snap move tool");
            MoveWithSnapping(targetTransform, newPos);
        }
    }

    private void MoveWithSnapping(Transform targetTransform, Vector3 newPosition)
    {
        var nearestPos = newPosition;
        var minimalDistance = float.PositiveInfinity;

        foreach (var point in _allPoints)
        {
            if (point.transform.parent == targetTransform)
                continue;

            foreach (var ownPoint in _targetPoints)
            {
                if (ownPoint.ConnectionType != point.ConnectionType)
                    if (ownPoint.ConnectionType != ConnectionType.Default &&
                        point.ConnectionType != ConnectionType.Default)
                        continue;

                var targetPos = point.transform.position - (ownPoint.transform.position - targetTransform.position);
                var distance = Vector3.Distance(targetPos, newPosition);

                if (!(distance < minimalDistance))
                    continue;

                minimalDistance = distance;
                nearestPos = targetPos;
            }
        }

        if (minimalDistance > MINIMAL_DISTANCE_TO_SNAP)
            nearestPos = newPosition;

        targetTransform.position = nearestPos;
    }

    private void SetDopHandles()
    {
        EditorGUI.BeginChangeCheck();
        Dictionary<CustomSnapPoint, Vector3> handleSnapPointPairs = new();
        foreach (var point in _targetPoints)
        {
            var style = new GUIStyle();
            style.normal.textColor = point.GetPointColor();
            style.normal.background = new Texture2D(1, 1);
            Handles.Label(point.transform.position, "Point", style);

            var handlePos = Handles.PositionHandle(point.transform.position, Quaternion.identity);
            handleSnapPointPairs.Add(point, handlePos);
        }

        if (EditorGUI.EndChangeCheck())
            foreach (var item in handleSnapPointPairs.Keys)
                if (handleSnapPointPairs.TryGetValue(item, out var pos))
                {
                    Undo.RecordObject(item, "Move with snap move tool");
                    item.transform.position = pos;
                }
        //Undo.RecordObject(handleSnapPointPairs, "Move with snap move tool");
        //MoveWithSnapping(targetTransform, newPos);
    }
}
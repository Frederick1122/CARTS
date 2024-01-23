using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(WaypointCircuit.WaypointList))]
public class WaypointListDrawer : PropertyDrawer
{
    private const string ITEMS = "Items";
    private const string CIRCUIT = "Circuit";

    private float _lineHeight = 18;
    private float _spacing = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float x = position.x;
        float y = position.y;
        float inspectorWidth = position.width;

        // Draw label
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var items = property.FindPropertyRelative(ITEMS);
        var titles = new string[] { "Transform", "", "", "" };
        var props = new string[] { "transform", "^", "v", "-" };
        var widths = new float[] { .7f, .1f, .1f, .1f };
        float lineHeight = 18;
        bool changedLength = false;

        if (items.arraySize > 0)
        {
            for (int i = 0; i < items.arraySize; ++i)
            {
                var item = items.GetArrayElementAtIndex(i);

                float rowX = x;
                for (int n = 0; n < props.Length; ++n)
                {
                    float w = widths[n] * inspectorWidth;

                    // Calculate rects
                    Rect rect = new(rowX, y, w, lineHeight);
                    rowX += w;

                    if (i == -1)
                    {
                        EditorGUI.LabelField(rect, titles[n]);
                    }
                    else
                    {
                        if (n == 0)
                        {
                            EditorGUI.ObjectField(rect, item.objectReferenceValue, typeof(Transform), true);
                        }
                        else
                        {
                            if (GUI.Button(rect, props[n]))
                            {
                                switch (props[n])
                                {
                                    case "-":
                                        items.DeleteArrayElementAtIndex(i);
                                        items.DeleteArrayElementAtIndex(i);
                                        changedLength = true;
                                        break;
                                    case "v":
                                        if (i > 0)
                                        {
                                            items.MoveArrayElement(i, i + 1);
                                        }
                                        break;
                                    case "^":
                                        if (i < items.arraySize - 1)
                                        {
                                            items.MoveArrayElement(i, i - 1);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                y += lineHeight + _spacing;
                if (changedLength)
                {
                    break;
                }
            }
        }
        else
        {
            // add button
            var addButtonRect = new Rect((x + position.width) - widths[widths.Length - 1] * inspectorWidth, y,
                                         widths[widths.Length - 1] * inspectorWidth, lineHeight);
            if (GUI.Button(addButtonRect, "+"))
                items.InsertArrayElementAtIndex(items.arraySize);

            y += lineHeight + _spacing;
        }

        // add all button
        var addAllButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
        if (GUI.Button(addAllButtonRect, "Assign using all child objects"))
        {
            var circuit = property.FindPropertyRelative(CIRCUIT).objectReferenceValue as WaypointCircuit;
            var children = new Transform[circuit.transform.childCount];

            for (int i = 0; i < circuit.transform.childCount; i++)
                children[i] = circuit.transform.GetChild(i);

            circuit.SetUpWaypoints(children.Length, children);
        }
        y += lineHeight + _spacing;

        // rename all button
        var renameButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
        if (GUI.Button(renameButtonRect, "Auto Rename numerically from this order"))
        {
            var circuit = property.FindPropertyRelative(CIRCUIT).objectReferenceValue as WaypointCircuit;
            int n = 0;
            foreach (Transform child in circuit.Waypoints)
                child.name = "Waypoint " + (n++).ToString("000");
        }
        y += lineHeight + _spacing;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty items = property.FindPropertyRelative(ITEMS);
        float lineAndSpace = _lineHeight + _spacing;
        return 40 + (items.arraySize * lineAndSpace) + lineAndSpace;
    }

    // comparer for check distances in ray cast hits
    public class TransformNameComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((Transform)x).name.CompareTo(((Transform)y).name);
        }
    }
}
#endif
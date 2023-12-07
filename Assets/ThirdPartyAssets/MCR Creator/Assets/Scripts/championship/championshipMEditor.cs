// Description : championshipMEditor.cs : Works in association with championshipM.cs . 
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(championshipM))]
public class championshipMEditor : Editor {
	SerializedProperty		SeeInspector;								// use to draw default Inspector


	private Texture2D MakeTex(int width, int height, Color col) {		// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}




	void OnEnable () {
		// Setup the SerializedProperties.
		SeeInspector 				= serializedObject.FindProperty ("SeeInspector");
		
	}


	public override void OnInspectorGUI()
	{
		

		serializedObject.Update ();

        if (SeeInspector.boolValue)                         // If true Default Inspector is drawn on screen
            DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("See Inspector:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Erase Championship Save"))
        {
            if (EditorUtility.DisplayDialog("Championship Save"
                , "Do you want to delete the Championship Save PlayerPrefs"
                , "Yes","No"))
            {
                PlayerPrefs.DeleteKey("chamionnshipState");
            }

        }

        GUILayout.Label("");
		

        serializedObject.ApplyModifiedProperties();
	}


	void OnSceneGUI( )
	{
	}
}
#endif
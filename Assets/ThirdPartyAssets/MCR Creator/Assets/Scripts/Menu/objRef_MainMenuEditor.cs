//Description : objRef_MainMenuEditor.cs : Work in association with objRef_MainMenu.cs
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(objRef_MainMenu))]
public class objRef_MainMenuEditor : Editor
{
    SerializedProperty      SeeInspector;                                           // use to draw default Inspector
    SerializedProperty      showOnlyP1andP2;
    SerializedProperty  showMoreThan4Cars;

    private Texture2D MakeTex(int width, int height, Color col) {                   // use to change the GUIStyle
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i) {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private Texture2D       Tex_01;
   /* private Texture2D       Tex_02;
    private Texture2D       Tex_03;
    private Texture2D       Tex_04;
    private Texture2D       Tex_05;
*/
    public Color _cGreen = Color.green;
    public Color _cRed = new Color(1, 0f, 0F, .5f);

    void OnEnable () {
        // Setup the SerializedProperties.
        SeeInspector    = serializedObject.FindProperty ("SeeInspector");
        showOnlyP1andP2 = serializedObject.FindProperty("showOnlyP1andP2");
        showMoreThan4Cars= serializedObject.FindProperty("showMoreThan4Cars");

        Tex_01 = MakeTex(2, 2, new Color(1,.92f,0.016F,.7f)); 
      /*  Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,1f)); 
        Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
        Tex_04 = MakeTex(2, 2, new Color(1,.3f,1,.3f)); 
        Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); */
        objRef_MainMenu myScript = (objRef_MainMenu)target; 
	}

	public override void OnInspectorGUI()
	{
        

        if (SeeInspector.boolValue)                                                  // If true Default Inspector is drawn on screen
            DrawDefaultInspector();


        serializedObject.Update();

        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box);
        style_Yellow_01.normal.background = Tex_01;
       /* GUIStyle style_Blue = new GUIStyle(GUI.skin.box);
        style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box);
        style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box);
        style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box);
        style_Yellow_Strong.normal.background = Tex_02;*/


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Inspector :", GUILayout.Width(90));
        EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        objRef_MainMenu myScript = (objRef_MainMenu)target; 

		
		

// --> Create the path
		if (GUILayout.Button("Update"))																			// --> Create path using checkpoints in Track_Path gameObject
		{
            Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
            GameObject obj_MainMenu = GameObject.Find("Canvas_MainMenu");

            Transform[] allChildren = obj_MainMenu.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if(child.name == "J1" && child.parent.parent.name =="Page_CarSelection"){
                    myScript.carSelection_J1 = child.gameObject;}
                if (child.name == "J2" && child.parent.parent.name == "Page_CarSelection"){
                    myScript.carSelection_J2 = child.gameObject;}
                if (child.name == "J3" && child.parent.parent.name == "Page_CarSelection"){
                    myScript.carSelection_J3 = child.gameObject;}
                if (child.name == "J4" && child.parent.parent.name == "Page_CarSelection"){
                    myScript.carSelection_J4 = child.gameObject;}

                if (child.name == "J2_CameraCar3D")
                    myScript.J4Plus_J2_Camera3D = child.gameObject;
                

                if (child.name == "J4+")
                    myScript.carSelection_J4Plus = child.gameObject;
                
            }
		}
			
	

		GUILayout.Label("");
        EditorGUILayout.BeginVertical(style_Yellow_01);
            GUILayout.Label("");

            EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Deactivate CPU car in menu car selection:", GUILayout.Width(230));
                EditorGUILayout.PropertyField(showOnlyP1andP2, new GUIContent(""), GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

        if (showOnlyP1andP2.boolValue && !showMoreThan4Cars.boolValue)
            showMoreThan4Cars.boolValue = true;

            EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Show More Than 4 Cars:", GUILayout.Width(230));
                EditorGUILayout.PropertyField(showMoreThan4Cars, new GUIContent(""), GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();
            GUILayout.Label("");
        EditorGUILayout.EndVertical();
        GUILayout.Label("");

		serializedObject.ApplyModifiedProperties ();


	}


}
#endif
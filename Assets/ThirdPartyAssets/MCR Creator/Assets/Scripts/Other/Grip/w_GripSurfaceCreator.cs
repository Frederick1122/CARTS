// Description : w_GripSurfaceCreator.cs : This script is used to create grip surface
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.IO;


public class w_GripSurfaceCreator : EditorWindow
{
    private Vector2 scrollPosAll;
    SerializedObject serializedObject2;
   // SerializedProperty helpBoxEditor;

    SerializedProperty listOfSurface;

    /*SerializedProperty currentDatasProjectFolder;
    SerializedProperty int_CurrentDatasProjectFolder;
    SerializedProperty int_CurrentDatasSaveSystem;
    SerializedProperty s_newProjectName;

    SerializedProperty s_newLanguageName;
    SerializedProperty firstSceneBuildInIndex;
    SerializedProperty buildinList;
    SerializedProperty newSceneName;

    SerializedProperty editorType;

    SerializedProperty specificChar;*/

    public grip_Datas datasGrip;
    public bool b_ProjectManagerAssetExist = true;

    //public EditorManipulateTextList manipulateTextList;




    // Add menu item named "Test Mode Panel" to the Window menu
    //[MenuItem("Tools/MCR/GripSurface Creator (w_GripSurfaceCreator)")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(w_GripSurfaceCreator));
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {                       // use to change the GUIStyle
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private Texture2D Tex_01;
    private Texture2D Tex_02;
    private Texture2D Tex_03;
    private Texture2D Tex_04;
    private Texture2D Tex_05;

    public string[] listItemType = new string[] { };

    public List<string> _test = new List<string>();
    public int page = 0;
    public int numberOfIndexInAPage = 50;
    public int seachSpecificID = 0;

    public Color _cGreen = Color.green;
    public Color _cGray = new Color(.9f, .9f, .9f, 1);


   /* public Texture2D eye;

    public string[] listDatas2 = new string[] { "wTextnVoices", "wFeedback", "wUI", "wItem" };      // use to update w_Diary,w_Inventory...
    public string[] listSaveType = new string[] { "PlayerPrefs", ".dat" };

*/

    void OnEnable()
    {
        //manipulateTextList = new EditorManipulateTextList();

        _MakeTexture();


        string objectPath = "Assets/MCR Creator/Assets/Datas/GripDatas.asset";
        datasGrip = AssetDatabase.LoadAssetAtPath(objectPath, typeof(UnityEngine.Object)) as grip_Datas;
        if (datasGrip)
        {
            serializedObject2 = new UnityEditor.SerializedObject(datasGrip);
            listOfSurface = serializedObject2.FindProperty("listOfSurface");

           // helpBoxEditor                   = serializedObject2.FindProperty("helpBoxEditor");
           /* currentDatasProjectFolder       = serializedObject2.FindProperty("currentDatasProjectFolder");
            int_CurrentDatasProjectFolder   = serializedObject2.FindProperty("int_CurrentDatasProjectFolder");
            int_CurrentDatasSaveSystem      = serializedObject2.FindProperty("int_CurrentDatasSaveSystem");
            s_newProjectName                = serializedObject2.FindProperty("s_newProjectName");
            s_newLanguageName               = serializedObject2.FindProperty("s_newLanguageName");
            firstSceneBuildInIndex          = serializedObject2.FindProperty("firstSceneBuildInIndex");
            buildinList                     = serializedObject2.FindProperty("buildinList");
            newSceneName                    = serializedObject2.FindProperty("newSceneName");
            specificChar                    = serializedObject2.FindProperty("specificChar");*/
        }
        else
        {
            b_ProjectManagerAssetExist = false;
        }

      
    }


    void OnGUI()
    {
        //--> Scrollview
        scrollPosAll = EditorGUILayout.BeginScrollView(scrollPosAll);

        //--> Window description
        //GUI.backgroundColor = _cGreen;
        CheckTex();
        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

        //		
        EditorGUILayout.BeginVertical(style_Purple);
        EditorGUILayout.HelpBox("Window Tab : Project Manager", MessageType.Info);
        EditorGUILayout.EndVertical();


        // --> Display data
        EditorGUILayout.BeginHorizontal();
        datasGrip = EditorGUILayout.ObjectField(datasGrip, typeof(UnityEngine.Object), true) as grip_Datas;
        EditorGUILayout.EndHorizontal();

        if (datasGrip != null)
        {

            GUILayout.Label("");

           
            serializedObject2.Update();

            for (var i = 0; i < listOfSurface.arraySize;i++){
                //_Tag
                EditorGUILayout.BeginVertical(style_Yellow_01);

                EditorGUILayout.LabelField( "Surface " + i + "-> "  + listOfSurface.GetArrayElementAtIndex(i).FindPropertyRelative("_Tag").stringValue,EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Tag:", GUILayout.Width(145));
                EditorGUILayout.PropertyField(listOfSurface.GetArrayElementAtIndex(i).FindPropertyRelative("_Tag"), new GUIContent(""));
                EditorGUILayout.EndHorizontal();

                // 1.5 = the car stop very quickly / 5 = the car stop slowly     
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Slow down (1.5 to 5):", GUILayout.Width(145));
                EditorGUILayout.PropertyField(listOfSurface.GetArrayElementAtIndex(i).FindPropertyRelative("CoeffZWhenCarIsSlow"), new GUIContent(""));
                EditorGUILayout.EndHorizontal();

                // Reduce the speed of the car 0 to 1
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Grip Forward (0 to 1):", GUILayout.Width(145));
                EditorGUILayout.PropertyField(listOfSurface.GetArrayElementAtIndex(i).FindPropertyRelative("GripForward"), new GUIContent(""));
                EditorGUILayout.EndHorizontal();

                // Reduce or increase break force 
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Brake Force:", GUILayout.Width(145));
                EditorGUILayout.PropertyField(listOfSurface.GetArrayElementAtIndex(i).FindPropertyRelative("BrakeForce"), new GUIContent(""));
                EditorGUILayout.EndHorizontal();

                // Slide coefficient .1f very little slide / .001 very long slide
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Slide Coeff (0.1f to .001f):", GUILayout.Width(145));
                EditorGUILayout.PropertyField(listOfSurface.GetArrayElementAtIndex(i).FindPropertyRelative("SlideCoeff"), new GUIContent(""));
                EditorGUILayout.EndHorizontal();

                // add rotation to the wheel
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Wheel rotation Y:", GUILayout.Width(145));
                EditorGUILayout.PropertyField(listOfSurface.GetArrayElementAtIndex(i).FindPropertyRelative("SlideEulerAngleY"), new GUIContent(""));
                EditorGUILayout.EndHorizontal();

                if(i > 0){
                    if (GUILayout.Button("Delete Surface"))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(datasGrip, datasGrip.name);
                        datasGrip.listOfSurface.RemoveAt(i);
                        break;
                    }
                }
               

                EditorGUILayout.EndVertical();


                EditorGUILayout.LabelField("");
            }
           
            GUI.backgroundColor = _cGreen;
            if (GUILayout.Button("Create new Surface", GUILayout.Height(60)))
            {
                Undo.RegisterFullObjectHierarchyUndo(datasGrip, datasGrip.name);
                datasGrip.listOfSurface.Add(new grip_Datas._Surface());
            }

            serializedObject2.ApplyModifiedProperties();

            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("");
        }
        EditorGUILayout.EndScrollView();
    }


	void OnInspectorUpdate()
	{
		Repaint();
	}
		
	//--> If texture2D == null recreate the texture (use for color in the custom editor)
	private void CheckTex (){
		if (Tex_01 == null || Tex_02 == null || Tex_03 == null || Tex_04 == null || Tex_05 == null) {
			_MakeTexture ();
		}
	}

	private void _MakeTexture (){
		Tex_01 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
		Tex_04 = MakeTex(2, 2, new Color(.4f,1f,.9F,1f)); 
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); 
	}


	void OnSceneGUI( )
	{
	}

    /*
	public void _helpBox(int value){
		if (helpBoxEditor.boolValue) {
			switch (value) {
			case 0:
				EditorGUILayout.HelpBox ("",MessageType.Info);
				break;
			case 1:
				EditorGUILayout.HelpBox ("",MessageType.Info);
				break;
			case 2:
				EditorGUILayout.HelpBox("",MessageType.Warning);
				break;
			}
		}
	}*/
}
#endif
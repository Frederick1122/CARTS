#if (UNITY_EDITOR)
#if PHOTON_UNITY_NETWORKING
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

[CustomEditor(typeof(carListPUN_MCR))]
public class carListPUN_MCR_Editor : Editor
{
    SerializedProperty SeeInspector;                                  // use to draw default Inspector
    SerializedProperty inventoryCar;                                  // Max length name

    SerializedProperty SeeCarList;                                  // Max length name
    SerializedProperty SeeTrackList;                                  // Max length name
                                                                      //SerializedProperty inventoryOnlineTracks;                         
    SerializedProperty inventoryOnlineTracks;

    SerializedProperty maxPlayerByRoom;
    SerializedProperty maxCPU;


    public Texture2D eye;

    private Texture2D Tex_01;
    private Texture2D Tex_02;
    private Texture2D Tex_03;
    private Texture2D Tex_04;
    private Texture2D Tex_05;

    private Texture2D MakeTex(int width, int height, Color col)
    {       // use to change the GUIStyle
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

    void OnEnable()
    {

        // Setup the SerializedProperties.
        SeeInspector = serializedObject.FindProperty("SeeInspector");
        inventoryCar = serializedObject.FindProperty("inventoryCar");
        SeeCarList = serializedObject.FindProperty("SeeCarList");
        SeeTrackList = serializedObject.FindProperty("SeeTrackList");
        inventoryOnlineTracks = serializedObject.FindProperty("inventoryOnlineTracks");
        maxPlayerByRoom = serializedObject.FindProperty ("maxPlayerByRoom");
        maxCPU = serializedObject.FindProperty("maxCPU");


        Tex_01 = MakeTex(2, 2, new Color(1, .92f, 0.016F, .7f));
        Tex_02 = MakeTex(2, 2, new Color(1, .8f, 0.2F, 1f));
        Tex_03 = MakeTex(2, 2, new Color(.3F, .9f, 1, .5f));
        Tex_04 = MakeTex(2, 2, new Color(1, 0, 1, .5f));
        Tex_05 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        if (SeeInspector.boolValue)                         // If true Default Inspector is drawn on screen
            DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("See Inspector:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""));
        EditorGUILayout.EndHorizontal();




        if (!EditorApplication.isPlaying)
        {

            //GUIStyle style = new GUIStyle(GUI.skin.box);
            GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
            GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
            GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
            GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
            GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

            carListPUN_MCR myScript = (carListPUN_MCR)target;

            //GUILayout.Label("");


            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("How many players by room:", GUILayout.Width(200));
            EditorGUILayout.PropertyField(maxPlayerByRoom, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Max CPU in a Race:", GUILayout.Width(200));
            EditorGUILayout.PropertyField(maxCPU, new GUIContent(""));
            EditorGUILayout.EndHorizontal();





            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("See Car List:", GUILayout.Width(120));
            EditorGUILayout.PropertyField(SeeCarList, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            if (SeeCarList.boolValue)                         // If true Default Inspector is drawn on screen
                displayCars(myScript, style_Yellow_01, style_Blue);



            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("See Track List:", GUILayout.Width(120));
            EditorGUILayout.PropertyField(SeeTrackList, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            if (SeeTrackList.boolValue)                         // If true Default Inspector is drawn on screen
                displayTracksList(myScript, style_Yellow_01, style_Blue);
        }
        serializedObject.ApplyModifiedProperties();
    }


    void displayCars(carListPUN_MCR myScript, GUIStyle Color_01, GUIStyle Color_02)
    {
        #region
        //for(var i = 0;i<)
        //Debug.Log("Here: " + inventoryCar.FindPropertyRelative("inventoryItem").GetArrayElementAtIndex(0).FindPropertyRelative("inventoryItem").GetArrayElementAtIndex(0).name);
        //EditorGUILayout.PropertyField(inventoryCar.FindPropertyRelative("inventoryItem").GetArrayElementAtIndex(0).FindPropertyRelative("Cars").GetArrayElementAtIndex(0), new GUIContent(""));

        EditorGUILayout.PropertyField(inventoryCar, new GUIContent(""));

        SerializedObject serializedObject0 = new UnityEditor.SerializedObject(myScript.inventoryCar);
        serializedObject0.Update();

        SerializedProperty inventoryItem = serializedObject0.FindProperty("inventoryItem");


        for (var i = 0; i < inventoryItem.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(Color_01);
            SerializedProperty cars = inventoryItem.GetArrayElementAtIndex(i).FindPropertyRelative("Cars");


            EditorGUILayout.BeginHorizontal(Color_02);
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                cars.InsertArrayElementAtIndex(cars.arraySize);
                cars.GetArrayElementAtIndex(cars.arraySize - 1).objectReferenceValue = null;
                break;
            }
            GUILayout.Label("Cars: Player " + (i + 1), EditorStyles.boldLabel);

            EditorGUILayout.EndHorizontal();

            for (var j = 0; j < cars.arraySize; j++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(cars.GetArrayElementAtIndex(j), new GUIContent(""));
                if (GUILayout.Button("^", GUILayout.Width(30)))
                {
                    if (j > 0) cars.MoveArrayElement(j, j - 1);
                    break;
                }
                if (GUILayout.Button("v", GUILayout.Width(30)))
                {

                    cars.MoveArrayElement(j, j + 1);
                    break;
                }
                if (cars.arraySize > 1)
                {
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {

                        if (cars.GetArrayElementAtIndex(j).objectReferenceValue == null)
                        {
                            cars.DeleteArrayElementAtIndex(j);
                        }
                        else
                        {
                            cars.DeleteArrayElementAtIndex(j);
                            cars.DeleteArrayElementAtIndex(j);
                        }
                        break;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        // EditorGUILayout.PropertyField(m_championshipInventory.GetArrayElementAtIndex(0).FindPropertyRelative("Cars").GetArrayElementAtIndex(0), new GUIContent(""));




        serializedObject0.ApplyModifiedProperties();
        #endregion
        // myScript.inventoryItemCar.inventoryItem[i].Cars
    }


    void displayTracksList(carListPUN_MCR myScript, GUIStyle Color_01, GUIStyle Color_02)
    {
        // inventoryOnlineTracks

        EditorGUILayout.PropertyField(inventoryOnlineTracks, new GUIContent(""));

        SerializedObject serializedObject0 = new UnityEditor.SerializedObject(myScript.inventoryOnlineTracks);
        serializedObject0.Update();

        SerializedProperty MultiPlayerTrackDisplayedNameList = serializedObject0.FindProperty("MultiPlayerTrackDisplayedNameList");
        SerializedProperty MultiPlayerTrackNameList = serializedObject0.FindProperty("MultiPlayerTrackNameList");
        SerializedProperty MultiPlayerTrackImageList = serializedObject0.FindProperty("MultiPlayerTrackImageList");


        for (var i = 0; i < MultiPlayerTrackDisplayedNameList.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(Color_01);
            SerializedProperty trackUINameList = MultiPlayerTrackDisplayedNameList.GetArrayElementAtIndex(i);
            SerializedProperty trackSceneNameList = MultiPlayerTrackNameList.GetArrayElementAtIndex(i);
            SerializedProperty trackSpriteList = MultiPlayerTrackImageList.GetArrayElementAtIndex(i);


            EditorGUILayout.BeginHorizontal(Color_02);
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                MultiPlayerTrackImageList.InsertArrayElementAtIndex(i+1);
                MultiPlayerTrackImageList.GetArrayElementAtIndex(i + 1).objectReferenceValue = null;

                MultiPlayerTrackDisplayedNameList.InsertArrayElementAtIndex(i+1);
                MultiPlayerTrackDisplayedNameList.GetArrayElementAtIndex(i+1).stringValue = "";

                MultiPlayerTrackNameList.InsertArrayElementAtIndex(i+1);
                MultiPlayerTrackNameList.GetArrayElementAtIndex(i+1).stringValue = "";
                break;
            }
            GUILayout.Label(i + ": ", GUILayout.Width(20));
            EditorGUILayout.PropertyField(trackSpriteList, new GUIContent(""), GUILayout.Width(100));
            EditorGUILayout.PropertyField(trackUINameList, new GUIContent(""), GUILayout.Width(100));
            EditorGUILayout.PropertyField(trackSceneNameList, new GUIContent(""), GUILayout.MinWidth(150));

            if (GUILayout.Button("^", GUILayout.Width(20)))
            {
                if (i > 0)
                {
                    MultiPlayerTrackImageList.MoveArrayElement(i, i - 1);
                    MultiPlayerTrackDisplayedNameList.MoveArrayElement(i, i - 1);
                    MultiPlayerTrackNameList.MoveArrayElement(i, i - 1);
                }
                break;
            }
            if (GUILayout.Button("v", GUILayout.Width(20)))
            {

                //cars.MoveArrayElement(j, j + 1);
                MultiPlayerTrackImageList.MoveArrayElement(i, i + 1);
                MultiPlayerTrackDisplayedNameList.MoveArrayElement(i, i + 1);
                MultiPlayerTrackNameList.MoveArrayElement(i, i + 1);
                break;
            }
            if (MultiPlayerTrackDisplayedNameList.arraySize > 1)
            {
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {

                    if (MultiPlayerTrackImageList.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    {
                        MultiPlayerTrackImageList.DeleteArrayElementAtIndex(i);
                    }
                    else
                    {
                        MultiPlayerTrackImageList.DeleteArrayElementAtIndex(i);
                        MultiPlayerTrackImageList.DeleteArrayElementAtIndex(i);
                    }


                    MultiPlayerTrackDisplayedNameList.DeleteArrayElementAtIndex(i);

                    MultiPlayerTrackNameList.DeleteArrayElementAtIndex(i);

                    break;
                }
            }
            EditorGUILayout.EndHorizontal();

           
            EditorGUILayout.EndVertical();
        }

        serializedObject0.ApplyModifiedProperties();
    }
}
#endif
#endif
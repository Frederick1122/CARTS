// LeaderboardSystemEditor : Descrption : Custom Editor for LeaderboardSystem
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

[CustomEditor(typeof(v2_Championship))]
public class v2_ChampionshipEditor : Editor {
	public bool 			SeeInspector = false;						// use to draw default Inspector

    SerializedProperty 		CheckModification;									// Max length name
	//SerializedProperty 		g_TestSlot;							// Max number scoredisplay on screen

	public string[] options = new string[] {"Easy", "Medium", "Expert"};


	public Texture2D eye;

	private Texture2D 		Tex_01;
	private Texture2D 		Tex_02;
	private Texture2D 		Tex_03;
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

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

		string objectEye = "Assets/MCR Creator/Assets/Textures/Edit/Eye.png";
		eye = AssetDatabase.LoadAssetAtPath (objectEye, typeof(UnityEngine.Object)) as Texture2D;
		// Setup the SerializedProperties.
        CheckModification = serializedObject.FindProperty ("CheckModification");
		//g_TestSlot = serializedObject.FindProperty ("g_TestSlot");

		Tex_01 = MakeTex(2, 2, new Color(1,.92f,0.016F,.7f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,1f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
		Tex_04 = MakeTex(2, 2, new Color(1,0,1,.5f)); 
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); 
	}

	public override void OnInspectorGUI()
	{
		SeeInspector = EditorGUILayout.Foldout(SeeInspector,"Inspector");

		if(SeeInspector)							// If true Default Inspector is drawn on screen
			DrawDefaultInspector();


        if (!EditorApplication.isPlaying)
        {
            serializedObject.Update();
            //GUIStyle style = new GUIStyle(GUI.skin.box);
            GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
            GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
            GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
            GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
            GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

            v2_Championship myScript = (v2_Championship)target;

            GUILayout.Label("");





            // --> Section : Max Score display on screen 

            EditorGUILayout.BeginVertical(style_Yellow_01);
            EditorGUILayout.LabelField("", GUILayout.Width(20));
            SerializedObject serializedObject0 = new UnityEditor.SerializedObject(myScript._championshipInventory);
            serializedObject0.Update();
            SerializedProperty m_championshipInventory = serializedObject0.FindProperty("listOfChampionship");
            if (GUILayout.Button("Create a new Championship"))
            {

                addNewChampionship(m_championshipInventory, myScript);
            }
            serializedObject0.ApplyModifiedProperties();

            EditorGUILayout.LabelField("", GUILayout.Width(20));

            EditorGUILayout.EndVertical();


            EditorGUILayout.LabelField("");

            championshipSetup(style_Yellow_01, style_Purple, style_Orange, myScript);

            int currentModification = CheckModification.intValue;
            myScript.t_TestSlot.GetComponent<Text>().text = myScript._championshipInventory.listOfChampionship[currentModification].championshipName;
            myScript.g_TestSlot.GetComponent<Image>().sprite = myScript._championshipInventory.listOfChampionship[currentModification].championshipIconOn;

            int rnd = UnityEngine.Random.Range(0, 2);

            myScript.g_TestSlot.transform.localScale = new Vector3(
                myScript._championshipInventory.listOfChampionship[currentModification].championshipIconSize.x + .00001f * rnd,
                myScript._championshipInventory.listOfChampionship[currentModification].championshipIconSize.y,
                1);


            serializedObject.ApplyModifiedProperties();
        }
	}


	private void championshipSetup(GUIStyle style_Yellow_01,GUIStyle style_Purple,GUIStyle style_Orange,v2_Championship myScript){
		SerializedObject serializedObject0 = new UnityEditor.SerializedObject(myScript._championshipInventory);
		serializedObject0.Update();
		SerializedProperty m_championshipInventory = serializedObject0.FindProperty("listOfChampionship");

        SerializedProperty unlockTrackInArcadeAndTimeTrial = serializedObject0.FindProperty("UnlockTrackInArcadeAndTimeTrial");

        EditorGUILayout.BeginVertical(style_Orange);
        GUILayout.Label("Global Parameters:",EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Unlock Track In Arcade And Time Trial:", GUILayout.Width(220));
        EditorGUILayout.PropertyField(unlockTrackInArcadeAndTimeTrial, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Erase Championship progression (PlayerPrefs)"))
        {
            PlayerPrefs.DeleteKey("chamionnshipState");
        }


        EditorGUILayout.EndVertical();

        GUILayout.Label("");

		for (var i = 0; i < m_championshipInventory.arraySize; i++) {
			EditorGUILayout.BeginVertical (style_Yellow_01);

            if(CheckModification.intValue == i)
				EditorGUILayout.BeginVertical (style_Purple);
			else
				EditorGUILayout.BeginVertical (style_Orange);

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("", GUILayout.Width (20));
			Rect r = GUILayoutUtility.GetLastRect ();

			if (GUI.Button (new Rect (22, r.y+2, 18, 18), eye, GUIStyle.none)) {

				Undo.RegisterFullObjectHierarchyUndo (myScript._championshipInventory, myScript._championshipInventory.name);
                CheckModification.intValue = i;
                //Debug.Log(myScript._championshipInventory.CheckModification);
			}

			GUILayout.Label ("Championship " + i + " :", EditorStyles.boldLabel);

			if (GUILayout.Button ("^", GUILayout.Width (20))) {
				moveChampionshipUp (m_championshipInventory,i,myScript);
			}
			if (GUILayout.Button ("v", GUILayout.Width (20))) {
				moveChampionshipDown(m_championshipInventory,i,myScript);
			}
			if (m_championshipInventory.arraySize > 1) {
				if (GUILayout.Button ("Remove", GUILayout.Width (60))) {
					removeChampionship (m_championshipInventory, i, myScript);
					break;
				}
			}

			EditorGUILayout.EndHorizontal ();




			EditorGUILayout.Space ();
			EditorGUILayout.EndVertical ();



			SerializedProperty championshipName = m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("championshipName");
			SerializedProperty championshipScenesName = m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("scenesName");
			SerializedProperty AI_Difficulty = m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("AI_Difficulty");
            SerializedProperty scenesSprite = m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("scenesSprite");
            SerializedProperty TracksName = m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("TracksName");


			//SerializedProperty championshipIconOff = m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("championshipIconOff");
			SerializedProperty championshipIconOn = m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("championshipIconOn");
			SerializedProperty championshipIconSize = m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("championshipIconSize");

            SerializedProperty championshipUnlock = m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("UnlockChampionship");
            //SerializedProperty championshipLock = m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("UnlockChampionship");


			EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ("Name:", GUILayout.Width (120));
			EditorGUILayout.PropertyField (championshipName, new GUIContent (""));
			EditorGUILayout.EndHorizontal ();


			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Championship Icon:", GUILayout.Width (120));
			EditorGUILayout.PropertyField (championshipIconOn, new GUIContent (""), GUILayout.Width (120));
			GUILayout.Label ("Scale :", GUILayout.Width (35));
			EditorGUILayout.PropertyField (championshipIconSize, new GUIContent (""), GUILayout.Width (90));
            if (GUILayout.Button("Update All Scales", GUILayout.Width(100)))
            {
                Undo.RegisterFullObjectHierarchyUndo(myScript._championshipInventory,myScript._championshipInventory.name);

                for (var j = 0; j < myScript.slot_01.Count; j++)
                {
                    Undo.RegisterFullObjectHierarchyUndo(myScript.slot_01[j], myScript.slot_01[j].name);
                    myScript.slot_01[j].transform.localScale = new Vector3(championshipIconSize.vector2Value.x, championshipIconSize.vector2Value.y, myScript.slot_01[j].transform.localScale.z);
                }

                for (var j = 0; j < myScript._championshipInventory.listOfChampionship.Count;j++){
                    myScript._championshipInventory.listOfChampionship[j].championshipIconSize.x = championshipIconSize.vector2Value.x;
                    myScript._championshipInventory.listOfChampionship[j].championshipIconSize.y = championshipIconSize.vector2Value.y;

                   
                }
               
            }
			EditorGUILayout.EndHorizontal ();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Unlock championship:", GUILayout.Width(120));
            EditorGUILayout.PropertyField(championshipUnlock, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

           



            GUILayout.Label("");
           
           EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(15));
                GUILayout.Label("Track name:", GUILayout.Width(100));
            GUILayout.Label("Scene:", GUILayout.Width(140));
                GUILayout.Label("Difficulty:", GUILayout.Width(55));
                GUILayout.Label("Track Sprite:", GUILayout.Width(70));
            EditorGUILayout.EndHorizontal();

			// Display scenes name
			for (var j = 0; j < championshipScenesName.arraySize; j++) {
				EditorGUILayout.BeginHorizontal ();
				//GUILayout.Label ("", GUILayout.Width (15));
                GUILayout.Label (j.ToString(), GUILayout.Width (20));

                EditorGUILayout.PropertyField(TracksName.GetArrayElementAtIndex(j), new GUIContent(""), GUILayout.Width(100));


                EditorGUILayout.PropertyField (championshipScenesName.GetArrayElementAtIndex (j), new GUIContent (""), GUILayout.Width (140));

				AI_Difficulty.GetArrayElementAtIndex (j).intValue = EditorGUILayout.Popup (AI_Difficulty.GetArrayElementAtIndex (j).intValue, options, GUILayout.Width (55));

                EditorGUILayout.PropertyField(scenesSprite.GetArrayElementAtIndex(j), new GUIContent(""), GUILayout.Width(90));

				if (GUILayout.Button ("-", GUILayout.Width (20))) {
					removeTrack(m_championshipInventory,i,j,myScript);
					break;
				}
				if (GUILayout.Button ("^", GUILayout.Width (20))) {
					moveTrackUp(m_championshipInventory,i,j,myScript);
					break;
				}
				if (GUILayout.Button ("v", GUILayout.Width (20))) {
					moveTrackDown(m_championshipInventory,i,j,myScript);
					break;
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("", GUILayout.Width (35));
			if (GUILayout.Button ("Add New Track to this championship", GUILayout.Width (245))) {
				addNewTrackToAChampionship(m_championshipInventory,i,myScript);
				break;
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();

			//



			GUILayout.Label ("");
		}

		serializedObject0.ApplyModifiedProperties ();
	
	}

	void addNewChampionship(SerializedProperty m_championshipInventory,v2_Championship myScript){
        PlayerPrefs.DeleteKey("chamionnshipState");
		Undo.RegisterFullObjectHierarchyUndo (myScript, myScript.name);
		m_championshipInventory.InsertArrayElementAtIndex (m_championshipInventory.arraySize);
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative("championshipName").stringValue = "New Championship";
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("scenesName").ClearArray ();
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("scenesName").InsertArrayElementAtIndex (0);
        m_championshipInventory.GetArrayElementAtIndex(m_championshipInventory.arraySize - 1).FindPropertyRelative("scenesSprite").ClearArray();
        m_championshipInventory.GetArrayElementAtIndex(m_championshipInventory.arraySize - 1).FindPropertyRelative("scenesSprite").InsertArrayElementAtIndex(0);

        m_championshipInventory.GetArrayElementAtIndex(m_championshipInventory.arraySize - 1).FindPropertyRelative("TracksName").ClearArray();
        m_championshipInventory.GetArrayElementAtIndex(m_championshipInventory.arraySize - 1).FindPropertyRelative("TracksName").InsertArrayElementAtIndex(0);



		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("AI_Difficulty").ClearArray ();
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("AI_Difficulty").InsertArrayElementAtIndex (0);
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("showInfoInCustomEditor").boolValue = true;
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("championshipIconOn").objectReferenceValue = null;
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("championshipIconOff").objectReferenceValue = null;
		m_championshipInventory.GetArrayElementAtIndex (m_championshipInventory.arraySize - 1).FindPropertyRelative ("championshipIconSize").vector2Value = new Vector2 (.7f, .7f);
	}

	void moveChampionshipUp (SerializedProperty m_championshipInventory,int i,v2_Championship myScript){
		if (i > 0) {
			Undo.RegisterFullObjectHierarchyUndo (myScript, myScript.name);
			m_championshipInventory.MoveArrayElement (i, i - 1);
		}
	}

	void moveChampionshipDown (SerializedProperty m_championshipInventory,int i,v2_Championship myScript){
		if (i < m_championshipInventory.arraySize) {
			Undo.RegisterFullObjectHierarchyUndo (myScript, myScript.name);
			m_championshipInventory.MoveArrayElement (i, i + 1);
		}
	}

	void removeChampionship(SerializedProperty m_championshipInventory,int i,v2_Championship myScript){
		if (m_championshipInventory.arraySize > 1) {
			Undo.RegisterFullObjectHierarchyUndo (myScript, myScript.name);
			m_championshipInventory.DeleteArrayElementAtIndex (i);
            CheckModification.intValue = 0;
		}
        //myScript._championshipInventory.CheckModification = 0;
	}

	void addNewTrackToAChampionship(SerializedProperty m_championshipInventory,int i,v2_Championship myScript){
		Undo.RegisterFullObjectHierarchyUndo (myScript, myScript.name);
		m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("scenesName").InsertArrayElementAtIndex (m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("scenesName").arraySize);
		m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("AI_Difficulty").InsertArrayElementAtIndex (m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("AI_Difficulty").arraySize);
        m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesSprite").InsertArrayElementAtIndex(m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesSprite").arraySize);
        m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("TracksName").InsertArrayElementAtIndex(m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("TracksName").arraySize);



        m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("AI_Difficulty").GetArrayElementAtIndex(m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("AI_Difficulty").arraySize - 1).intValue = 0;
        m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("scenesName").GetArrayElementAtIndex(m_championshipInventory.GetArrayElementAtIndex (i).FindPropertyRelative ("scenesName").arraySize - 1).stringValue = "";
        m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesSprite").GetArrayElementAtIndex(m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesSprite").arraySize - 1).objectReferenceValue = null;
        m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("TracksName").GetArrayElementAtIndex(m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("TracksName").arraySize - 1).stringValue = "";


	}


	void removeTrack(SerializedProperty m_championshipInventory,int i,int j,v2_Championship myScript){
		if (m_championshipInventory.arraySize > 1) {
			Undo.RegisterFullObjectHierarchyUndo (myScript._championshipInventory, myScript._championshipInventory.name);
			myScript._championshipInventory.listOfChampionship [i].AI_Difficulty.RemoveAt(j);
			myScript._championshipInventory.listOfChampionship [i].scenesName.RemoveAt(j);
            myScript._championshipInventory.listOfChampionship[i].scenesSprite.RemoveAt(j);
            myScript._championshipInventory.listOfChampionship[i].TracksName.RemoveAt(j);

			//m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesName").DeleteArrayElementAtIndex (j);
			//m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("AI_Difficulty").DeleteArrayElementAtIndex (j);
		}

	}

	void moveTrackUp(SerializedProperty m_championshipInventory,int i,int j,v2_Championship myScript){
		if (j > 0) {
			Undo.RegisterFullObjectHierarchyUndo (myScript, myScript.name);
			m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesName").MoveArrayElement (j, j - 1);
			m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("AI_Difficulty").MoveArrayElement (j, j - 1);
            m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesSprite").MoveArrayElement(j, j - 1);
            m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("TracksName").MoveArrayElement(j, j - 1);

		}
	}

	void moveTrackDown(SerializedProperty m_championshipInventory,int i,int j,v2_Championship myScript){
		if (m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesName").arraySize > 1) {
			Undo.RegisterFullObjectHierarchyUndo (myScript, myScript.name);
			m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesName").MoveArrayElement (j, j + 1);
			m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("AI_Difficulty").MoveArrayElement (j, j + 1);
            m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("scenesSprite").MoveArrayElement(j, j + 1);
            m_championshipInventory.GetArrayElementAtIndex(i).FindPropertyRelative("TracksName").MoveArrayElement(j, j + 1);

		}
	}

}
#endif
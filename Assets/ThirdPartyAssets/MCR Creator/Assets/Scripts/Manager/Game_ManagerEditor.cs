// Description : Game_ManagerEditor.cs : Works in association with Game_Manager.cs .  Manage game Rules
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Game_Manager))]
public class Game_ManagerEditor : Editor {
	SerializedProperty		SeeInspector;		// use to draw default Inspector
    SerializedProperty      howManyCarsInRace;

    SerializedProperty		numberOfLine;
    SerializedProperty      distanceXbetweenTwoCars;
    SerializedProperty      distanceZbetweenTwoCars;
    SerializedProperty      distanceZbetweenTwoCarsInSameLine;

    SerializedProperty      offsetRoadAi;
    SerializedProperty      randomRangeOffsetRoadAI;

	public LapCounter 		lapCounter;

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

	private Texture2D 		Tex_01;
	private Texture2D 		Tex_02;
	private Texture2D 		Tex_03;
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

	void OnEnable () {
		// Setup the SerializedProperties.
		SeeInspector 			= serializedObject.FindProperty ("SeeInspector");
        howManyCarsInRace 		= serializedObject.FindProperty ("howManyCarsInRace");
        numberOfLine            = serializedObject.FindProperty("numberOfLine");
        distanceXbetweenTwoCars = serializedObject.FindProperty("distanceXbetweenTwoCars");
        distanceZbetweenTwoCars = serializedObject.FindProperty("distanceZbetweenTwoCars");
        distanceZbetweenTwoCarsInSameLine = serializedObject.FindProperty("distanceZbetweenTwoCarsInSameLine");

        offsetRoadAi            = serializedObject.FindProperty("offsetRoadAi");
        randomRangeOffsetRoadAI = serializedObject.FindProperty("randomRangeOffsetRoadAI");

		Tex_01 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.92f,0.016F,.8f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
		Tex_04 = MakeTex(2, 2, new Color(1,.3f,1,.3f)); 
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); 
	}


	public override void OnInspectorGUI()
	{
		if(SeeInspector.boolValue)							// If true Default Inspector is drawn on screen
			DrawDefaultInspector();

		serializedObject.Update ();
		Game_Manager myScript = (Game_Manager)target; 

		GUIStyle style_Yellow_01 		= new GUIStyle(GUI.skin.box);	style_Yellow_01.normal.background 		= Tex_01; 
		GUIStyle style_Blue 			= new GUIStyle(GUI.skin.box);	style_Blue.normal.background 			= Tex_03;
		GUIStyle style_Purple 			= new GUIStyle(GUI.skin.box);	style_Purple.normal.background 			= Tex_04;
		GUIStyle style_Orange 			= new GUIStyle(GUI.skin.box);	style_Orange.normal.background 			= Tex_05; 
		GUIStyle style_Yellow_Strong 	= new GUIStyle(GUI.skin.box);	style_Yellow_Strong.normal.background 	= Tex_02;

		GUILayout.Label("");

		GUILayout.Label("");
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Inspector :",GUILayout.Width(90));
			EditorGUILayout.PropertyField(SeeInspector, new GUIContent (""),GUILayout.Width(30));
		EditorGUILayout.EndHorizontal();



		GUILayout.Label("");
		EditorGUILayout.HelpBox("This script Manager Game Rules.",MessageType.Info);
		GUILayout.Label("");


		SerializedObject serializedObject2 = new UnityEditor.SerializedObject (myScript.inventoryItemCar);
		serializedObject2.Update ();
		SerializedProperty m_b_mobile = serializedObject2.FindProperty ("b_mobile");
		SerializedProperty m_mobileMaxSpeedOffset = serializedObject2.FindProperty ("mobileMaxSpeedOffset");
		SerializedProperty mobileWheelStearingOffsetReactivity = serializedObject2.FindProperty ("mobileWheelStearingOffsetReactivity");

		//SerializedProperty m_b_LapCounter = serializedObject2.FindProperty ("b_LapCounter");


// -->Mobile Options
		EditorGUILayout.BeginVertical (style_Yellow_Strong);




		EditorGUILayout.HelpBox("IMPORTANT : Modification is applied to the entire project in this section." +
			"\n" +
			"\n-You could modify the Max speed for all car on Mobile." +
			"\n" +
			"-You could modify the wheel stearing reactivity for all cars on mobile." +
			"\n",MessageType.Info);

		if (m_b_mobile.boolValue) {
			if (GUILayout.Button ("Cars are setup for Mobile Inputs")) {
				m_b_mobile.boolValue = false;
			}
		} else {
			if (GUILayout.Button ("Cars are setup for Desktop Inputs")) {
				m_b_mobile.boolValue = true;
			}
		}


		if (m_b_mobile.boolValue) {
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Mobile Max Speed Offset :", GUILayout.Width (220));
			EditorGUILayout.PropertyField (m_mobileMaxSpeedOffset, new GUIContent (""));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Mobile Wheel Stearing Offset Reactivity :", GUILayout.Width (220));
			EditorGUILayout.PropertyField (mobileWheelStearingOffsetReactivity, new GUIContent (""));
			EditorGUILayout.EndHorizontal ();
		}



		EditorGUILayout.EndVertical ();


// --> Countdown
/*		EditorGUILayout.BeginVertical (style_Orange);
		EditorGUILayout.HelpBox("IMPORTANT : Modification is applied to the entire project in this section.",MessageType.Info);
		
		if (m_b_Countdown.boolValue) {
			if (GUILayout.Button ("Countdown is activated when game Start")) {
				//b_UseCountdown.boolValue = false;
				m_b_Countdown.boolValue = false;
			}
		} else {
			if (GUILayout.Button ("Countdown is deactivated when game Start")) {
				//b_UseCountdown.boolValue = true;
				m_b_Countdown.boolValue = true;
			}
		}
		EditorGUILayout.EndVertical ();*/

// --> Lap Counter
	/*	EditorGUILayout.BeginVertical (style_Blue);
		EditorGUILayout.HelpBox("IMPORTANT : Modification is applied to the entire project in this section.",MessageType.Info);
		if (!lapCounter) {
			GameObject tmpObj = GameObject.FindGameObjectWithTag ("TriggerStart");
			if (tmpObj)
				lapCounter = tmpObj.GetComponent<LapCounter> ();
		}

		if (lapCounter) {
			//SerializedObject serializedObject1 = new UnityEditor.SerializedObject (lapCounter);
			//serializedObject1.Update ();
			//SerializedProperty m_b_ActivateLapCounter = serializedObject1.FindProperty ("b_ActivateLapCounter");
			//SerializedProperty m_lapNumber = serializedObject1.FindProperty ("lapNumber");


			if (m_b_LapCounter.boolValue) {
				if (GUILayout.Button ("Lap Counter is activated")) {
					m_b_LapCounter.boolValue = false;
				}
			} else {
				if (GUILayout.Button ("Lap Counter is deactivated")) {
					m_b_LapCounter.boolValue = true;
				}
			}

			//serializedObject1.ApplyModifiedProperties ();
		}
		EditorGUILayout.EndVertical ();*/

		serializedObject2.ApplyModifiedProperties ();

		GUILayout.Label("");
		GUILayout.Label("");
		// --> Lap Counter
		EditorGUILayout.BeginVertical (style_Yellow_01);
		EditorGUILayout.HelpBox("The next sections only affect this scene.",MessageType.Info);
		if (!lapCounter) {
			GameObject tmpObj = GameObject.FindGameObjectWithTag ("TriggerStart");
			if (tmpObj)
				lapCounter = tmpObj.GetComponent<LapCounter> ();
		}

		if (lapCounter) {
			SerializedObject serializedObject1 = new UnityEditor.SerializedObject (lapCounter);
			serializedObject1.Update ();
			//SerializedProperty m_b_ActivateLapCounter = serializedObject1.FindProperty ("b_ActivateLapCounter");
			SerializedProperty m_lapNumber = serializedObject1.FindProperty ("lapNumber");

			// --> Lap Numbers
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label( "Lap number:",GUILayout.Width(100));
			EditorGUILayout.PropertyField(m_lapNumber, new GUIContent (""));
			EditorGUILayout.EndHorizontal ();


			serializedObject1.ApplyModifiedProperties ();
		}
		EditorGUILayout.EndVertical ();

		GUILayout.Label("");



        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.HelpBox("Set the number of player for this race.", MessageType.Info);
       
         
            // --> Lap Numbers
            EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Cars number:", GUILayout.Width(250));
            EditorGUILayout.PropertyField(howManyCarsInRace, new GUIContent(""));
            EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Cars per line:", GUILayout.Width(250));
        EditorGUILayout.PropertyField(numberOfLine , new GUIContent(""));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Distance between cars on the same line X:", GUILayout.Width(250));
        EditorGUILayout.PropertyField(distanceXbetweenTwoCars, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Offset between two cars on the same line Z:", GUILayout.Width(250));
        EditorGUILayout.PropertyField(distanceZbetweenTwoCarsInSameLine, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Distance between two lines Z:", GUILayout.Width(250));
        EditorGUILayout.PropertyField(distanceZbetweenTwoCars, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("");
       
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Offset car AI:", GUILayout.Width(250));
        EditorGUILayout.PropertyField(offsetRoadAi, new GUIContent(""));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("random Range Offset Road AI:", GUILayout.Width(250));
        EditorGUILayout.PropertyField(randomRangeOffsetRoadAI, new GUIContent(""));
        EditorGUILayout.EndHorizontal();




        if (GUILayout.Button("Update"))
        {
            List<float> listFloat = new List<float>();
            float newValue = offsetRoadAi.floatValue;
            listFloat.Clear();
            float multipler = 1;
            int counter = 0;

            if(numberOfLine.intValue%2 != 0)
                listFloat.Add(0);

            int howManyEntry = numberOfLine.intValue;
            if(numberOfLine.intValue%2 != 0)
                howManyEntry = numberOfLine.intValue-1;

            for (var i = 0; i < howManyEntry; i++)
            {
                listFloat.Add(newValue );

                counter++;
                if(counter%2 == 0){
                    multipler++;
                    newValue += offsetRoadAi.floatValue;
                }
                counter = counter % 2;
            }

            counter = 0;
            for (var i = 0; i < listFloat.Count; i++)
            {
                if (counter % 2 == 0){
                    listFloat[i] = -listFloat[i]; 
                }
                counter++;  
            }


            string value = "";
            foreach(float val in listFloat)
            {
                value += val + " : ";
            }
            //Debug.Log(value);

            listFloat.Sort();
            value = "";
            foreach (float val in listFloat)
            {
                value += val + " : ";
            }
           // Debug.Log(value);

            //-> Destroy current StartPosition
            GameObject refStartPosition = GameObject.Find("Start_Position_01");

            for (var i = 1; i < 300; i++){
                GameObject tmpPosition = GameObject.Find("Start_Position_0" + (i + 1));
                if(tmpPosition)
                    Undo.DestroyObjectImmediate(tmpPosition);
            }


            //-> Create new Start_Position
            Undo.RegisterFullObjectHierarchyUndo(refStartPosition, refStartPosition.name);
            //float offsetX = distanceXbetweenTwoCars.floatValue;
            float offsetDependingNumberOfLine = distanceXbetweenTwoCars.floatValue * .5f * (numberOfLine.intValue-1);

            refStartPosition.transform.localPosition = new Vector3(0 - offsetDependingNumberOfLine, 
                                                                   -0.4f, 
                                                                   1.2f );
            refStartPosition.name = "Start_Position_0" + howManyCarsInRace.intValue;

            //distanceXbetweenTwoCars
            //distanceZbetweenTwoCars
            float newPos = 0;
            int newModulo = 0;
            for (var i = howManyCarsInRace.intValue-1; i > 0; i--)
            {
                //offsetX = -offsetX;

                GameObject tmpPosition = Instantiate(refStartPosition,refStartPosition.transform.parent);
                tmpPosition.name = "Start_Position_0" + i;

                newModulo++;
                newModulo %= numberOfLine.intValue;

                if (newModulo == 0)
                    newPos += .5f + distanceZbetweenTwoCars.floatValue;


                //numberOfLine
                tmpPosition.transform.localPosition = new Vector3(newModulo*distanceXbetweenTwoCars.floatValue - offsetDependingNumberOfLine, 
                                                                  -0.4f, 
                                                                  1.2f -newPos - distanceZbetweenTwoCarsInSameLine.floatValue * newModulo);

                //tmpPosition.transform.localPosition = new Vector3(offsetX,0,1.5f - howManyCarsInRace.intValue * .5f + (1+i)*.5f);

                Undo.RegisterCreatedObjectUndo(tmpPosition,tmpPosition.name);
            }

            //-> Destroy current target for each car
            GameObject refTarget = GameObject.Find("P1_Target");

            for (var i = 1; i < 300; i++)
            {
                GameObject tmptarget = GameObject.Find("P" + (i + 1) + "_Target");
                if (tmptarget)
                    Undo.DestroyObjectImmediate(tmptarget);
            }

            //-> Create new target for each car
            for (var i = 0; i < howManyCarsInRace.intValue - 1; i++)
            {
                GameObject tmptarget = Instantiate(refTarget);
                tmptarget.name = "P" + (i + 2) + "_Target";
                tmptarget.transform.GetChild(0).name = tmptarget.name + "_Part2";

                Undo.RegisterCreatedObjectUndo(tmptarget, tmptarget.name);
            }


            //-> Create new difficulties parameters
            Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
            for (var i = 0; i < 3;i++){
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].addGlobalSpeedOffset.Clear();
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].waypointSuccesRate.Clear();
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].waypointMinTarget.Clear();
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].waypointMaxTarget.Clear();
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].speedSuccesRate.Clear();
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].speedOffset.Clear();
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].rotationSuccesRate.Clear();
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].rotationOffset.Clear();


                for (var j = 0; j < howManyCarsInRace.intValue-1; j++)
                {
                    if(j % 3 == 0)
                        SetCarValue(myScript, 
                                    i,
                                    -.2f,0,100,100,
                                    -.05f,0,100,100,
                                   .05f,0,100,100);
                    else if (j % 3 == 1)
                        SetCarValue(myScript,
                                    i,
                                    -.1f, 0, 50, 70,
                                    0f, 0, 50, 70,
                                   .15f, 0, 50, 70);
                    else
                        SetCarValue(myScript,
                                    i,
                                    0f, 0, 100, 70,
                                    .1f, 0, 100, 70,
                                   .25f, 0, 100, 70);
                   
                    myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].waypointMinTarget.Add(0);
                    myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].waypointMaxTarget.Add(0);


                    myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].speedOffset.Add(0);

                    myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].rotationOffset.Add(-15);

                    counter++;
                    counter = counter % listFloat.Count;
                }

                counter = 0;
                for (var k = howManyCarsInRace.intValue - 2; k >= 0; k--)
                {
                  
 
                    float newValue2 = returnRandomValue(listFloat[counter]);
                    myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].waypointMinTarget[k] = newValue2;
                    myScript.gameObject.GetComponent<DifficultyManager>().difficulties[i].waypointMaxTarget[k] = newValue2;

                    counter++;
                    counter = counter % listFloat.Count;
                    //Debug.Log("car_" + k);
                }
            }
           

        }

        EditorGUILayout.EndVertical();

        GUILayout.Label("");


		serializedObject.ApplyModifiedProperties ();
	}

    void SetCarValue(Game_Manager myScript,int difficulty,
                     float speedBoose1,int followCircuit1, int OptimizedSpeed1, int OptimizedCarRotation1,
                     float speedBoose2, int followCircuit2, int OptimizedSpeed2, int OptimizedCarRotation2,
                     float speedBoose3, int followCircuit3, int OptimizedSpeed3, int OptimizedCarRotation3){

            
            if(difficulty == 0){
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].addGlobalSpeedOffset.Add(speedBoose1);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].waypointSuccesRate.Add(followCircuit1);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].rotationSuccesRate.Add(OptimizedSpeed1);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].speedSuccesRate.Add(OptimizedCarRotation1);  
            }
            else if (difficulty == 1)
            {
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].addGlobalSpeedOffset.Add(speedBoose2);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].waypointSuccesRate.Add(followCircuit2);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].rotationSuccesRate.Add(OptimizedSpeed2);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].speedSuccesRate.Add(OptimizedCarRotation2);
            }
            else
            {
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].addGlobalSpeedOffset.Add(speedBoose3);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].waypointSuccesRate.Add(followCircuit3);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].rotationSuccesRate.Add(OptimizedSpeed3);
                myScript.gameObject.GetComponent<DifficultyManager>().difficulties[difficulty].speedSuccesRate.Add(OptimizedCarRotation3);
            }
           
        }


	void AddBefore(int value){

	}

    float returnRandomValue(float currentValue){
        currentValue = UnityEngine.Random.Range(currentValue - randomRangeOffsetRoadAI.floatValue, currentValue + randomRangeOffsetRoadAI.floatValue);
        return currentValue;
    }


	void OnSceneGUI( )
	{
	}
}
#endif
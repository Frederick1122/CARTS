// Description : TimeResult.cs : Allow to display car name and car time when a race is finished
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TimeResult : MonoBehaviour {
	public List<Text> 	Player_Name = new List<Text>(); 							// gameObjects to display car name for each car
	public List<Text> 	Player_Time = new List<Text>(); 							// gameObjects to display time for each car
	public List<bool> 	TimeIsCalulated = new List<bool>(); 						// Calculate the time one time for each car
	public LapCounter 	lapCounter;													// access component

	public string 		TextWhenPlayerIsInRace = "Waiting...";						// the text dispaly on screen when a has not ended the race
	public bool 		useCarName = false;											// if True : The name of the car prefab is used on score board

	public string 		stringForPlayerText = "P";									// Text for player 1 or 2
	public string 		stringForCPUText = "CPU ";									// Text for CPU 2,3,4

    public Transform   objScoreContent;
    public GameObject   objScore;
    public int PositionCounter = 1;

    [Header("Championship")]
    public bool         b_btn_NextTrack = false;
    public GameObject   objNextPage;
    public GameObject   objNextTrack;
    public GameObject   objRestartChampionship;
    public Text         txt_ScreenTitle;
    public string       s_NewTitleGlobalresult = "Global Result";
    public string       s_NewTitleFinalResult = "Final Result";
    public GameObject   objTxt_Time;
    public GameObject   objContent;

    public List<Transform> listScore = new List<Transform>();
    public List<int> listPoints = new List<int>();
    public List<int> listCarPosition = new List<int>();
    public GameObject grp_Medal;
    public objStamp objStamp_01;        // Stamp Track Result
    public objStamp objStamp_02;        // Stamp Championship current result
    public objStamp objStamp_03;        // Stamp Championship Final Result
    public UpdateSpriteScale updateSpriteScale; // Display track sprite in theTransition Menu Screen

    public bool b_AudioFadeOut = false;
    public float fadeSpeed = .1f;

    [Header("Championship")]
    public MCR_ChangeButtonNavigation changeBtnNavigation;
    public Button btn_MainMenu;
    public Button btn_NextPage;
    public Button btn_NextTrack;
    public Button btn_RestartChampionship;

    private void Start()
    {
        if (objStamp_01){
                objStamp_01.gameObject.SetActive(true);
                objStamp_01.AP_LogoAnimation();
            }

        changeBtnNavigation = new MCR_ChangeButtonNavigation();

        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(btn_NextPage.gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (lapCounter == null)
            lapCounter = GameObject.Find("StartLine_lapCounter").GetComponent<LapCounter>();

        if(!b_btn_NextTrack){                                                // button Next Track in championship mode is not pressed
            if (lapCounter != null)
            {
                if (TimeIsCalulated.Count != lapCounter.raceFinished.Count)
                {
                    TimeIsCalulated.Clear();
                    for (var i = 0; i < lapCounter.raceFinished.Count; i++)
                    {
                        TimeIsCalulated.Add(false);
                        objScoreContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 40 * lapCounter.raceFinished.Count);
                    }
                }


                if (TimeIsCalulated.Count == lapCounter.raceFinished.Count)
                {
                    for (var i = 0; i < lapCounter.raceFinished.Count; i++)
                    {
                        if (lapCounter.raceFinished[i] && !TimeIsCalulated[i] && PositionCounter == lapCounter.carPosition[i])
                        {
                            MCR_InstantiateScorePrefab(i);
                        }
                    }
                }
            }  
        }


        for (var i = 0; i < lapCounter.carController.Count; i++)
        {
            if (lapCounter.carController[i]) { 
                lapCounter.carController[i].audio_.volume = Mathf.MoveTowards(lapCounter.carController[i].audio_.volume, .1f, Time.deltaTime * fadeSpeed);
                lapCounter.carController[i].objSkid_Sound.volume = Mathf.MoveTowards(lapCounter.carController[i].audio_.volume, 0, Time.deltaTime);
                lapCounter.carController[i].obj_CarImpact_Sound.volume = Mathf.MoveTowards(lapCounter.carController[i].audio_.volume, 0, Time.deltaTime);
            }
        }

	}

    void MCR_InstantiateScorePrefab(int value){
        TimeIsCalulated[value] = true;
        GameObject newObj = Instantiate(objScore, objScoreContent);

        newObj.transform.GetChild(0).GetComponent<Text>().text = PositionCounter.ToString();    // Display position

        PlayerName(value, newObj.transform.GetChild(1).GetComponent<Text>());                       // Display player or CPU name

        newObj.transform.GetChild(2).GetComponent<Text>().text = F_Timer(lapCounter.carTime[value]); // display time
        newObj.name = PositionCounter.ToString();

        listScore.Add(newObj.transform);

        listPoints.Add(lapCounter.carController.Count - PositionCounter + 1);
        listCarPosition.Add(value);

        if(PlayerPrefs.GetString("Which_GameMode") == "Championship")
            newObj.transform.GetChild(3).GetComponent<Text>().text = "+" + (lapCounter.carController.Count - PositionCounter + 1).ToString(); // display Score
                                                                                                                                          // Debug.Log(lapCounter.carController[i].name + " : " + i + " : " + (lapCounter.carController.Count-PositionCounter+1).ToString());
        PositionCounter++; 
    }

    void PlayerName (int value,Text objNameTxt){
		if (lapCounter.carController [value] != null
			&& lapCounter.carController [value].playerNumber == 1) {											// --> Player 1

			objNameTxt.text = stringForPlayerText + 1;					

		}
		else if (
			lapCounter.carController [value] != null
			&& lapCounter.carController [value].playerNumber == 2
			&& !lapCounter.carController [value].b_AutoAcceleration) {											// --> Player 2

			objNameTxt.text = stringForPlayerText + 2;

		}
		else {

			if (useCarName)
                objNameTxt.text = lapCounter.carController [value].name;									// display name
			else
                objNameTxt.text = stringForCPUText + " " + (value+1).ToString();		

		}
	}


// Format result to 00:00:00
	string F_Timer (float value){
		//value += Time.deltaTime;
		string minutes = "";
		if(Mathf.Floor(value / 60) > 0 && Mathf.Floor(value / 60) < 10)
			minutes = Mathf.Floor(value / 60).ToString("0");

		if(Mathf.Floor(value / 60) >  10)
			minutes = Mathf.Floor(value / 60).ToString("00");


		string seconds = Mathf.Floor(value % 60).ToString("00");
		string milliseconds = Mathf.Floor((value*1000) % 1000).ToString("000");

		string result = "";
		if(Mathf.Floor(value / 60) == 0)
			result = seconds + ":" + milliseconds;
		else
			result = minutes + ":" + seconds + ":" + milliseconds;

		return result;
	}


   
    //--> Championship Part
    public void MCR_NextTrackOrRestartChampionship(){
        if(objStamp_01.MCR_CheckIfAnimationEnded()){
            b_btn_NextTrack = true; // Championship Mode: Player press the button next track

            //-> Find the position of all the cars even if they have finnish the race
            for (var i = 0; i < lapCounter.raceFinished.Count; i++)    // Force  raceFinished to true for all the cars
            {
                lapCounter.raceFinished[i] = true;
            }

            int numberOfCarThatHaveFinishTheRace = listScore.Count;

            for (var i = numberOfCarThatHaveFinishTheRace; i < lapCounter.carController.Count; i++)
            {
                for (var j = 0; j < lapCounter.carController.Count; j++)
                {
                    if (lapCounter.carPosition[j] == i + 1)
                    {
                        //Debug.Log("Pos: " + (i + 1).ToString() + " : " + lapCounter.carController[j].name);
                        MCR_InstantiateScorePrefab(j);
                    }
                }
            }


            championshipM championshipManager = GameObject.Find("championshipManager").GetComponent<championshipM>();
            championshipManager.MCR_CalculateResult(listCarPosition, listPoints);  
        }
       
    }



    public void MCR_NextTrackOrRestartChampionship_Part2(List<int> carName,List<int> carPoints)
    {
        objNextPage.SetActive(false);

        b_btn_NextTrack = true;
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

       

        championshipM championshipManager = GameObject.Find("championshipManager").GetComponent<championshipM>();

        // Championship continue
        if (championshipManager.currentTrackInTheList < championshipManager.champInventory.listOfChampionship[championshipManager.currentSelectedChampionship].scenesName.Count - 1)
        {
            objNextTrack.SetActive(true);
            championshipManager.currentTrackInTheList++;
            txt_ScreenTitle.text = s_NewTitleGlobalresult;
            if (objStamp_02){
                objStamp_02.gameObject.SetActive(true);
                objStamp_02.AP_LogoAnimation();
            }
            updateSpriteScale.MCR_UpdateNextTrackSprite();
            //Debug.Log("Next Page");
            eventSystem.SetSelectedGameObject(btn_NextTrack.gameObject);
            changeBtnNavigation.MCR_NewButtonNavigation(btn_MainMenu, btn_NextTrack, "Right");
        }
        // End of the championship
        else
        {
            objRestartChampionship.SetActive(true);
            championshipManager.currentTrackInTheList++;
            txt_ScreenTitle.text = s_NewTitleFinalResult;
            if (objStamp_03){
                objStamp_03.gameObject.SetActive(true);
                objStamp_03.AP_LogoAnimation();
            }
            updateSpriteScale.MCR_UpdateTrackSprite();
            eventSystem.SetSelectedGameObject(btn_MainMenu.gameObject);
            //Debug.Log("Next Track");
            changeBtnNavigation.MCR_NewButtonNavigation(btn_MainMenu, btn_RestartChampionship, "Right");
        }


        objTxt_Time.SetActive(false);


        //Update Score List
        for (var i = 0; i < listScore.Count; i++)
        {
            listScore[i].GetChild(0).gameObject.GetComponent<Text>().text = (i+1).ToString();

            if (carName[i] == 0 &&
            PlayerPrefs.GetInt("HowManyPlayers") == 1)
            {                                           // --> Player 1
                listScore[i].GetChild(1).gameObject.GetComponent<Text>().text = stringForPlayerText + 1;
            }
            else if (
                (carName[i] == 0 ||  carName[i] == 1 )&&
                PlayerPrefs.GetInt("HowManyPlayers") == 2)
            {                                           // --> Player 2
                if (carName[i] == 0)
                    listScore[i].GetChild(1).gameObject.GetComponent<Text>().text = stringForPlayerText + 1;
                if(carName[i] == 1)
                listScore[i].GetChild(1).gameObject.GetComponent<Text>().text = stringForPlayerText + 2;
            }
            else
            {
                if (useCarName)
                    listScore[i].GetChild(1).gameObject.GetComponent<Text>().text = lapCounter.carController[carName[i]].name;                                    // display name
                else
                    listScore[i].GetChild(1).gameObject.GetComponent<Text>().text = stringForCPUText + " " + (carName[i] + 1).ToString();
            }

            listScore[i].GetChild(2).gameObject.SetActive(false);
            listScore[i].GetChild(3).gameObject.GetComponent<Text>().text = carPoints[i].ToString();
        }

        MCR_checkIfNewChampionshipNeedToBeUnlocked(carName,carPoints);



    }

    public void MCR_checkIfNewChampionshipNeedToBeUnlocked(List<int> carName, List<int> carPoints)
    {
        championshipM championshipManager = GameObject.Find("championshipManager").GetComponent<championshipM>();
        //-> championship is finished
       /* Debug.Log("currentTrackInTheList :  " + championshipManager.currentTrackInTheList +
                  "Number of tracks :  " + (championshipManager.champInventory.listOfChampionship[championshipManager.currentSelectedChampionship].scenesName.Count - 1).ToString() +
                  "currentSelectedChampionship :  " + championshipManager.currentSelectedChampionship + 
                  "Number champ :  " + (championshipManager.champInventory.listOfChampionship.Count - 1).ToString());*/
        if (championshipManager.currentTrackInTheList == championshipManager.champInventory.listOfChampionship[championshipManager.currentSelectedChampionship].scenesName.Count &&
            championshipManager.currentSelectedChampionship < championshipManager.champInventory.listOfChampionship.Count-1)
        {
            if(!championshipManager.listChampionshipState[championshipManager.currentSelectedChampionship+1]){    // This championship was never won
                if (PlayerPrefs.GetInt("HowManyPlayers") == 1) // --> Player 1
                {
                    if (carName[0] == 0)
                    {
                        Debug.Log("New Championship Available");
                        if (grp_Medal) grp_Medal.SetActive(true);
                        championshipManager.listChampionshipState[championshipManager.currentSelectedChampionship+1] = true;
                        championshipManager.MCR_Save_ChampionshipData();
                        championshipManager.MCR_UnlockTrackInArcadeAndTimeTrialMode(championshipManager.currentSelectedChampionship + 1);

                    }
                    else
                    {
                        Debug.Log("New Championship not Available");
                    }
                }
                else if (PlayerPrefs.GetInt("HowManyPlayers") == 2)// --> Player 2
                {
                    if (carName[0] == 0 || carName[0] == 1)
                    {
                        Debug.Log("New Championship Available");
                        if (grp_Medal) grp_Medal.SetActive(true);
                        championshipManager.listChampionshipState[championshipManager.currentSelectedChampionship+1] = true;
                        championshipManager.MCR_Save_ChampionshipData();
                        championshipManager.MCR_UnlockTrackInArcadeAndTimeTrialMode(championshipManager.currentSelectedChampionship+1);
                    }
                    else
                    {
                        Debug.Log("New Championship not Available");
                    }
                }  
            }

        }
        else{
            Debug.Log("No more Championship not Available");
        }
    }






    public void MCR_RestartChampionship(){
        championshipM championshipManager = GameObject.Find("championshipManager").GetComponent<championshipM>();
        championshipManager.MCR_Init_Championship();

    }

    public void MCR_LoadNextChampionshipTrack()
    {
        championshipM championshipManager = GameObject.Find("championshipManager").GetComponent<championshipM>();
        championshipManager.MCR_LoadASceneWhenPlayIsPressed_MainMenu();
    }

}

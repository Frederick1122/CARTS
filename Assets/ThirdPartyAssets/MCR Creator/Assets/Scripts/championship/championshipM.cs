using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class championshipM : MonoBehaviour {
    public bool SeeInspector = false;

	public static championshipM 	instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public championshipInventory    champInventory;

    public List<bool> listChampionshipState = new List<bool>();
    public int currentSelectedChampionship = 0;
    public int currentTrackInTheList = 0;

    public List<int> listScore = new List<int>();

    public bool b_InitSave = false;

    public bool b_GlobalSkidMark = true;

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);


        GameObject[] allObjectTaggedWall = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject obj in allObjectTaggedWall)
        {
            obj.layer = 16;
        }

        b_GlobalSkidMark = true;

    }

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);

        if (b_InitSave) PlayerPrefs.DeleteKey("chamionnshipState");

        MCR_Load_ChampionshipData();    // Load championship datas
	}

   /* private void Update()
    {
        if(Input.GetKeyDown("h")){
            MCR_Save_ChampionshipData();
        }
    }*/

    // -> Unlock Track in Arcade and Time Trial Mode
    public void MCR_UnlockTrackInArcadeAndTimeTrialMode(int newChampionatUnlock)
    {
        if(champInventory.UnlockTrackInArcadeAndTimeTrial){
            //Debug.Log(newChampionatUnlock +  " : " + champInventory.listOfChampionship[newChampionatUnlock].scenesName.Count + "_Lock");
            for (var i = 0; i < champInventory.listOfChampionship[newChampionatUnlock].scenesName.Count; i++)
            {
                //Debug.Log(champInventory.listOfChampionship[newChampionatUnlock].scenesName[i].name + "_Lock");
                PlayerPrefs.SetString(champInventory.listOfChampionship[newChampionatUnlock].scenesName[i] + "_Lock", "Unlocked");         // Unlock Scene Game
            }
        }


    }
    // -> Save Championship Data
    public void MCR_Save_ChampionshipData(){
        //Debug.Log("Save Championship datas");
        string saveString = "";
        for (var i = 0; i < listChampionshipState.Count; i++)
        {
            if(listChampionshipState[i] == false) 
                saveString += "False";       
            else 
                saveString += "True";       
                                              
            saveString += ",";
        }
        PlayerPrefs.SetString("chamionnshipState", saveString);     
        //Debug.Log("Championship saved : " + PlayerPrefs.GetString("chamionnshipState"));
    } 


    // -> Load Championship Data
    public void MCR_Load_ChampionshipData(){
        if (PlayerPrefs.HasKey("chamionnshipState"))
        {
            string text = PlayerPrefs.GetString("chamionnshipState");

            char[] separators = { ','};                                     
            string[] strValues = text.Split(separators);

            listChampionshipState.Clear();
            foreach (string str in strValues)
            {
                if(str == "False")
                    listChampionshipState.Add(false);
                if (str == "True")
                    listChampionshipState.Add(true);  
            }
            //Debug.Log("List of unlock championship exist" + PlayerPrefs.GetString("chamionnshipState"));

        }
        else
        {
            Debug.Log("List of unlock championship doesn't exist");
            string saveString = "";
            for (var i = 0; i < champInventory.listOfChampionship.Count;i++){
                if(champInventory.listOfChampionship[i].UnlockChampionship)  // Add bool to the save system
                    saveString += "True";
                else{
                    saveString += "False";
                }
                                                       
                listChampionshipState.Add(champInventory.listOfChampionship[i].UnlockChampionship);   // Add bool to know the state of the championship during this game.
                saveString += ",";
            }
            PlayerPrefs.SetString("chamionnshipState",saveString);

            //Debug.Log(PlayerPrefs.GetString("chamionnshipState"));
        }
    } 


    public void MCR_Init_Championship(){
        listScore.Clear(); 
        currentTrackInTheList = 0;
        MCR_LoadASceneWhenPlayIsPressed_MainMenu();
    }


    public void MCR_LoadASceneWhenPlayIsPressed_MainMenu()
    {                                                                   // Call when button Play is pressed on Main Menu
        StartCoroutine(AsynchronousLoad());
    }

    IEnumerator AsynchronousLoad()                                                                                  // --> Load a scene (Pinball Table)
    {
      
        yield return new WaitForEndOfFrame();
        //currentTrackInTheList
        AsyncOperation a = SceneManager.LoadSceneAsync(champInventory.listOfChampionship[currentSelectedChampionship].scenesName[currentTrackInTheList]);              // Load a scene

        a.allowSceneActivation = false;                                                                                 // Do not activated the loading until allowSceneActivation = true 

        while (!a.isDone)
        {
            //float progress = Mathf.Clamp01(a.progress / 0.9f);

            if (a.progress == 0.9f)
            {                                                                                   // Loading completed
                a.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    public void MCR_CalculateResult(List<int> listCarPosition,List<int> listPoints){  // Update Players score at the end of the race
        if(listScore.Count == 0){                                               // Create the score list
            GameObject tmp = GameObject.Find("Game_Manager");

            if(tmp){
                Game_Manager gameManager = tmp.GetComponent<Game_Manager>();
                for (var i = 0; i < gameManager.list_Cars.Count;i++){
                    listScore.Add(0); 
                }
            }
        }

        for (var i = 0; i < listCarPosition.Count; i++)                   // add points to each player
        {
            listScore[listCarPosition[i]] += listPoints[i];
        }

       

        List<carPositionCompare> playersPositions = new List<carPositionCompare>();                             // Create a list 

        for (int i = 0; i < listCarPosition.Count; i++)
        {                                                                       // Create the list with name and scores
            playersPositions.Add(new carPositionCompare(i, listScore[i]));
        }

        playersPositions.Sort();                                                                                // sort the list
        playersPositions.Reverse();
        /*for (int i = 0; i < listCarPosition.Count; i++)
        {                                                                       // Create the list with name and scores
            Debug.Log("Car : " + playersPositions[i].car + " : Points : " + playersPositions[i].total);
        }*/
        List<int> carName = new List<int>();
        List<int> carPoints = new List<int>();
        for (int i = 0; i < listCarPosition.Count; i++)// Create the list with name and scores
        {
            carName.Add(playersPositions[i].car);
            carPoints.Add(playersPositions[i].total);
        }


        TimeResult result_Championship = GameObject.Find("Result_Championship").GetComponent<TimeResult>();
        result_Championship.MCR_NextTrackOrRestartChampionship_Part2(carName, carPoints);
    }




    // --> Compare car position 1st, 2nd, 3, or 4
    public class carPositionCompare : IComparable<carPositionCompare>
    {
        public int car;
        public int total;

        public carPositionCompare(int newCar, int newtotal)
        {
            car = newCar;
            total = newtotal;
        }

        //This method is required by the IComparable
        //interface. 
        public int CompareTo(carPositionCompare other)
        {
            if (other == null)
            {
                return 1;
            }

            //Return the difference in power.
            return total - other.total;
        }
    }
}

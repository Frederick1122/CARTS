using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTransition : MonoBehaviour {

    public v2_Championship objChampionship;
    public Menu_Manager canvasMainMenu;
    public CanvasGroup canvasGroup;
    public bool b_isLoadingTrack = false;
    public bool b_isMainMenu = true;
    public bool b_WaitBeforeAllowsKey = false;
    public bool b_Once = false;

    public int whichButtonIsPressed = 0; // 0: Next track 1: Restart Championship

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if(b_WaitBeforeAllowsKey){
            if (Input.anyKey && !b_isLoadingTrack && b_isMainMenu)
            {      // Main Menu Scene
                b_isLoadingTrack = true;
                StartCoroutine(MCR_I_LoadNewTrackChampionship_MainMenu());
            }
            else if (Input.anyKey && !b_isLoadingTrack && !b_isMainMenu) // Track Scene
            {
                b_isLoadingTrack = true;
                StartCoroutine(MCR_I_LoadNewTrackChampionship_Track());
            } 
        }
        else{
            if(!b_Once){
                b_Once = true;
                StartCoroutine(MCR_Wait());
            }
        }

	}

    IEnumerator MCR_Wait()
    {
        yield return new WaitForSeconds(.5f);
        b_WaitBeforeAllowsKey = true;
        yield return null;
    }

    IEnumerator MCR_I_LoadNewTrackChampionship_MainMenu(){
        canvasMainMenu.GoToOtherPage(canvasGroup);

        objChampionship.MCR_LoadFirstChampionshipTrack();
        yield return null;
    }


    IEnumerator MCR_I_LoadNewTrackChampionship_Track()
    {
        canvasMainMenu.GoToOtherPage(canvasGroup);


        if(whichButtonIsPressed == 0){          // Next Track
            MCR_LoadNextChampionshipTrack(); 
        }
        else if(whichButtonIsPressed == 1){     // Restart Championship
            MCR_RestartChampionship();
        }
         

        yield return null;
    }

    public void MCR_RestartChampionship()
    {
        championshipM championshipManager = GameObject.Find("championshipManager").GetComponent<championshipM>();
        championshipManager.MCR_Init_Championship();

    }

    public void MCR_LoadNextChampionshipTrack()
    {
        championshipM championshipManager = GameObject.Find("championshipManager").GetComponent<championshipM>();
        championshipManager.MCR_LoadASceneWhenPlayIsPressed_MainMenu();
    }

    public void MCR_Change_whichButtonIsPressed(int value){
        whichButtonIsPressed = value;
    }
}

// Description : MainMenuManageChooses.cs : Save info when player navigate on menus. These info are used when a scene is loading
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManageChooses : MonoBehaviour {


	public string b_ModeSoloMulti 	= "Solo";
	public string b_GameMode 		= "Arcade";


    [Header("Buttons Navigation")]
    private MCR_ChangeButtonNavigation changeBtnNavigation;
    public Button btn_BackSelectMode;
    public Button btn_ValidateChampionship;
    public Button btn_ValidateArcadeTrial;





    public void Start()
    {
       changeBtnNavigation = new MCR_ChangeButtonNavigation();

    }

    // --> Know if payer want to play on solo mode or multiplayer mode
    public void Choose_Solo_Or_Multi (string newMode){									
		b_ModeSoloMulti = newMode;
		if (b_ModeSoloMulti == "Solo")														// Button solo on Page Hub Menu
			PlayerPrefs.SetInt ("HowManyPlayers", 1);										
		else if (b_ModeSoloMulti == "Multi")												// Button Versus on Page Hub Menu
			PlayerPrefs.SetInt ("HowManyPlayers", 2);
	}


// --> Know if player want to play Arcade Mode or Time Trial Mode.
	public void Choose_GameMode (string newMode){										
		b_GameMode = newMode;																
		if (newMode == "Arcade")													// button Arcade on Page_RaceModeSelection
			PlayerPrefs.SetString ("Which_GameMode", newMode);
		else if (newMode == "TimeTrial")											// button TimeTrial on Page_RaceModeSelection
			PlayerPrefs.SetString ("Which_GameMode", newMode);
        else if (newMode == "Championship")                                            // button TimeTrial on Page_RaceModeSelection
            PlayerPrefs.SetString("Which_GameMode", newMode);


       // GameObject obj_CheckCarSelection = GameObject.Find("CheckCarSelection");
        GameObject Obj_TrackSelection = GameObject.Find("TrackSelection");
        GameObject Obj_Reference = GameObject.Find("ObjectsReference");

        if (newMode == "Arcade" || newMode == "Championship"){
            if (Obj_Reference.GetComponent<objRef_MainMenu>().showMoreThan4Cars && Obj_TrackSelection)
            {
                if (b_ModeSoloMulti == "Solo")
                {
                    Obj_TrackSelection.GetComponent<TrackSelection>().J2.SetActive(false);
                    Obj_TrackSelection.GetComponent<TrackSelection>().J3.SetActive(false);
                    Obj_TrackSelection.GetComponent<TrackSelection>().J4.SetActive(false);
                    if (Obj_Reference)
                    {
                        if (Obj_Reference.GetComponent<objRef_MainMenu>().showMoreThan4Cars && !Obj_Reference.GetComponent<objRef_MainMenu>().showOnlyP1andP2)
                        {
                            Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(true);
                            Obj_Reference.GetComponent<objRef_MainMenu>().J4Plus_J2_Camera3D.SetActive(true);
                        }
                        else
                        {
                            Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(false);
                        }
                    }
                }
                else if (b_ModeSoloMulti == "Multi")
                {
                    Obj_TrackSelection.GetComponent<TrackSelection>().J3.SetActive(false);
                    Obj_TrackSelection.GetComponent<TrackSelection>().J4.SetActive(false);

                    if (Obj_Reference)
                    {
                        if (Obj_Reference.GetComponent<objRef_MainMenu>().showMoreThan4Cars && !Obj_Reference.GetComponent<objRef_MainMenu>().showOnlyP1andP2)
                        {
                            Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(true);
                            Obj_Reference.GetComponent<objRef_MainMenu>().J4Plus_J2_Camera3D.SetActive(false);
                        }
                        else
                        {
                            Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(false);
                        }
                    }

                }

            }
            else
            {
                if (b_ModeSoloMulti == "Solo")
                {
                    Obj_TrackSelection.GetComponent<TrackSelection>().J2.SetActive(true);
                    Obj_TrackSelection.GetComponent<TrackSelection>().J3.SetActive(true);
                    Obj_TrackSelection.GetComponent<TrackSelection>().J4.SetActive(true);
                    if (Obj_Reference)
                        Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(false);
                }
                else if (b_ModeSoloMulti == "Multi")
                {
                    Obj_TrackSelection.GetComponent<TrackSelection>().J2.SetActive(true);
                    Obj_TrackSelection.GetComponent<TrackSelection>().J3.SetActive(true);
                    Obj_TrackSelection.GetComponent<TrackSelection>().J4.SetActive(true);
                    if (Obj_Reference)
                        Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(false);
                }
            }
        }
        else if(newMode == "TimeTrial"){
            Obj_TrackSelection.GetComponent<TrackSelection>().J2.SetActive(false);
            Obj_TrackSelection.GetComponent<TrackSelection>().J3.SetActive(false);
            Obj_TrackSelection.GetComponent<TrackSelection>().J4.SetActive(false);
            if (Obj_Reference)
                Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(false);
        }


	}

    public void MCR_PressButtonChampionship()
    {
        changeBtnNavigation.MCR_NewButtonNavigation(btn_BackSelectMode, btn_ValidateChampionship, "Down");
    }
    public void MCR_PressButtonArcadeTrial()
    {
        changeBtnNavigation.MCR_NewButtonNavigation(btn_BackSelectMode, btn_ValidateArcadeTrial, "Down");
    }
    public void MCR_PressButtonBackFromChampionship()
    {
        changeBtnNavigation.MCR_NewButtonNavigation(btn_BackSelectMode, btn_ValidateChampionship, "Down");
    }
    public void MCR_PressButtonBackFromArcadeTrial()
    {
        changeBtnNavigation.MCR_NewButtonNavigation(btn_BackSelectMode, btn_ValidateArcadeTrial, "Down");
    }

    public void MCR_PressButtonOnlineMultiplayer()
    {
        StartCoroutine(E_Load_LCD("MCR_Lobby"));
    }

    private IEnumerator E_Load_LCD(string value)
    {
        GameObject tmpMenu = GameObject.Find("Canvas_MainMenu");
        if (tmpMenu)
            tmpMenu.GetComponent<Menu_Manager>().GoToOtherPageWithHisNumber(10);                   // Open the Loading menu

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        AsyncOperation a = SceneManager.LoadSceneAsync(value, LoadSceneMode.Single);
        a.allowSceneActivation = false;
        while (a.isDone)
        {
            Debug.Log("loading " + value + " : " + a.progress);
            yield return null;
        }
        a.allowSceneActivation = true;
    }
}

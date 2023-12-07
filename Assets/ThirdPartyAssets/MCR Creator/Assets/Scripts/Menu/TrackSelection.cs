// Description : TrackSelection : Use to  Init the Main menu when you came back from the track selection
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour {

	public GameObject 		J2;
	public GameObject 		J3;
	public GameObject 		J4;
	public CarSelection		carSelection;
	public GameObject 		button_Trial;
	public Text 			TextPlayer2;
	public GameObject 		Buttons_Choose_Car;


	/*public void Start(){
		Debug.Log (
			PlayerPrefs.GetString ("Which_GameMode") + " : " + 
			PlayerPrefs.GetInt ("HowManyPlayers") + " : " +
			PlayerPrefs.GetString ("Player_0_Car") + " : " +
			PlayerPrefs.GetString ("Player_1_Car") + " : " +
			PlayerPrefs.GetString ("Player_2_Car") + " : " +
			PlayerPrefs.GetString ("Player_3_Car"));

	}*/

// --> Init the Main menu when you came back from the track selection
	public void BackFromTrackSelection(){
       // GameObject obj_CheckCarSelection = GameObject.Find("CheckCarSelection");
        GameObject Obj_Reference = GameObject.Find("ObjectsReference");
        Debug.Log(Obj_Reference);

        if (PlayerPrefs.GetString ("Which_GameMode") == "Arcade" || PlayerPrefs.GetString("Which_GameMode") == "Championship") {												// ARcade mode is selected
            if (Obj_Reference.GetComponent<objRef_MainMenu>().showMoreThan4Cars)
            {
                if (PlayerPrefs.GetInt("HowManyPlayers") == 1)
                {
                    if (J2) J2.SetActive(false);
                    if (J3) J3.SetActive(false);
                    if (J4) J4.SetActive(false);
                }
                else if(PlayerPrefs.GetInt("HowManyPlayers") == 2)
                {
                    if (J2) J2.SetActive(true);
                    if (J3) J3.SetActive(false);
                    if (J4) J4.SetActive(false);
                    if (Obj_Reference)
                        Obj_Reference.GetComponent<objRef_MainMenu>().J4Plus_J2_Camera3D.SetActive(false);
                }
                MCR_ActivateMoreThan4Cars();
            }
            else{
                if (J2) J2.SetActive(true);
                if (J3) J3.SetActive(true);
                if (J4) J4.SetActive(true);  
            }
			
		} else if (PlayerPrefs.GetString ("Which_GameMode") == "TimeTrial") {									// Time Trial is selected
			if (J2)J2.SetActive (false);
			if (J3)J3.SetActive (false);
			if (J4)J4.SetActive (false);
          
            if (Obj_Reference)
                Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(false);
		}


		if (PlayerPrefs.GetInt ("HowManyPlayers") == 1) {														// Button solo on Page Hub Menu
			if (button_Trial)button_Trial.SetActive (true);
			if (TextPlayer2)TextPlayer2.text = "CPU";
			if (Buttons_Choose_Car)Buttons_Choose_Car.SetActive (false);
		}
		else if(PlayerPrefs.GetInt ("HowManyPlayers") == 2){													// Button Versus on Page Hub Menu
			if (button_Trial)button_Trial.SetActive (false);
			if (TextPlayer2)TextPlayer2.text = "P2";
			if (Buttons_Choose_Car)Buttons_Choose_Car.SetActive (true);
            MCR_ActivateMoreThan4Cars();
		}

		if(carSelection)carSelection.initCarSelectionFromTrackSelection ();
	}




    void MCR_ActivateMoreThan4Cars(){
        GameObject Obj_Reference = GameObject.Find("ObjectsReference");

        if (Obj_Reference)
        {
            if (Obj_Reference.GetComponent<objRef_MainMenu>().showMoreThan4Cars && !Obj_Reference.GetComponent<objRef_MainMenu>().showOnlyP1andP2)
            {
                Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(true);
            }
            else
            {
                Obj_Reference.GetComponent<objRef_MainMenu>().carSelection_J4Plus.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MCR
{
    public class PauseMobile : MonoBehaviour
    {
        public Menu_Manager canvas_MainMenu;
        private EventSystem eventSystem;                                        // access the evenSystem 


        public void DisplayUIPauseMenu()
        {
            //GameObject tmpMenu = GameObject.Find("Canvas_MainMenu");
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();


            // Display Menu Online Multiplayer
            if (PlayerPrefs.GetString("Which_GameMode") == "OnlineMultiPlayer")
            {
                if (canvas_MainMenu)
                {
                    canvas_MainMenu.GetComponent<Menu_Manager>().GoToOtherPageWithHisNumber(15);                   // Open the menu Online Pause
                    GameObject tmpButton = GameObject.Find("btn_ResumeOnline");                                   // Select the button "btn_ResumeOnline"
                    if (tmpButton)
                    {
                        eventSystem.SetSelectedGameObject(tmpButton);
                        eventSystem.gameObject.GetComponent<StandaloneInputModule>().submitButton = "Submit";
                    }
                }

            }
            else // Display Menu (Arcade,Time Trial, Championship Mode)
            {
                if (canvas_MainMenu)
                {
                    canvas_MainMenu.GetComponent<Menu_Manager>().GoToOtherPageWithHisNumber(0);                   // Open the menu settings 
                    GameObject tmpButton = GameObject.Find("btn_Sound");                                   // Select the button "btn_Sound"
                    if (tmpButton)
                    {
                        eventSystem.SetSelectedGameObject(tmpButton);
                        eventSystem.gameObject.GetComponent<StandaloneInputModule>().submitButton = "Submit";
                    }
                }

                GameObject tmpPause = GameObject.Find("PauseManager");

                if (tmpPause)
                {
                    tmpPause.GetComponent<PauseManager>().PauseGame();                                    // Start Pause Game 
                }
            }

        }
    }
}


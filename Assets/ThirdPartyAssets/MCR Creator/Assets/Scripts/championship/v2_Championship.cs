//Description : v2_Championship : Manage the championship creation in the menu scene. Mangage the menu navigation to ingame.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class v2_Championship : MonoBehaviour
{

    public Text t_TestSlot;                                                 // Test 
    public GameObject g_TestSlot;													// Test Image			
    public championshipM champManager;
    public championshipInventory _championshipInventory;                                        // Champioinship Datas

    public List<GameObject> slot_01 = new List<GameObject>() { null, null, null };          // Represent Image for slot 1, 2, 3
    public List<GameObject> Lock_01 = new List<GameObject>() { null, null, null };          // Represent Lock for slot 1, 2, 3

    [SerializeField]
    public List<Text> _Text = new List<UnityEngine.UI.Text>() { null, null, null }; // Represent Text for slot 1, 2, 3

    public int currentstartValue = 0;                                      // use to display the championship in the scene view

    public GameObject lastChampionship;                                         // represent gameObject button next championship
    public GameObject nextChampionship;                                         // represent gameObject button last championship

    public int CheckModification = 0;                                       // the current championship selected in the Inspector
    public EventSystem eventSystem;
    public GameObject newSelectedGameObject_01;
    public GameObject newSelectedGameObject_02;

    public int currentSelectedChampionship = 0;


    public List<GameObject> objSelector = new List<GameObject>() { null, null, null };

    public List<Text> listTextTrackNumberInTheChamp = new List<Text>();

    public List<GameObject> listPadlock = new List<GameObject>();

    public string onlyOneTrack = "Track";
    public string moreThanOneTrack = "Tracks";

    public int lastSelectedChampionship = 0;
    public GameObject btn_Validate;
    public GameObject Grp_YesNo_Start;
    public AudioSource audioClick;
    public AudioSource audioWrong;

    // Use this for initialization
    void Start()
    {
        if (champManager == null)
            champManager = GameObject.Find("championshipManager").GetComponent<championshipM>();

       
         if (championshipM.instance.currentSelectedChampionship <= 2)
         {
            currentstartValue = 0;
            InitChampionshipWhenScenStart(championshipM.instance.currentSelectedChampionship);
            eventSystem.SetSelectedGameObject(slot_01[championshipM.instance.currentSelectedChampionship]);
            objSelector[championshipM.instance.currentSelectedChampionship].SetActive(true);
            if (lastChampionship.activeSelf)
                lastChampionship.SetActive(false);
         }
         else if(championshipM.instance.currentSelectedChampionship > 2){
             currentstartValue = championshipM.instance.currentSelectedChampionship - 2;
            InitChampionshipWhenScenStart(championshipM.instance.currentSelectedChampionship);
            eventSystem.SetSelectedGameObject(slot_01[2]);
            objSelector[2].SetActive(true);
         }

        if (nextChampionship.activeSelf && championshipM.instance.champInventory.listOfChampionship.Count < 3)
            nextChampionship.SetActive(false);

        //Debug.Log("Champ: " + championshipM.instance.currentSelectedChampionship + " : modulo : " + championshipM.instance.currentSelectedChampionship % 3);
    }


    //--> Display championship in the UI Menu
    private void InitChampionshipWhenScenStart(int startValue)
    {
        int _counter = 0;
        for (var i = 2; i >= 0 ; i--)
        {
            int _value = 0;

            if(championshipM.instance.currentSelectedChampionship <= 2){
                _value = i;
            }
            else{
                _value = startValue - _counter;
            }

            if (!slot_01[i].gameObject.activeSelf)
                slot_01[i].gameObject.SetActive(true);


            slot_01[i].GetComponent<RectTransform>().localScale =
                          new Vector3(_championshipInventory.listOfChampionship[_value].championshipIconSize.x,
                                      _championshipInventory.listOfChampionship[_value].championshipIconSize.y,
                       1);

            _Text[i].text = _championshipInventory.listOfChampionship[_value].championshipName;
            slot_01[i].GetComponent<Image>().sprite = _championshipInventory.listOfChampionship[_value].championshipIconOn;


            string sTrack = moreThanOneTrack;
            if (_championshipInventory.listOfChampionship[_value].TracksName.Count <= 1)
                sTrack = onlyOneTrack;

            listTextTrackNumberInTheChamp[i].text = (_championshipInventory.listOfChampionship[_value].TracksName.Count).ToString() + " " + sTrack;

            if (champManager.listChampionshipState[_value])
                listPadlock[i].SetActive(false);
            else
                listPadlock[i].SetActive(true);


            _counter++;
        }
    }
   

    //--> Display championship in the UI Menu
    private void InitChampionship(int startValue)
    {
        if (currentstartValue + 3 < _championshipInventory.listOfChampionship.Count)
        {
            nextChampionship.SetActive(true);
        }
        else
        {
            nextChampionship.SetActive(false);
            eventSystem.SetSelectedGameObject(newSelectedGameObject_02);
        }

        if (currentstartValue == 0)
        {
            lastChampionship.SetActive(false);
            eventSystem.SetSelectedGameObject(newSelectedGameObject_01);
        }
        else
        {
            lastChampionship.SetActive(true);
        }

        champManager = GameObject.Find("championshipManager").GetComponent<championshipM>();

        //Debug.Log("Start value: " + startValue);

        for (var i = startValue; i < 3 + startValue; i++)
        {
            if (i < _championshipInventory.listOfChampionship.Count)
            {
                if (!slot_01[i - startValue].gameObject.activeSelf)
                    slot_01[i - startValue].gameObject.SetActive(true);

                slot_01[i - startValue].GetComponent<RectTransform>().localScale =
                    new Vector3(_championshipInventory.listOfChampionship[i].championshipIconSize.x,
                        _championshipInventory.listOfChampionship[i].championshipIconSize.y,
                        1);

                slot_01[i - startValue].GetComponent<Image>().sprite = _championshipInventory.listOfChampionship[i].championshipIconOff;

                slot_01[i - startValue].GetComponent<Image>().sprite = _championshipInventory.listOfChampionship[i].championshipIconOn;

                _Text[i - startValue].text = _championshipInventory.listOfChampionship[i].championshipName;

                string sTrack = moreThanOneTrack;
                if (_championshipInventory.listOfChampionship[i].TracksName.Count <= 1)
                    sTrack = onlyOneTrack;

                listTextTrackNumberInTheChamp[i - startValue].text = (_championshipInventory.listOfChampionship[i].TracksName.Count).ToString() + " " + sTrack;


                if(champManager.listChampionshipState[i])
                    listPadlock[i - startValue].SetActive(false);
                else
                    listPadlock[i - startValue].SetActive(true);
            }
            else
            {
                _Text[i - startValue].text = "";
                slot_01[i - startValue].gameObject.SetActive(false);
                listTextTrackNumberInTheChamp[i - startValue].text = "";
                listPadlock[i - startValue].SetActive(false);
            }


        }
    }


    //--> Call by UI Button next_Championship
    public void NextChamp()
    {
        if (currentstartValue + 3 < _championshipInventory.listOfChampionship.Count)
            currentstartValue++;
        InitChampionship(currentstartValue);
    }

    //--> Call by UI Button Last_Championship
    public void LastChamp()
    {
        if (currentstartValue > 0)
            currentstartValue--;
        InitChampionship(currentstartValue);
    }



    public void SelectANewChampionship(int slotValue)
    {
        currentSelectedChampionship = currentstartValue + slotValue;
        if (champManager == null)
            champManager = GameObject.Find("championshipManager").GetComponent<championshipM>();

        Debug.Log(currentSelectedChampionship);

        if (champManager.listChampionshipState[currentSelectedChampionship] == true){
            champManager.currentSelectedChampionship = currentSelectedChampionship;
            MCR_RememberChampionship(slotValue);
            Grp_YesNo_Start.SetActive(true);
            eventSystem.SetSelectedGameObject(btn_Validate);
            audioClick.Play();
            Debug.Log("Championship can be played");
        }
        else{
            Debug.Log("Championship can't be played");
            audioWrong.Play();
        }


    }



    public void MCR_LoadFirstChampionshipTrack(){
        if (champManager == null)
            champManager = GameObject.Find("championshipManager").GetComponent<championshipM>();


        champManager.MCR_LoadASceneWhenPlayIsPressed_MainMenu();
    }


    public void MCR_RememberChampionship(int value){
        lastSelectedChampionship = value;
    }

    public void MCR_SelectLastChampionship()
    {
        
        eventSystem.SetSelectedGameObject(slot_01[lastSelectedChampionship]);
    }
}

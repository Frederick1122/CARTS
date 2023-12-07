#if PHOTON_UNITY_NETWORKING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResultPageOnlineMulti_MCR : MonoBehaviour
{
    private MCR.GameManager_MCR_Photon gameManager_MCR_Photon;
    public GameObject btnRestart;
    public GameObject btnScore;
    public GameObject btnLeaveGame;
    public GameObject btnAccessNewTrack;
    public GameObject txtWaitingForOtherPlayers;

    public GameObject gPanelSore;
    public GameObject gPanelTrackSelection;

    public Image ImTrackSelection;
    public Text txtTrackSelection;

    public Text txtTime;

    public GameObject btnPreviousTrack;
    public GameObject btnNextTrack;
    public GameObject btnRandomTrack;

    public GameObject OnlineResult;
    public Transform resultScrollView;

    private Game_Manager gManager;

    public List<GameObject> _OnlineResultList = new List<GameObject>();
    public List<GameObject> _TotalOnlineResultList = new List<GameObject>();

    private int _counter = 0;

    private MCR.GameManager_MCR_Photon PhotonManager;

    public objStamp objStamp_01;        // Stamp Track Result
    public objStamp objStamp_02;        // Stamp Championship current result
    private bool b_ScoreDisplayer = false;

    private EventSystem eventSystem;

    public void Awake()
    {
        #region
        btnRestart.SetActive(false);
        btnLeaveGame.SetActive(false);
        txtWaitingForOtherPlayers.SetActive(true);
        btnScore.SetActive(false);
        btnAccessNewTrack.SetActive(false);

        gManager  = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

        GameObject tmpObj = GameObject.Find("GM_Photon");
        if(tmpObj)PhotonManager = tmpObj.GetComponent<MCR.GameManager_MCR_Photon>();

        tmpObj = GameObject.Find("EventSystem");
        if (tmpObj) eventSystem = tmpObj.GetComponent<EventSystem>();

        #endregion
    }

    private void Update()
    {
        #region
        if (!gameManager_MCR_Photon)
        {
            GameObject obj = GameObject.Find("GM_Photon");

            if (obj)
                gameManager_MCR_Photon = obj.GetComponent<MCR.GameManager_MCR_Photon>();
        }


        if (gameManager_MCR_Photon && btnRestart && !btnRestart.activeSelf && gameManager_MCR_Photon.b_IsMasterClient && gManager.b_initDone)
        {
            bool b_AllThePlayerEndedTheTrack = true; ;
            for(var i = 0; i < gameManager_MCR_Photon.tmpCarRaceEndedList.Count; i++)
            {
                if(gameManager_MCR_Photon.tmpCarRaceEndedList[i] == false)
                {
                    b_AllThePlayerEndedTheTrack = false;
                    break;
                }
            }


            if (b_AllThePlayerEndedTheTrack && !btnAccessNewTrack.activeSelf && !btnScore.activeInHierarchy)
            {
                if (PhotonManager) PhotonManager.initTrackImageInGame();
                btnScore.SetActive(true);
                txtWaitingForOtherPlayers.SetActive(false);
                if (eventSystem)
                {
                    eventSystem.SetSelectedGameObject(btnScore);
                    eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Submit";
                }
           }
           else if (btnScore.activeSelf && btnAccessNewTrack.activeSelf)
           {

                btnScore.SetActive(false);
           }
        }

        /*if (gameManager_MCR_Photon && !gameManager_MCR_Photon.b_IsMasterClient && gManager.b_initDone)
        {
            Debug.Log("Here 0");

            if (!btnAccessNewTrack.activeSelf && !btnLeaveGame.activeInHierarchy && objStamp_01 && !objStamp_01.gameObject.activeInHierarchy)
            {
                Debug.Log("Here 1");
                if (PhotonManager) PhotonManager.initTrackImageInGame();
                if (objStamp_01)
                {
                    Debug.Log("Here 3");
                    objStamp_01.gameObject.SetActive(true);
                    objStamp_01.AP_LogoAnimation();
                }
                btnLeaveGame.SetActive(true);
            }
           
        }*/


        #endregion
    }

    public void OnClickButtonLoadNewTrack()
    {
        #region
        if (!gameManager_MCR_Photon)
        {
            GameObject obj = GameObject.Find("GM_Photon");

            if (obj)
            {
                gameManager_MCR_Photon = obj.GetComponent<MCR.GameManager_MCR_Photon>();
                gameManager_MCR_Photon.NewRaceButtonClicked();
            }
        }
        else
        {
            gameManager_MCR_Photon.NewRaceButtonClicked();
        }
        #endregion
    }

    public void OnClickButtonTotalScorOnline()
    {
        #region
        btnScore.SetActive(false);
        btnAccessNewTrack.SetActive(true);
        btnRestart.SetActive(false);
        btnLeaveGame.SetActive(true);
        txtWaitingForOtherPlayers.SetActive(false);

        if (objStamp_02)
        {
            objStamp_02.gameObject.SetActive(true);
            objStamp_02.AP_LogoAnimation();
        }

        if (!gameManager_MCR_Photon)
        {
            GameObject obj = GameObject.Find("GM_Photon");
            if (obj)
            {
                gameManager_MCR_Photon = obj.GetComponent<MCR.GameManager_MCR_Photon>();
                gameManager_MCR_Photon.displayOnlineTotalScore();
            }
        }
        else
        {
            gameManager_MCR_Photon.displayOnlineTotalScore();
        }

        // Display Stamp Global result and display leave game button for all clients except the master client
        if (PhotonManager) PhotonManager.totalScoreAllClient();

        if (eventSystem)
            eventSystem.SetSelectedGameObject(btnAccessNewTrack);

        #endregion
    }

    public void TotalScorOnlineAllClient()
    {
        #region
        btnLeaveGame.SetActive(true);
        txtWaitingForOtherPlayers.SetActive(false);


        if (objStamp_02)
        {
            objStamp_02.gameObject.SetActive(true);
            objStamp_02.AP_LogoAnimation();
        }


        #endregion
    }



    public void OnClickButtonTrackSelection()
    {
        #region
        gPanelSore.SetActive(false);
        gPanelTrackSelection.SetActive(true);

        btnScore.SetActive(false);
        btnAccessNewTrack.SetActive(false);
        btnRestart.SetActive(true);
        btnLeaveGame.SetActive(true);
        txtWaitingForOtherPlayers.SetActive(false);

        if (PhotonManager) PhotonManager.TrackSelectionAllClientExceptMaster();

        if (eventSystem)
            eventSystem.SetSelectedGameObject(btnNextTrack);


        #endregion
    }

    public void TrackSelectionAllClientExceptMaster()
    {
        #region
        gPanelSore.SetActive(false);
        gPanelTrackSelection.SetActive(true);

        btnPreviousTrack.SetActive(false);
        btnRandomTrack.SetActive(false);
        btnNextTrack.SetActive(false);

        #endregion
    }



    public void MCR_InstantiateOnlineScore(string _car, string raceDuration, string _Points)
    {
        #region
       // Debug.Log("Here 10");
        if(!b_ScoreDisplayer && carListPUN_MCR.instance.playerNameThisComputer == _car)
        {
            b_ScoreDisplayer = true;
            if (objStamp_01)
            {
                objStamp_01.gameObject.SetActive(true);
                objStamp_01.AP_LogoAnimation();
            }
        }


        _counter++;

        GameObject newScore = Instantiate(OnlineResult, resultScrollView);
        OnlineResult_MCR onlineResult = newScore.GetComponent<OnlineResult_MCR>();

        onlineResult.txtPos.text = _counter.ToString();
        onlineResult.txtName.text = _car;
        onlineResult.txtTime.text = raceDuration;
        onlineResult.txtPoints.text = _Points;

        _OnlineResultList.Add(newScore);
        #endregion
    }

    public void MCR_TotalOnlineScore(string _carName, string _score)
    {
        #region
        if(_OnlineResultList.Count > 0)
        {
            if (txtTime) txtTime.text = "";

            foreach (GameObject GO in _OnlineResultList)
            {
                Destroy(GO);
            }
            _OnlineResultList.Clear();
            _counter = 0;
        }

        _counter++;

        GameObject newScore = Instantiate(OnlineResult, resultScrollView);
        OnlineResult_MCR onlineResult = newScore.GetComponent<OnlineResult_MCR>();

        onlineResult.txtPos.text = _counter.ToString();
        onlineResult.txtName.text = _carName;
        onlineResult.txtTime.text = "";
        onlineResult.txtPoints.text = _score;

        _TotalOnlineResultList.Add(newScore);
        #endregion
    }

    public void ShowNextTrack()
    {
        if (PhotonManager) PhotonManager.nextTrackImage();
    }

    public void ShowPreviousTrack()
    {
        if (PhotonManager) PhotonManager.previousTrackImage();
    }

    public void ShowRandomTrack()
    {
        if (PhotonManager) PhotonManager.randomTrackImage();
    }

    public void MCR_SetSelectedGameobject(GameObject obj)
    {
        if (eventSystem)
            eventSystem.SetSelectedGameObject(obj);
    }
}
#endif
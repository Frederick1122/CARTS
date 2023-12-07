#if PHOTON_UNITY_NETWORKING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using System;

namespace MCR
{
    public class GameManager_MCR_Photon : MonoBehaviourPunCallbacks
    {
        public static GameManager_MCR_Photon Instance = null;

        public Text InfoText;

        private Dictionary<int, string> playerListEntries;

        public bool b_IsMasterClient = false;
        public bool b_IsMine = false;

        [Header("Select Track Panel")]
        public int currentSelectedTrack = 0;
        public Image currentTrack_Image;
        public Text currentTrack_Name;

        #region UNITY

        public void Awake()
        {
            Instance = this;

            playerListEntries = new Dictionary<int, string>();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                playerListEntries.Add(p.ActorNumber, p.NickName);
            }

            b_IsMasterClient = PhotonNetwork.IsMasterClient;
            b_IsMine = GetComponent<PhotonView>().IsMine;

            if (PlayerPrefs.GetString("Which_GameMode") != "OnlineMultiPlayer")
                Destroy(this.gameObject);


            // Init gameobject needed for the Track Selection Panel
            GameObject tmpObj = GameObject.Find("ResultMultiObjRef");


            if (tmpObj) currentTrack_Image = tmpObj.GetComponent<ResultPageOnlineMulti_MCR>().ImTrackSelection;
            if (tmpObj) currentTrack_Name = tmpObj.GetComponent<ResultPageOnlineMulti_MCR>().txtTrackSelection;

        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        public void Start()
        {
            InfoText.text = "Waiting for other players...";

            Hashtable props = new Hashtable
            {
                {MCR.PhotonPlayerInfo.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

       
      


        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene("MCR_Lobby");
            PhotonNetwork.LocalPlayer.CustomProperties.Remove(PlayerNumbering.RoomPlayerIndexedProp);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MCR_Lobby");
            Hashtable props = new Hashtable() { { MCR.PhotonPlayerInfo.PLAYER_READY, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                // StartCoroutine(SpawnAsteroid());
            }
            //PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //CheckEndOfGame();
            Debug.Log("Player: " + otherPlayer.NickName + " left the room");

            for(var i = 0;i< tmpCarNickNameList.Count; i++)
            {
                if(tmpCarNickNameList[i] == otherPlayer.NickName)
                {
                    CarController tmpCarControler = tmpCarList[i];
                    tmpCarList.RemoveAt(i);
                    tmpCarRaceEndedList.RemoveAt(i);
                    tmpPhotonManagerList.RemoveAt(i);
                    tmpCarNickNameList.RemoveAt(i);
                    Destroy(tmpCarControler.gameObject);
                }
            }

        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(MCR.PhotonPlayerInfo.PLAYER_LIVES))
            {
                //CheckEndOfGame();
                return;
            }

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (changedProps.ContainsKey(MCR.PhotonPlayerInfo.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    Hashtable props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
            }
        }

        #endregion

        private void StartGame()
        {         
            MCR_Start();
        }

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(MCR.PhotonPlayerInfo.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool)playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

    

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }

        public void LeaveRoom()
        {
            //PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }


        

        public void MCR_Start()
        {
            #region
            //PlayerPrefs.SetString("Which_GameMode", "OnlineMultiPlayer");


            GameObject spawnPoint = GameObject.Find("Start_Position_0" + (PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1));
            Debug.Log("GetPlayerNumber: " + PhotonNetwork.LocalPlayer.GetPlayerNumber());


            int _playerNb = PhotonNetwork.LocalPlayer.GetPlayerNumber();
            //string carToInstantiate = carListPUN_MCR.instance.inventoryCar.inventoryItem[_playerNb].Cars[UnityEngine.Random.Range(0, carListPUN_MCR.instance.inventoryCar.inventoryItem[_playerNb].Cars.Count)].name;
            string carToInstantiate = carListPUN_MCR.instance.inventoryCar.inventoryItem[_playerNb].Cars[carListPUN_MCR.instance.selectedCar].name;
            GameObject _car = PhotonNetwork.Instantiate(carToInstantiate, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
            //_car.name = PhotonNetwork.LocalPlayer.NickName;


            // Connect the camera to the Player car
            GameObject _cam = GameObject.Find("P1_Cam");
            if (_cam)
            {
                _cam.GetComponent<Cam_Follow>().target = _car.GetComponent<CarController>().camTarget.transform;
                //_car.GetComponent<CarController>().b_CountdownActivate = false;
            }



            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("How many Players: " + PhotonNetwork.PlayerList.Length);
                int tmpCounter = 0;
                for (var i = PhotonNetwork.PlayerList.Length; i< carListPUN_MCR.instance.maxPlayerByRoom; i++)
                {
                    if(carListPUN_MCR.instance.maxCPU > tmpCounter)
                    {
                        spawnPoint = GameObject.Find("Start_Position_0" + (i + 1));
                        //Debug.Log("GetPlayerNumber: " + PhotonNetwork.LocalPlayer.GetPlayerNumber());

                        string carAIToInstantiate = carListPUN_MCR.instance.inventoryCar.inventoryItem[i].Cars[UnityEngine.Random.Range(0, carListPUN_MCR.instance.inventoryCar.inventoryItem[i].Cars.Count)].name;
                        _car = PhotonNetwork.Instantiate(carAIToInstantiate, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
                        _car.name = (i + 1).ToString();
                        _car.GetComponent<CarAI>().enabled = true;
                        _car.GetComponent<CarController>().playerNumber = i + 1;
                        tmpCarAIList.Add(_car.GetComponent<CarController>());
                        //_car.name = _car.GetComponent<PhotonView>().Owner.;
                        //_car.GetComponent<PhotonView>().
                        PhotonView photonView = PhotonView.Get(this);
                        int _ViewID = 0;



                        foreach (Transform child in _car.transform)
                        {
                            if (child.GetComponent<localPlayerInstance_MCR>())
                            {
                                _ViewID = child.GetComponent<PhotonView>().ViewID;
                            }
                        }

                        photonView.RPC("carUsingAI", RpcTarget.Others, _ViewID.ToString(), _car.name);

                    }
                    tmpCounter++;
                }
             
            }

            // Update txt that display the position in race using the number of cars
            //carListPUN_MCR.instance.HowManyCarsInCurrentRace = PhotonNetwork.PlayerList.Length + tmpCounter;


            /*foreach (Player p in PhotonNetwork.PlayerList)
            {
                Debug.Log(
                "ActorNumber: " + p.ActorNumber +
                " : NickName: " + p.NickName);
            }*/

            GameObject gameManager = GameObject.Find("Game_Manager");
            gameManager.GetComponent<Game_Manager>().countdown.b_ActivateCountdown = true;

            // If needed disable objects when Online Mode Starts.
            GameObject grp_Disable_Online = GameObject.Find("Grp_Disable_Online");
            if(grp_Disable_Online)
                grp_Disable_Online.SetActive(false);

            StartCoroutine(AllCarAreInstantiated());
            #endregion
        }

        IEnumerator AllCarAreInstantiated()
        {
            #region
            //yield return new WaitForSeconds(2);
            yield return new WaitUntil(() => checkIFALLCarsInstatiated() == true);



            //GameObject[] arrCars = GameObject.FindGameObjectsWithTag("Car");
            //Debug.Log("All car Instantiated:  " + " : " + arrCars.Length + " : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("tmpCarList: " + tmpCarList.Count);

           

                foreach (CarController car in tmpCarList)
            {
                GameObject _carPhotonManager = null;
                foreach (Transform child in car.transform)
                {
                    if (child.GetComponent<localPlayerInstance_MCR>())
                    {
                        _carPhotonManager = child.gameObject;
                        break;
                    }
                }

                if (_carPhotonManager && !car.gameObject.GetComponent<CarAI>().enabled)
                {
                    int PlayerNumber = _carPhotonManager.GetComponent<PhotonView>().OwnerActorNr;
                    car.gameObject.name = PlayerNumber.ToString();                              // Rename car in the Hierarchy
                    car.playerNumber = PlayerNumber;                                            // Chose the correct car number for each car.
                }
            }

            Game_Manager gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

            gameManager.list_Cars.Clear();

            for (var i = 0; i < gameManager.howManyCarsInRace; i++)// Create the array with needed size
                gameManager.list_Cars.Add(null);

            for (var i = 0; i < tmpCarList.Count; i++)
            {// Add cars manager by player to the car list    
                tmpCarList[i].playerNumber = int.Parse(tmpCarList[i].name);
                gameManager.list_Cars[int.Parse(tmpCarList[i].name) - 1] = tmpCarList[i];

            }

            for (var i = 0; i < tmpCarAIList.Count; i++)
            {// Add cars manager by AI to the car list      
                tmpCarAIList[i].playerNumber = int.Parse(tmpCarAIList[i].name);
                gameManager.list_Cars[int.Parse(tmpCarAIList[i].name) - 1] = tmpCarAIList[i];

            }

            // Init LapCounter;
            GameObject tmpObj = GameObject.Find("StartLine_lapCounter");

            if (tmpObj)
                tmpObj.GetComponent<LapCounter>().Online_InitLapCOunter();


            gameManager.Player2Position.SetActive(false);       // Disable some UIs
            gameManager.Player2PositionPart2.SetActive(false);
            gameManager.Player2LapCounter.SetActive(false);
            gameManager.lapcounter.Txt_Timer.gameObject.SetActive(false);
            gameManager.lapcounter.Txt_P2_Lap.gameObject.SetActive(false);
            gameManager.lapcounter.Txt_P2.gameObject.SetActive(false);


            // Update txt that display the position in race using the number of cars
            carListPUN_MCR.instance.HowManyCarsInCurrentRace = tmpCarList.Count + tmpCarAIList.Count;

            gameManager.b_initDone = true;

            for (var i = 0; i < tmpCarAIList.Count; i++)
            {    // Update Follow Target for each AI car
                tmpCarAIList[i].gameObject.GetComponent<CarController>().onlineNoFakeRotation = true;
                tmpCarAIList[i].gameObject.GetComponent<CarPathFollow>().MultiplayerUpdateCarFollowtarget();

            }

            for (var i = 0; i < tmpCarList.Count; i++)      // Update Follow Target for each car Players
                tmpCarList[i].gameObject.GetComponent<CarPathFollow>().MultiplayerUpdateCarFollowtarget();


            gameManager.canvas_MainMenu.GoToOtherPageWithHisNumber(5);

            yield return null;
            #endregion
        }

        public List<CarController> tmpCarList = new List<CarController>();
        public List<string> tmpCarNickNameList = new List<string>();

        public List<CarController> tmpCarAIList = new List<CarController>();
        public List<GameObject> tmpPhotonManagerList = new List<GameObject>();

        public List<bool> tmpCarRaceEndedList = new List<bool>();


        bool checkIFALLCarsInstatiated()
        {
            #region
            GameObject[] arrCars = GameObject.FindGameObjectsWithTag("Car");


            int tmpCounter = 0;
            tmpCarList.Clear();
            tmpCarNickNameList.Clear();
            tmpPhotonManagerList.Clear();
            tmpCarRaceEndedList.Clear();

            foreach (GameObject car in arrCars)
            {
                GameObject _carPhotonManager = null;
                foreach (Transform child in car.transform)
                {
                    if (child.GetComponent<localPlayerInstance_MCR>())
                    {
                        _carPhotonManager = child.gameObject;
                        break;
                    }
                }

                if (_carPhotonManager && _carPhotonManager.GetComponent<PhotonView>() && !car.gameObject.GetComponent<CarAI>().enabled)
                {
                    tmpCounter++;
                    tmpCarList.Add(car.GetComponent<CarController>());
                    tmpCarRaceEndedList.Add(false);

                    foreach (Player p in PhotonNetwork.PlayerList)
                    {
                       if(p.ActorNumber == _carPhotonManager.GetComponent<PhotonView>().OwnerActorNr)
                        {
                            tmpCarNickNameList.Add(p.NickName);
                            break;
                        }
                    }

                    tmpPhotonManagerList.Add(_carPhotonManager);
                }
            }

            //foreach (int usingAI in carUsingAIList)
            //{
                for (var j = 0; j < carUsingAIList.Count; j++)
                {
                    //foreach (CarController car in tmpCarList)
                    //{
                    for (var i = 0;i< tmpPhotonManagerList.Count; i++) {
                    // Debug.Log("ViewID: " + tmpPhotonManagerList[i].gameObject.GetComponent<PhotonView>().ViewID + " : usingAI: " + carUsingAIList[j]);
                    if (tmpPhotonManagerList[i].gameObject.GetComponent<PhotonView>().ViewID == carUsingAIList[j])
                    {
                        tmpPhotonManagerList[i].transform.parent.gameObject.GetComponent<CarAI>().enabled = true;
                        tmpPhotonManagerList[i].transform.parent.name = carUsingAINameList[j];
                        tmpCarAIList.Add(tmpPhotonManagerList[i].transform.parent.gameObject.GetComponent<CarController>());
                    }
                }

            }

            //Debug.Log("tmpCounter: " + tmpCounter + " : " + "PlayerList lenght: " + PhotonNetwork.PlayerList.Length);

            if (tmpCounter == PhotonNetwork.PlayerList.Length )
                return true;
            else
                return false;
            #endregion
        }


        // --> Arcade race is finished. Display result Page
       public IEnumerator WinProcessOnlineMultiplayer(GameObject _car,int raceDuration)
        {
            #region

          
            GameObject _carPhotonManager = null;
            foreach (Transform child in _car.transform)
            {
                if (child.GetComponent<localPlayerInstance_MCR>())
                {
                    _carPhotonManager = child.gameObject;
                    break;
                }
            }

            if ((_carPhotonManager.GetComponent<PhotonView>().IsMine || PhotonNetwork.IsMasterClient) && !_carPhotonManager.transform.parent.GetComponent<CarAI>().enabled)
            {
                                         
                GameObject gM_Photon = GameObject.Find("GM_Photon");
                if (gM_Photon) gM_Photon.GetComponent<MCR.GameManager_MCR_Photon>().MCRMultiDisplayResult();

                foreach (Player p in PhotonNetwork.PlayerList)
                {

                    if (_carPhotonManager.GetComponent<PhotonView>().OwnerActorNr == p.ActorNumber)
                    {
                        Game_Manager Game_Manager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

                        if (Game_Manager.canvas_MainMenu && _carPhotonManager.GetComponent<PhotonView>().IsMine) Game_Manager.canvas_MainMenu.GoToOtherPageWithHisNumber(14);

                        // Debug.Log("Score");

                        if (PhotonNetwork.IsMasterClient)
                        {
                            int _carPoints = 0;
                            if (HowManyCarFinishTheRace < carListPUN_MCR.instance.scoreTable.Count)
                            {
                                //Debug.Log("HowManyCarFinishTheRace: " + HowManyCarFinishTheRace);
                                //Debug.Log("carListPUN_MCR.instance.scoreTable[HowManyCarFinishTheRace]: " + carListPUN_MCR.instance.scoreTable[HowManyCarFinishTheRace]);

                                _carPoints = carListPUN_MCR.instance.scoreTable[HowManyCarFinishTheRace];
                            }


                            PhotonView photonView = PhotonView.Get(this);
                            photonView.RPC("DisplayScoreMultiOnline", RpcTarget.All, p.NickName, raceDuration.ToString(), _carPoints.ToString());

                           

                            carListPUN_MCR.instance.MCR_AddToPlayerScore(p.NickName, _carPoints);

                            for (var i = 0; i < tmpCarList.Count; i++)
                            {
                                if (p.NickName == tmpCarNickNameList[i])
                                {
                                    tmpCarRaceEndedList[i] = true;
                                }
                            }

                        }

                        break;
                    }
                }
            }
            else
            {
                if (!b_TotalScoreDisplayed)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonView photonView = PhotonView.Get(this);
                        photonView.RPC("DisplayScoreMultiOnline", RpcTarget.All, "CPU", raceDuration.ToString()," ");
                    }

                    photonView.RPC("StopAIOtherPlayers", RpcTarget.Others, _car.name);
                }

            }


            yield return null;
            #endregion
        }


        public Text ScoreTxt;
       /* private string sTextPos = "Pos:" + "\n";
        private string sTextName = "Name:"  + "\n";
        private string sTextTime = "Time:" + "\n";
        private string sTextPoints = "Points:" + "\n";*/
        public int HowManyCarFinishTheRace = 0;
        private ResultPageOnlineMulti_MCR resultMultiObjRef;

        ResultPageOnlineMulti_MCR Find_Page_Result_Multi()
        {
            ResultPageOnlineMulti_MCR resultMultiObjRef = GameObject.Find("ResultMultiObjRef").GetComponent<ResultPageOnlineMulti_MCR>();
            return resultMultiObjRef;
        }

        public void MCRMultiDisplayResult()
        {
            if (!resultMultiObjRef)
                resultMultiObjRef = Find_Page_Result_Multi();


            //Debug.Log("Here");
        }

        [PunRPC]
        public void DisplayScoreMultiOnline(string _car, string raceDuration,string _Points)
        {

            if (!resultMultiObjRef)
                resultMultiObjRef = Find_Page_Result_Multi();

            resultMultiObjRef.MCR_InstantiateOnlineScore(_car, F_Timer(raceDuration), _Points);

            HowManyCarFinishTheRace++;
        }

        public List<int> carUsingAIList = new List<int>();
        public List<string> carUsingAINameList = new List<string>();

        [PunRPC]
        public void carUsingAI(string number,string name)
        {
            //Debug.Log("number: " + number + ": name: " + name);


            carUsingAIList.Add(int.Parse(number));
            carUsingAINameList.Add(name);
        }


        [PunRPC]
        public void RPCNewTrack()
        {
            //int _random = UnityEngine.Random.Range(0, carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackNameList.Count);
            //string levelName = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackNameList[_random];

            string levelName = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackNameList[currentSelectedTrack];
            PhotonNetwork.LoadLevel(levelName);
        }



        [PunRPC]
        public void StopAIOtherPlayers(string _carName)
        {
            for(var i = 0; i< tmpCarAIList.Count; i++)
            {
                if(_carName == tmpCarAIList[i].name)
                {
                    tmpCarAIList[i].GetComponent<CarController>().raceIsFinished = true;
                    break;
                }
            }
        }




        public void NewRaceButtonClicked()
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCNewTrack", RpcTarget.All);
        }



        bool b_TotalScoreDisplayed = false;

        public void displayOnlineTotalScore()
        {
            #region
            b_TotalScoreDisplayed = true;
            List<PlayerScoreCompare> playersScores = new List<PlayerScoreCompare>();        // Create a list 

            //carListPUN_MCR.instance.scoreTable[HowManyCarFinishTheRace - 1];
            for (int i = 0; i < carListPUN_MCR.instance._Players.Count; i++)
            {                                    // Create the list with name and scores

                playersScores.Add(new PlayerScoreCompare(
                    carListPUN_MCR.instance._Players[i]._NickName,
                    carListPUN_MCR.instance._Players[i]._Score.ToString(),
                    "",
                    carListPUN_MCR.instance._Players[i]._Score));


            }
            playersScores.Sort();
            playersScores.Reverse();

            /*sTextPos = "";
            sTextName = "";
            sTextTime = "";
            sTextPoints = "";*/

            for (int j = 0; j < playersScores.Count; j++)
            {
               if (PhotonNetwork.IsMasterClient)
                {
                    PhotonView photonView = PhotonView.Get(this);
                    photonView.RPC("RPCDisplayOnlineTotalScore", RpcTarget.All, playersScores[j].name, playersScores[j].total.ToString());
                }

            }
            #endregion


        }

        [PunRPC]
        public void RPCDisplayOnlineTotalScore(string _carName, string _score)
        {
            #region
            b_TotalScoreDisplayed = true;



            if (!resultMultiObjRef)
                resultMultiObjRef = Find_Page_Result_Multi();

            resultMultiObjRef.MCR_TotalOnlineScore(_carName, _score);

            #endregion
        }

        // Format result to 00:00:00
        string F_Timer(string duration)
        {

            int time = int.Parse(duration);
           
           
            string result = "";
            //TimeSpan ts = TimeSpan.FromMilliseconds(yourMiliseconds);
            //result = ts.ToString();
            int intTime  = time;
            int minutes = (intTime/1000) / 60;
            int seconds  = (intTime / 1000) % 60;
            int fraction  = time % 1000;
           // fraction = fraction % 1000;

            result = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);

            //Debug.Log("Time: " + duration + " : " + time + " : " + result);
            return result;

        }


        public void initTrackImageInGame()
        {
            currentSelectedTrack = 0;
            if (PlayerPrefs.HasKey("LastOnlineTrack"))
            {
                currentSelectedTrack = PlayerPrefs.GetInt("LastOnlineTrack");
            }

            PlayerPrefs.SetInt("LastOnlineTrack", currentSelectedTrack);


            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCUpdateSelectedTrackInGame", RpcTarget.All, currentSelectedTrack);
        }

        public void nextTrackImage()
        {
            #region
            currentSelectedTrack++;
            currentSelectedTrack = currentSelectedTrack % carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList.Count;
            PlayerPrefs.SetInt("LastOnlineTrack", currentSelectedTrack);

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCUpdateSelectedTrackInGame", RpcTarget.All, currentSelectedTrack);
            #endregion
        }

        public void previousTrackImage()
        {
            #region
            currentSelectedTrack--;
            if (currentSelectedTrack < 0)
                currentSelectedTrack = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList.Count - 1;

            currentSelectedTrack = currentSelectedTrack % carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList.Count;
            PlayerPrefs.SetInt("LastOnlineTrack", currentSelectedTrack);

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCUpdateSelectedTrackInGame", RpcTarget.All, currentSelectedTrack);
            #endregion
        }


        public void randomTrackImage()
        {
            #region
            int _random = UnityEngine.Random.Range(0, carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList.Count);

            currentSelectedTrack = _random;
            PlayerPrefs.SetInt("LastOnlineTrack", currentSelectedTrack);


            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCUpdateSelectedTrackInGame", RpcTarget.All, currentSelectedTrack);
            #endregion
        }

        public void UpdateSelectedTrack(int _selectedTrack)
        {
            if (currentTrack_Image) currentTrack_Image.sprite = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList[_selectedTrack];
            if (currentTrack_Name) currentTrack_Name.text = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackDisplayedNameList[_selectedTrack];
        }

        [PunRPC]
        public void RPCUpdateSelectedTrackInGame(int _selectedTrack)
        {
            UpdateSelectedTrack(_selectedTrack);
        }

        public void totalScoreAllClient()
        {
            #region
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCTotalScoreAllClient", RpcTarget.Others);
            #endregion
        }

        [PunRPC]
        public void RPCTotalScoreAllClient()
        {
            if (!resultMultiObjRef)
                resultMultiObjRef = Find_Page_Result_Multi();

            resultMultiObjRef.TotalScorOnlineAllClient();
        }



        public void TrackSelectionAllClientExceptMaster()
        {
            #region
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCTrackSelectionAllClientExceptMaster", RpcTarget.Others);
            #endregion
        }

        [PunRPC]
        public void RPCTrackSelectionAllClientExceptMaster()
        {
            if (!resultMultiObjRef)
                resultMultiObjRef = Find_Page_Result_Multi();

            resultMultiObjRef.TrackSelectionAllClientExceptMaster();
        }


    }



}
#endif
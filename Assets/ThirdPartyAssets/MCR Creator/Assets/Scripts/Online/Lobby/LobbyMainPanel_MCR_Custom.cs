#if PHOTON_UNITY_NETWORKING
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;


namespace MCR
{
    public class LobbyMainPanel_MCR_Custom : MonoBehaviourPunCallbacks
    {
        private EventSystem eventSystem;
        [Header("Login Panel")]
        public GameObject LoginPanel;
        public GameObject btnLogin;
        public GameObject btnBack;

        public InputField PlayerNameInput;

        [Header("Selection Panel")]
        public GameObject SelectionPanel;
        public GameObject btnCreateRoom;

        [Header("Room List Panel")]
        public GameObject RoomListPanel;

        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;
        public GameObject btnBackRoomList;


        [Header("Inside Room Panel")]
        public GameObject InsideRoomPanel;
        public GameObject btnNextTrack;
        public GameObject btnPreviousTrack;
        public Text txtSelectedTrack;

        public Button StartGameButton;
        public GameObject PlayerListEntryPrefab;

        private string roomName = "";
        public Text txtRoomName;
        private int HowManyPlayerInARoom = 4;
        public string SceneToLoad = "";

        public Image currentTrack_Image;
        public Text currentTrack_Name;

       
        //public inventoryOnlineTracks _inventoryOnlineTracks;

        public int currentSelectedTrack = 0;

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;

        public List<int> playerActorNumbList  = new List<int>();
        public List<string> playerActorNickNameList = new List<string>();
        public List<int> playerSelectedCarList = new List<int>();
        public List<GameObject> playerInstantiatedCarList = new List<GameObject>();
        public List<MCR.PlayerListEntry_MCR_Custom> entriesList = new List<MCR.PlayerListEntry_MCR_Custom>();

        private ExitGames.Client.Photon.Hashtable hashSelectedCar = new ExitGames.Client.Photon.Hashtable();


        #region UNITY

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();

            PlayerNameInput.text = "Player " + Random.Range(1000, 10000);

            PlayerPrefs.SetString("Which_GameMode", "OnlineMultiPlayer");

            GameObject tmpObj = GameObject.Find("EventSystem");

            if (tmpObj)
                eventSystem = tmpObj.GetComponent<EventSystem>();
        }

        public void Start()
        {
            HowManyPlayerInARoom = carListPUN_MCR.instance.maxPlayerByRoom;
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnConnectedToMaster()
        {
            this.SetActivePanel(SelectionPanel.name);
            Debug.Log("On Connect To Master");
            SetSelectedGameObject_MCR(2);       // Select the button Create Room
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            #region
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
            #endregion
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();

            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");
            SetActivePanel(InsideRoomPanel.name);

            txtRoomName.text = roomName;

        
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
                playerActorNumbList.Clear();
                playerActorNickNameList.Clear();
                playerSelectedCarList.Clear();
                entriesList.Clear();
                playerInstantiatedCarList.Clear();
            }
            //Debug.Log(PhotonNetwork.PlayerList.Length);
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerListEntryPrefab);
                entry.transform.SetParent(InsideRoomPanel.transform.GetChild(0).transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<MCR.PlayerListEntry_MCR_Custom>().Initialize(p.ActorNumber, p.NickName);

                //Debug.Log("p.ActorNumber: " + p.ActorNumber + " : p.NickName: " + p.NickName);

                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(MCR.PhotonPlayerInfo.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<MCR.PlayerListEntry_MCR_Custom>().SetPlayerReady((bool)isPlayerReady);
                }

                playerListEntries.Add(p.ActorNumber, entry);

                playerActorNumbList.Add(p.ActorNumber);
                playerActorNickNameList.Add(p.NickName);


                /*
                int KillScore = (int)p.CustomProperties["Kills"];
                KillScore++;
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
                hash.Add("Kills", KillScore);
                p.SetCustomProperties(hash);
                */
                //int KillScore = (int)p.CustomProperties["Kills"];


                int _sCar = 0;
                if (p.CustomProperties["SelectedCar"] != null)
                {
                    _sCar = (int)p.CustomProperties["SelectedCar"];
                    //Debug.Log("_sCar: " + p.NickName + " : Yes: " + _sCar);
                }


                if (_sCar == 0)
                {
                    playerSelectedCarList.Add(_sCar);
                    hashSelectedCar["SelectedCar"] = _sCar;
                    p.SetCustomProperties(hashSelectedCar);
                   
                }

                else
                {
                    playerSelectedCarList.Add(_sCar);
                    hashSelectedCar["SelectedCar"] = _sCar;
                    p.SetCustomProperties(hashSelectedCar);
                   
                }

                //Debug.Log("_sCar: " + p.NickName + " : " + _sCar);



                playerInstantiatedCarList.Add(null);





                callRPCLoadNewCar(p.ActorNumber, playerSelectedCarList[playerSelectedCarList.Count - 1]);





                entriesList.Add(entry.GetComponent<MCR.PlayerListEntry_MCR_Custom>());
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
            if (!PhotonNetwork.IsMasterClient)
            {
                btnNextTrack.gameObject.SetActive(false);
                btnPreviousTrack.gameObject.SetActive(false);
                txtSelectedTrack.text = "Selected Track";
            }
          

            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                {MCR.PhotonPlayerInfo.PLAYER_LOADED_LEVEL, false}
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("RPCUpdateSelectedTrack", RpcTarget.All, currentSelectedTrack);
            }

        }

        public override void OnLeftRoom()
        {
            #region
            SetActivePanel(SelectionPanel.name);

            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerActorNumbList.Clear();
            playerActorNickNameList.Clear();
            //playerSelectedCarList.Clear();
            playerInstantiatedCarList.Clear();
            entriesList.Clear();
            playerListEntries.Clear();
            playerListEntries = null;
            #endregion
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
        
            Debug.Log("Enter the room");
          
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(InsideRoomPanel.transform.GetChild(0).transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<MCR.PlayerListEntry_MCR_Custom>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

            playerListEntries.Add(newPlayer.ActorNumber, entry);

            playerActorNumbList.Add(newPlayer.ActorNumber);
            playerActorNickNameList.Add(newPlayer.NickName);


            int _sCar = 0;
            if (newPlayer.CustomProperties["SelectedCar"] != null)
                _sCar = (int)newPlayer.CustomProperties["SelectedCar"];

            if (_sCar == 0)
            {
                playerSelectedCarList.Add(_sCar);
                hashSelectedCar["SelectedCar"] = _sCar;
                newPlayer.SetCustomProperties(hashSelectedCar);
               //Debug.Log("_sCar: " + _sCar);
            }

            else
            {
                playerSelectedCarList.Add(_sCar);
                hashSelectedCar["SelectedCar"] = _sCar;
                newPlayer.SetCustomProperties(hashSelectedCar);
                //Debug.Log("_sCar: " + _sCar);
            }

            Debug.Log("_sCar: " + newPlayer.NickName + " : " + _sCar);

            // playerSelectedCarList.Add(0);
            playerInstantiatedCarList.Add(null);
            entriesList.Add(entry.GetComponent<MCR.PlayerListEntry_MCR_Custom>());


            callRPCLoadNewCar(newPlayer.ActorNumber, 0);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());


            if (PhotonNetwork.IsMasterClient)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("RPCUpdateSelectedTrack", RpcTarget.All, currentSelectedTrack);
            }

        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            #region
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);


            for (var i = 0;i< playerActorNumbList.Count; i++)
            {
                if(playerActorNumbList[i] == otherPlayer.ActorNumber)
                {
                    foreach(GameObject obj in playerInstantiatedCarList)
                    {
                        obj.SetActive(false);
                        obj.transform.position += new Vector3(0, obj.transform.position.y + 1000, 0);
                        Destroy(obj);
                    }
                   

                    playerActorNumbList.RemoveAt(i);
                    playerActorNickNameList.RemoveAt(i);
                    playerSelectedCarList.RemoveAt(i);
                    playerInstantiatedCarList.RemoveAt(i);
                    entriesList.RemoveAt(i);
                    break;
                }

            }

            // Reset Cars
            for (var i = 0; i < playerActorNumbList.Count; i++)
            {
                callRPCLoadNewCar(playerActorNumbList[i], playerSelectedCarList[i]);


                // Init cam 
                GameObject carCam = GameObject.Find("Cam_Car_" + (i + 1));

                if (carCam)
                { entriesList[i].rImage.texture = carCam.GetComponent<Camera>().targetTexture; }
            }

          



            StartGameButton.gameObject.SetActive(CheckPlayersReady());
            #endregion
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                StartGameButton.gameObject.SetActive(CheckPlayersReady());
                btnNextTrack.gameObject.SetActive(true);
                btnPreviousTrack.gameObject.SetActive(true);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                object isPlayerReady;
                if (changedProps.TryGetValue(MCR.PhotonPlayerInfo.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<MCR.PlayerListEntry_MCR_Custom>().SetPlayerReady((bool)isPlayerReady);
                }
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        #endregion

        #region UI CALLBACKS

        public void OnBackButtonClicked()
        {
            #region
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            SetActivePanel(SelectionPanel.name);
            #endregion
        }

        public void OnCreateRoomButtonClicked()
        {
            #region
            roomName = "Room " + Random.Range(1000, 10000);
            RoomOptions options = new RoomOptions { MaxPlayers = (byte)HowManyPlayerInARoom };
            PhotonNetwork.CreateRoom(roomName, options, null);
            #endregion
        }


        public void OnLeaveGameButtonClicked()
        {
            #region
            PhotonNetwork.LeaveRoom();
            #endregion
        }

        public void OnLoginButtonClicked()
        {
            string playerName = PlayerNameInput.text;

            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
                carListPUN_MCR.instance._Players.Clear();
                carListPUN_MCR.instance.playerNameThisComputer = playerName;
            }
            else
            {
                Debug.LogError("Player Name is invalid.");
            }
        }

        public void OnRoomListButtonClicked()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }

            SetActivePanel(RoomListPanel.name);
            SetSelectedGameObject_MCR(5);       // Select the button Back Room List
        }

        public void OnLeaveCreateRoomClicked()
        {
            PhotonNetwork.Disconnect();
            SetActivePanel(LoginPanel.name);
        }

        public void OnStartGameButtonClicked()
        {
            StartCoroutine(IE_OnStartGameButtonClicked());
        }


       public IEnumerator IE_OnStartGameButtonClicked()
       {
            #region
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("DisplayLoadingScreen", RpcTarget.All);

            yield return new WaitForSeconds(1); 
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel(carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackNameList[currentSelectedTrack]);
            yield return null;
            #endregion
       }


        [PunRPC]
        public void DisplayLoadingScreen()
        {
            #region
             GameObject canvas_MainMenu = GameObject.Find("Canvas_MainMenu");
            if (canvas_MainMenu) canvas_MainMenu.GetComponent<Menu_Manager>().GoToOtherPageWithHisNumber(1);

            #endregion
        }




        #endregion

        private bool CheckPlayersReady()
        {
            #region
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(MCR.PhotonPlayerInfo.PLAYER_READY, out isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
            #endregion
        }

        private void ClearRoomListView()
        {
            #region
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
            #endregion
        }

        public void LocalPlayerPropertiesUpdated()
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        private void SetActivePanel(string activePanel)
        {
            #region
            LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            //CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
            //JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
            InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
            #endregion
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            #region
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
            #endregion
        }

        private void UpdateRoomListView()
        {
            #region
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<MCR.RoomListEntry_MCR_Custom>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

                roomListEntries.Add(info.Name, entry);
            }
            #endregion
        }

        public void OnClickBackToLoggin()
        {
            this.SetActivePanel(LoginPanel.name);
            PhotonNetwork.Disconnect();           
        }

        public void nextTrackImage()
        {
            #region
            currentSelectedTrack++;
            currentSelectedTrack = currentSelectedTrack % carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList.Count;
            //UpdateSelectedTrack(currentSelectedTrack);

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCUpdateSelectedTrack", RpcTarget.All, currentSelectedTrack);
            #endregion
        }

        public void previousTrackImage()
        {
            #region
            currentSelectedTrack--;
            if (currentSelectedTrack < 0)
                currentSelectedTrack = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList.Count - 1;

            currentSelectedTrack = currentSelectedTrack % carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList.Count;
            //UpdateSelectedTrack(currentSelectedTrack);

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCUpdateSelectedTrack", RpcTarget.All, currentSelectedTrack);
            #endregion
        }

        public void UpdateSelectedTrack(int _selectedTrack)
        {
            if (currentTrack_Image) currentTrack_Image.sprite = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackImageList[_selectedTrack];
            if (currentTrack_Name) currentTrack_Name.text = carListPUN_MCR.instance.inventoryOnlineTracks.MultiPlayerTrackDisplayedNameList[_selectedTrack];
        }

        [PunRPC]
        public void RPCUpdateSelectedTrack(int _selectedTrack)
        {
            UpdateSelectedTrack(_selectedTrack);
        }

        public void callRPCLoadNewCar(int playerNumber, int selectedCar)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCLoadNewCar", RpcTarget.All, playerNumber, selectedCar);

        }

        [PunRPC]
        public void RPCLoadNewCar(int _ActorNumber, int selectedCar)
        {
            #region
            //Debug.Log("Here Player: " + _ActorNumber + " : selectedCar:" + selectedCar);

            for (var i = 0; i < playerActorNumbList.Count; i++)
            {
                if (playerActorNumbList[i] == _ActorNumber)
                {
                    playerSelectedCarList[i] = selectedCar;
                    _instantiateNewCar(i, selectedCar);
                    break;
                }

            }
            #endregion
        }

        public void _instantiateNewCar(int playerNumber, int selectedCar)
        {
            #region
            //Debug.Log("playerNumberplayerNumber: " + playerNumber);

            GameObject SpawnPoint = GameObject.Find("Pivot_Car" + (playerNumber + 1));
            GameObject refObj = null;
            if (SpawnPoint)
            {
                if (SpawnPoint.transform.childCount > 0)
                {
                    SpawnPoint.transform.GetChild(0).gameObject.SetActive(false);
                    SpawnPoint.transform.GetChild(0).gameObject.transform.position += new Vector3(0, SpawnPoint.transform.GetChild(0).gameObject.transform.position.y + 1000, 0);
                    refObj = SpawnPoint.transform.GetChild(0).gameObject;
                    SpawnPoint.transform.GetChild(0).SetParent(null);
                    Destroy(refObj);
                }

                InventoryCar inventoryCar = carListPUN_MCR.instance.inventoryCar;

                GameObject newCar = Instantiate(inventoryCar.inventoryItem[playerNumber].Cars[selectedCar], SpawnPoint.transform);


                Transform[] allChildren = newCar.GetComponentsInChildren<Transform>(true);
                foreach (Transform child in allChildren)
                {
                    if (child.GetComponent<Collider>())
                    {
                        Collider[] allCol = newCar.GetComponentsInChildren<Collider>();
                        foreach (Collider col in allCol)
                            col.enabled = false;
                    }
                   
                }

               

                playerInstantiatedCarList[playerNumber] = newCar;

                newCar.name = newCar.name.Replace("(Clone)", "");
                newCar.GetComponent<Rigidbody>().isKinematic = true;
                CarController carController = newCar.GetComponent<CarController>();
                carController.enabled = false;
                carController.audio_.mute = true;
                carController.objSkid_Sound.mute = true;
                carController.obj_CarImpact_Sound.mute = true;
                newCar.GetComponent<CarAI>().enabled = false;


                Transform pivotCarSelection = newCar.GetComponent<CarController>().pivotCarSelection.transform;                                                     // Move the car to a specific position

                pivotCarSelection.parent = SpawnPoint.transform;
                newCar.transform.parent = pivotCarSelection;
                pivotCarSelection.transform.localPosition = new Vector3(0, 0, 0);
                pivotCarSelection.transform.eulerAngles = SpawnPoint.transform.eulerAngles;


                GameObject mainPanel = GameObject.Find("MainPanel");


            }
            #endregion
        }

        public void SetSelectedGameObject_MCR(int value)
        {
            #region
            if (eventSystem) { 
            switch (value)
            {
                case 5:
                        eventSystem.SetSelectedGameObject(btnBackRoomList); 
                        break;
                case 4:
                        eventSystem.SetSelectedGameObject(btnNextTrack);
                        break;
                case 3:
                        eventSystem.SetSelectedGameObject(btnBack);
                        break;
                case 2:
                        eventSystem.SetSelectedGameObject(btnCreateRoom);
                        break;
                case 1:
                        eventSystem.SetSelectedGameObject(btnLogin);
                        break;
                default:
                    
                    break;
            }

            }
            #endregion
        }
    }
}
#endif
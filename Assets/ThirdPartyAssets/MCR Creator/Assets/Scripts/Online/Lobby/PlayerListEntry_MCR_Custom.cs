#if PHOTON_UNITY_NETWORKING
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

namespace MCR
{
    public class PlayerListEntry_MCR_Custom : MonoBehaviourPunCallbacks
    {
        [Header("UI References")]
        public Text PlayerNameText;

        public Image PlayerColorImage;
        public Button PlayerReadyButton;
        public Image PlayerReadyImage ;

        private int ownerId;
        private bool isPlayerReady;

        public GameObject btn_NextCar;
        public GameObject btn_PreviousCar;

        public RawImage rImage;

        private EventSystem eventSystem;
        private ExitGames.Client.Photon.Hashtable hashSelectedCar = new ExitGames.Client.Photon.Hashtable();


        #region UNITY

        public override void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        public void Start()
        {
            GameObject tmpObj = GameObject.Find("EventSystem");
            if (tmpObj)
                eventSystem = tmpObj.GetComponent<EventSystem>();

            if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
            {
                PlayerReadyButton.gameObject.SetActive(false);
                btn_NextCar.GetComponent<Image>().enabled = false;
                btn_NextCar.GetComponent<Button>().enabled = false;
                btn_PreviousCar.GetComponent<Image>().enabled = false;
                btn_PreviousCar.GetComponent<Button>().enabled = false;

            }
            else
            {

                SetPlayerReady(false);                      // Init the ready tick to Ready?

                PhotonNetwork.LocalPlayer.SetScore(0);



                if (tmpObj)
                    eventSystem.SetSelectedGameObject(PlayerReadyButton.gameObject);

                PlayerReadyButton.onClick.AddListener(() =>
                {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() { { MCR.PhotonPlayerInfo.PLAYER_READY, isPlayerReady } };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        FindObjectOfType<MCR.LobbyMainPanel_MCR_Custom>().LocalPlayerPropertiesUpdated();
                    }
                });
            }

        }

        public override void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        public void Initialize(int playerId, string playerName)
        {
            #region
            ownerId = playerId;
            PlayerNameText.text = playerName;

            // Init cam that display the player car
            initRendererTexture();
            #endregion
        }

        private void initRendererTexture()
        {
            #region
            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].ActorNumber == ownerId)
                {
                    GameObject carCam = GameObject.Find("Cam_Car_" + (i + 1));
                    if (carCam)
                    { rImage.texture = carCam.GetComponent<Camera>().targetTexture; }                   
                    break;
                }
            }

            #endregion
        }

        private void OnPlayerNumberingChanged()
        {
            #region
            /*foreach (Player p in PhotonNetwork.PlayerList)
            {

            }*/

            #endregion
        }

        public void SetPlayerReady(bool playerReady)
        {
           // Debug.Log("Set");
            PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
            PlayerReadyImage.enabled = playerReady;
        }


        public void NextCar()
        {
            #region
            carListPUN_MCR.instance.changeSelectedCar(ownerId);

            for(var i = 0;i< PhotonNetwork.PlayerList.Length; i++ )
            {
                if (PhotonNetwork.PlayerList[i].ActorNumber == ownerId)
                {
                    hashSelectedCar["SelectedCar"] = carListPUN_MCR.instance.selectedCar;
                    PhotonNetwork.PlayerList[i].SetCustomProperties(hashSelectedCar);

                    LoadNewCar(i,PhotonNetwork.PlayerList[i].ActorNumber);
                    break;
                }
            }
            #endregion
        }

        public void PreviousCar()
        {
            #region
            carListPUN_MCR.instance.changeSelectedCarPrevious(ownerId);

            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].ActorNumber == ownerId)
                {
                    hashSelectedCar["SelectedCar"] = carListPUN_MCR.instance.selectedCar;
                    PhotonNetwork.PlayerList[i].SetCustomProperties(hashSelectedCar);

                    LoadNewCar(i, PhotonNetwork.PlayerList[i].ActorNumber);
                    break;
                }
            }
            #endregion
        }

        // --> Load and save on playerPrefs a new car for a specific player
        public void LoadNewCar(int playerNumber,int _ActorNumber)
        {
            #region
            GameObject SpawnPoint = GameObject.Find("Pivot_Car" + (playerNumber+1));

            if (SpawnPoint)
            {
                GameObject mainPanel = GameObject.Find("MainPanel");
                mainPanel.GetComponent<LobbyMainPanel_MCR_Custom>().callRPCLoadNewCar(_ActorNumber, carListPUN_MCR.instance.selectedCar);
            }
            else
            {
                Debug.Log("Spawn point doesn't exist: playerNumber: " + playerNumber);
            }
            #endregion
        }

        public void SetSelectedGameObject_btn_NextCar()
        {
            #region
            if (eventSystem)
            {
                eventSystem.SetSelectedGameObject(btn_NextCar);
            }
            #endregion
        }

        public void SetSelectedGameObject_btn_PreviousCar()
        {
            #region
            if (eventSystem)
            {
                eventSystem.SetSelectedGameObject(btn_PreviousCar);
            }
            #endregion
        }

        public void SetSelectedGameObject_btn_PlayerReady()
        {
            #region
            if (eventSystem)
            {
                eventSystem.SetSelectedGameObject(PlayerReadyButton.gameObject);
            }
            #endregion
        }

    }
}
#endif
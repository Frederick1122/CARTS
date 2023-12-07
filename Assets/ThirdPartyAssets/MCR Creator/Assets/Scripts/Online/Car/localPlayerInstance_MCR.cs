#if PHOTON_UNITY_NETWORKING
using System.Collections;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;


namespace MCR
{
    public class localPlayerInstance_MCR : MonoBehaviourPunCallbacks
    {

        private PhotonView _photonView;
        private CarController _carController;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #region UNITY




        public void Awake()
        {
            this._photonView = GetComponent<PhotonView>();
            this._carController = this.gameObject.transform.parent.GetComponent<CarController>();


            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (_photonView.IsMine)
            {
                localPlayerInstance_MCR.LocalPlayerInstance = this.gameObject;
            }


            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

       
        public void Update()
        {
            if (PlayerPrefs.GetString("Which_GameMode") == "OnlineMultiPlayer"){
                if (!_photonView.IsMine)
                {
                    _carController.b_IsMine = false;
                    return;
                }


                _carController.b_IsMine = true;
            }
           

        }




        public void FixedUpdate()
        {
            if (PlayerPrefs.GetString("Which_GameMode") == "OnlineMultiPlayer")
            {
                if (!_photonView.IsMine)
                {
                    _carController.b_IsMine = false;
                    return;
                }

                _carController.b_IsMine = true;
            }
        }

        #endregion


        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("OnMasterClientSwitched: " + newMasterClient);
            PhotonNetwork.LeaveRoom();
           
        }

    }
}
#endif
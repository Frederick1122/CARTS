#if PHOTON_UNITY_NETWORKING
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

namespace MCR
{
    public class RoomListEntry_MCR_Custom : MonoBehaviour
    {
        public Text RoomNameText;
        public Text RoomPlayersText;
        public Button JoinRoomButton;

        private string roomName;

        private EventSystem eventSystem;

        public void Start()
        {
            GameObject tmpObj = GameObject.Find("EventSystem");
            if (tmpObj)
                eventSystem = tmpObj.GetComponent<EventSystem>();

            JoinRoomButton.onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }

                PhotonNetwork.JoinRoom(roomName);
            });
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers)
        {
            roomName = name;

            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        }

        public void SetSelectedGameObject_btn_JoinRoomButton()
        {
            if (eventSystem)
            {
                eventSystem.SetSelectedGameObject(JoinRoomButton.gameObject);
            }
        }
    }
}
#endif
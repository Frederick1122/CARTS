#if PHOTON_UNITY_NETWORKING
using UnityEngine;
using UnityEngine.UI;


using Photon.Pun;


namespace MCR
{
    public class LobbyTopPanel_MCR_Custom : MonoBehaviour
    {
        private readonly string connectionStatusMessage = "    Connection Status: ";

        [Header("UI References")]
        public Text ConnectionStatusText;

#region UNITY

        public void Update()
        {
            ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
        }

#endregion
    }
}

#endif
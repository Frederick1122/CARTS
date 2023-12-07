#if PHOTON_UNITY_NETWORKING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace MCR
{
    public class OnlinePause_MCR : MonoBehaviourPun
    {

        public void QuitGame()
        {
            PhotonNetwork.Disconnect();
            //PhotonNetwork.LeaveRoom();
        }


        public void OnCLickOnlineResumeGame()
        {
            GameObject tmpMenu = GameObject.Find("Canvas_MainMenu");
            if(tmpMenu) tmpMenu.GetComponent<Menu_Manager>().GoToOtherPageWithHisNumber(5);
        }
    }
}
#endif

#if PHOTON_UNITY_NETWORKING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun.UtilityScripts;
using Photon.Pun;

namespace MCR
{
    public class MCRPhotonCountdown : MonoBehaviourPunCallbacks
    {
        private float startTime = 0;
        public bool b_InitStartTime = false;

        [Header("Countdown time in seconds")]
        public float Countdown = 4.0f;



        public string returnPhotonCountdown()
        {
            if (b_InitStartTime)
            {
                if (startTime == 0)
                    startTime = (float)PhotonNetwork.Time;

                float timer = (float)PhotonNetwork.Time - startTime;
                float countdown = Countdown - timer;

                //Debug.Log(string.Format("Game starts in {0} seconds", countdown.ToString("n0")));

                return string.Format(countdown.ToString("n0"));
            }
            return "100";
        }

        public void F_InitStartTime()
        {
#region
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonView _photonView = PhotonView.Get(this);
                _photonView.RPC("InitStartTime", RpcTarget.All, (float)PhotonNetwork.Time);
            }
           
#endregion
        }

        [PunRPC]
        public void InitStartTime(float fStartTime)
        {
            startTime = fStartTime;
            b_InitStartTime = true;
        }

    }
}
#endif
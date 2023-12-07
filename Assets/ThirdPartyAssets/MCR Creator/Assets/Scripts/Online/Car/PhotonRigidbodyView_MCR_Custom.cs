#if PHOTON_UNITY_NETWORKING
namespace MCR
{
    using UnityEngine;
    using UnityEngine.UI;

    using Photon.Pun;
    using System.Collections;
    using System.Collections.Generic;

    [RequireComponent(typeof(PhotonView))]
    //[RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("Photon Networking/Photon Rigidbody View")]
    public class PhotonRigidbodyView_MCR_Custom : MonoBehaviour, IPunObservable
    {
        private float m_Distance;
        private float m_Angle;

        private Rigidbody m_Body;

        private PhotonView m_PhotonView;

        private Vector3 m_NetworkPosition;

        private Quaternion m_NetworkRotation;

        public bool m_TeleportEnabled = true;
        public float m_TeleportIfDistanceGreaterThan = 3.0f;

        private float speedTransition = .25f;
        private float speedTransitionRotation = 3f;

        private Text UITransitionSpeed;
        private Text UITransitionrotationSpeed;

        private Text UISendRate;
        private Text UIOffset;


        private Text UIWantedPos;

        private Text UIMiniDistance;

        private int currentSendRate = 12;

        public GameObject Ghost;

       // private Vector3 nextPos;
        //private float nextDistance;
       // private float distanceMinimum = .5f;
       // private Vector3 lastPosition;
       // public float lastDistance;

        //private int _Counter = 0;

        //private float fOffset = 1.5f;
        //private float fWantedPos = 1.7f;
        public GameObject CubeImpact;
        public impact MImpact;
        //private bool b_AccelerationImpact = false;

        public GameObject CubeWantedPoos;
        private Vector3 m_NetworkWantedPos;

        private Quaternion m_NetworkWantedRot;


        public GameObject CubeWantedPos2;



        public void Awake()
        {
            if (PlayerPrefs.GetString("Which_GameMode") != "OnlineMultiPlayer")
            {
                Destroy(this.gameObject);
                return;
            }

            this.m_Body = this.gameObject.transform.parent.GetComponent<Rigidbody>();
            this.m_PhotonView = GetComponent<PhotonView>();

            this.m_NetworkPosition = new Vector3();
            this.m_NetworkRotation = new Quaternion();
        }


        public void Start()
        {
            PhotonNetwork.SendRate = currentSendRate;
            PhotonNetwork.SerializationRate = currentSendRate;
            //Debug.Log(PhotonNetwork.SendRate);
           

            if (Ghost)
                Ghost.transform.SetParent(null);
            if (CubeWantedPoos)
                CubeWantedPoos.transform.SetParent(null);

      


        }

        public void Update()
        {
            #region
            /*
            if (Input.GetKeyDown(KeyCode.D))
            {
                distanceMinimum += .01f;
                distanceMinimum %= .4f;

            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                fWantedPos += .1f;
                fWantedPos %= 4;

            }


            if (Input.GetKeyDown(KeyCode.T))
            {
                speedTransition += .05f;
                speedTransition %= 2;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {

                currentSendRate += 1;

                currentSendRate %= 40;
                if (currentSendRate < 2)
                    currentSendRate = 2;

                PhotonNetwork.SendRate = currentSendRate;
                PhotonNetwork.SerializationRate = currentSendRate;
            }




                if (Input.GetKeyDown(KeyCode.O))
            {
                fOffset += .1f;
                fOffset %= 4;
                if (fOffset < .5f) fOffset = .5f;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                speedTransitionRotation += .05f;
                speedTransitionRotation %= 7;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                if (Ghost) Ghost.gameObject.SetActive(!Ghost.gameObject.activeSelf);
                if (CubeWantedPoos) CubeWantedPoos.gameObject.SetActive(!CubeWantedPoos.gameObject.activeSelf);
            }

            if (UITransitionSpeed == null)
            {
                GameObject obj = GameObject.Find("UITransitionSpeed");
                if (obj) UITransitionSpeed = obj.GetComponent<Text>();
            }
            else
            {
                UITransitionSpeed.text = "T: " + speedTransition.ToString();
            }

            if (UISendRate == null)
            {
                GameObject obj = GameObject.Find("UISendRate");
                if (obj) UISendRate = obj.GetComponent<Text>();
            }
            else
            {
                UISendRate.text = "Rate: " + currentSendRate.ToString();
            }

            if (UITransitionrotationSpeed == null)
            {
                GameObject obj = GameObject.Find("UITransitionrotationSpeed");
                if (obj) UITransitionrotationSpeed = obj.GetComponent<Text>();
            }
            else
            {
                UITransitionrotationSpeed.text = "Y Rot: " + speedTransitionRotation.ToString();
            }

            if (UIOffset == null)
            {
                GameObject obj = GameObject.Find("UIOffset");
                if (obj) UIOffset = obj.GetComponent<Text>();
            }
            else
            {
                UIOffset.text = "O Offset: " + fOffset.ToString();
            }



            if (UIWantedPos == null)
            {
                GameObject obj = GameObject.Find("UIWantedPos");
                if (obj) UIWantedPos = obj.GetComponent<Text>();
            }
            else
            {
                UIWantedPos.text = "I WPos: " + fWantedPos.ToString();
            }


            if (UIMiniDistance == null)
            {
                GameObject obj = GameObject.Find("UIMiniDistance");
                if (obj) UIMiniDistance = obj.GetComponent<Text>();
            }
            else
            {
                UIMiniDistance.text = "D MinDist: " + distanceMinimum.ToString();
            }
            */
            #endregion
        }



        public void FixedUpdate()
        {
            if (!this.m_PhotonView.IsMine)
            {
                //scaledValue = (rawValue - min) / (max - min);
                this.m_Body.position = Vector3.MoveTowards(this.m_Body.position, new Vector3(this.m_NetworkWantedPos.x, this.m_NetworkWantedPos.y, this.m_NetworkWantedPos.z), this.m_Distance * (speedTransition / PhotonNetwork.SerializationRate));
                this.m_Body.rotation = Quaternion.RotateTowards(this.m_Body.rotation, this.m_NetworkWantedRot, this.m_Angle * (speedTransitionRotation / PhotonNetwork.SerializationRate));
            }
        }



        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                #region
                stream.SendNext(this.m_Body.position);
                stream.SendNext(this.m_Body.rotation);

               //m_SynchronizeVelocity)
                stream.SendNext(this.m_Body.velocity);

                //m_SynchronizeAngularVelocity
                stream.SendNext(this.m_Body.angularVelocity);

                //if(MImpact)stream.SendNext(MImpact.carControl.b_AccelerationImpact);

               
                #endregion
            }
            else
            {

                    this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
                    this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();

                    this.m_NetworkWantedPos = this.m_NetworkPosition;
                    this.m_NetworkWantedRot = this.m_NetworkRotation;

                    if (this.m_TeleportEnabled)
                    {
                        if (Vector3.Distance(this.m_Body.position, this.m_NetworkPosition) > this.m_TeleportIfDistanceGreaterThan)
                        {
                            this.m_Body.position = this.m_NetworkPosition;
                        }
                    }


                    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                    //m_SynchronizeVelocity
                    this.m_Body.velocity = (Vector3)stream.ReceiveNext();
                    this.m_NetworkPosition += this.m_Body.velocity * lag /* fOffset*/;
                    //this.m_Distance = Vector3.Distance(this.m_Body.position, this.m_NetworkPosition);


                    //m_SynchronizeAngularVelocity)
                    this.m_Body.angularVelocity = (Vector3)stream.ReceiveNext();
                    this.m_NetworkRotation = Quaternion.Euler(this.m_Body.angularVelocity * lag/* fOffset*/) * this.m_NetworkRotation;
                    //this.m_Angle = Quaternion.Angle(this.m_Body.rotation, this.m_NetworkRotation);


                    //Debug.Log(this.m_NetworkPosition);
                    if (Ghost) Ghost.transform.position = this.m_NetworkPosition;
                    if (Ghost) Ghost.transform.rotation = this.m_NetworkRotation;


                    this.m_NetworkWantedPos += this.m_Body.velocity * lag/* * fOffset * fWantedPos*/;

                    this.m_NetworkWantedPos += this.m_Body.velocity * .07f;


                    this.m_Distance = Vector3.Distance(this.m_Body.position, this.m_NetworkWantedPos);


                /*
                    float distanceToLastPos = Vector3.Distance(this.m_Body.position, lastPosition);


                    lastPosition = this.m_Body.position;
                    lastDistance = this.m_Distance;


                    this.nextPos = this.m_NetworkWantedPos;
                    this.nextDistance = this.m_Distance;
                    */

                    this.m_NetworkWantedRot = Quaternion.Euler(this.m_Body.angularVelocity * lag/* fOffset * fWantedPos*/) * this.m_NetworkWantedRot;
                    this.m_Angle = Quaternion.Angle(this.m_Body.rotation, this.m_NetworkWantedRot);

                    if (CubeWantedPoos) CubeWantedPoos.transform.position = this.m_NetworkWantedPos;
                    if (CubeWantedPoos) CubeWantedPoos.transform.rotation = this.m_NetworkWantedRot;

                    //b_AccelerationImpact = (bool)stream.ReceiveNext();
            }
        }

        public bool returnIsMine()
        {
            return m_PhotonView.IsMine;
        }

        public string returnOwnerNickName()
        {
            return m_PhotonView.Owner.NickName;
        }


       

    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCR_Skid : MonoBehaviour
{
    private bool             Auto = true;

    private float            rayDistance = .3f;
    public float            OffsetYPosition = .16f;
    private AudioSource     carSkidSound;
    public GameObject       ObjTrail;

    private bool            lastFrameSkid = false;
    private GameObject      currentTrail;
    //public LayerMask        myLayer;


    private List<GameObject> Grp_Wheels = new List<GameObject>();
    private List<GameObject> listCurrentTrail = new List<GameObject>();

    private CarController   carController;
    private CarAI carAI;

    private Transform Front_Ray;

    void Start()
    {
        if (Auto)
        {
            Transform[] children = GetComponentsInChildren<Transform>(true);

            foreach(Transform child in children)
            {
                if(child.name == "a_Skid")
                {
                    carSkidSound = child.gameObject.GetComponent<AudioSource>();
                }

                if (child.name == "Front_Ray_SlowCar")
                {
                    Front_Ray = child;
                }
            }

            carController = GetComponent<CarController>();
            carAI = GetComponent<CarAI>();


            foreach (GameObject obj in carController.Wheel_X_Rotate)
            {
                Grp_Wheels.Add(obj.transform.parent.gameObject);
                listCurrentTrail.Add(null);
            }

            rayDistance = carController.SpringHeight;
        }

    }

    #region
    /*void FixedUpdate()
    {
        RaycastHit hit;

        if (!carSkidSound.mute && lastFrameSkid)
        {
            currentTrail = Instantiate(ObjTrail, transform);
        }

        if (Physics.Raycast(transform.position, -transform.up, out hit, rayDistance, myLayer) && !carSkidSound.mute)
        {
            Debug.DrawRay(transform.position, -transform.up * rayDistance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up * rayDistance, Color.green);
            if(currentTrail)
            currentTrail.transform.SetParent(null);
        }

        lastFrameSkid = carSkidSound.mute;
    }*/
    #endregion

    void FixedUpdate()
    {
        RaycastHit hit;


   
        if(carAI.enabled)
        {
            if(carAI.b_Impact && carSkidSound.mute)
                carSkidSound.mute = false;
            else if(!carAI.b_Impact && !carSkidSound.mute)
                carSkidSound.mute = true;
        }



        if (!carSkidSound.mute && lastFrameSkid)
        {
            for(var i = 0;i< Grp_Wheels.Count;i++)
            {
                listCurrentTrail[i] = Instantiate(ObjTrail, Grp_Wheels[i].transform);
                listCurrentTrail[i].transform.localPosition = new Vector3(0, listCurrentTrail[i].transform.localPosition.y - rayDistance + OffsetYPosition, 0);
            }
        }

        bool isGrounded = false;
        if(Front_Ray && Physics.Raycast(Front_Ray.position, -Front_Ray.up, out hit, rayDistance)){
            isGrounded = true;
        }

        for (var i = 0; i < Grp_Wheels.Count; i++)
        {
            if (Physics.Raycast(Grp_Wheels[i].transform.position, -Grp_Wheels[i].transform.up, out hit, rayDistance/*, myLayer*/) && !carSkidSound.mute && isGrounded && Front_Ray ||
                Physics.Raycast(Grp_Wheels[i].transform.position, -Grp_Wheels[i].transform.up, out hit, rayDistance/*, myLayer*/) && !carSkidSound.mute && !Front_Ray)
            {
                Debug.DrawRay(Grp_Wheels[i].transform.position, -Grp_Wheels[i].transform.up * rayDistance, Color.red);
            }
            else
            {
                Debug.DrawRay(Grp_Wheels[i].transform.position, -Grp_Wheels[i].transform.up * rayDistance, Color.green);
                if (listCurrentTrail[i])
                    listCurrentTrail[i].transform.SetParent(null);
            }
        }


        lastFrameSkid = carSkidSound.mute;
    }
}

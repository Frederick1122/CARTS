using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KMH : MonoBehaviour
{
    public Game_Manager gManager;
    //public CarController carP1;
    //public CarController carP2;
    public Text txtP1;
    public Text txtP2;

    // Update is called once per frame
    void Update()
    {
        if (gManager.list_Cars[0] && txtP1)
            txtP1.text = "KMH: " + gManager.list_Cars[0].MCR_KMH();

        if (gManager.list_Cars[0] && txtP1 && gManager.cam_P2.gameObject.activeInHierarchy)
            txtP2.text = "KMH: " + gManager.list_Cars[1].MCR_KMH();
    }
}

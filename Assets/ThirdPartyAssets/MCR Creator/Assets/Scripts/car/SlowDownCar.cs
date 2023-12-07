using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownCar : MonoBehaviour
{
    public GameObject refObj;
    private CarController carControl;
    public float rayDistance = .2f;
    public float force = -.02f;

    public LayerMask myLayer;

    private void Start()
    {
        carControl = transform.parent.parent.parent.GetComponent<CarController>();
    }

    void FixedUpdate()
    {
            RaycastHit hit;
            if (Physics.Raycast(refObj.transform.position, refObj.transform.forward, out hit, rayDistance, myLayer) && carControl.b_CarMoveForward)
            {
                Debug.DrawRay(refObj.transform.position, refObj.transform.forward * rayDistance, Color.red);
                carControl.rb.velocity *= .9f;
            }
            else
            {
                Debug.DrawRay(refObj.transform.position, refObj.transform.forward * rayDistance, Color.green);
            }
    }
}

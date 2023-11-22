using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AICarInputSystem input))
            input.BrakeZoneInteract(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out AICarInputSystem input))
            input.BrakeZoneInteract(false);
    }
}

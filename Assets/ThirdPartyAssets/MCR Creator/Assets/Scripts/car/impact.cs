using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impact : MonoBehaviour
{
    private AudioSource audioSource;
    public CarController carControl;
    private CarAI carAI;

    public float power = .5f;


    public bool b_CheckBigImpactStep_One = false;
    public bool b_CheckBigImpactStep_Two = false;

    private float BigImpactGauge = 0;
    private float TimeToEnableBigImpact = .5f;  //.25f

    private float TurnGauge = 0;
    private float TimeToTurnBeforeImpact = .5f; //1f

    private float timerDisableAccelerationAndRotation = .25f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        carControl = GetComponent<CarController>();

        if (championshipM.instance && !championshipM.instance.b_GlobalSkidMark)
        {
            if(gameObject.GetComponent<MCR_Skid>())
                Destroy(GetComponent<MCR_Skid>());
        }

    }

    private void Update()
    {
        if (!carAI && GetComponent<CarAI>() && GetComponent<CarAI>().enabled == true)
        {carAI = GetComponent<CarAI>();}


        // Check if the Big Impact is enabled
        if (!carAI && !carControl.raceIsFinished)
        {
            if (carControl.rb && carControl.rb.velocity.magnitude >= carControl.MaxSpeed)
            {
                BigImpactGauge = Mathf.MoveTowards(BigImpactGauge, TimeToEnableBigImpact, Time.deltaTime);
                if (TimeToEnableBigImpact == BigImpactGauge)
                    b_CheckBigImpactStep_One = true;
            }
            else
            {
                BigImpactGauge = 0;
                TurnGauge = 0;
                b_CheckBigImpactStep_One = false;
            }

            // Check if the car turn left or right when big impact is enabled
            if (b_CheckBigImpactStep_One)
            {
                if (!carAI && carControl && carControl.turnDirection != 0)
                {
                    b_CheckBigImpactStep_Two = true;
                    TurnGauge = Mathf.MoveTowards(TurnGauge, TimeToTurnBeforeImpact, Time.deltaTime);
                    if (TurnGauge == TimeToTurnBeforeImpact)
                    {
                        b_CheckBigImpactStep_One = false;
                        b_CheckBigImpactStep_Two = false;
                        BigImpactGauge = 0;
                    }
                }
                if (!carAI && carControl && carControl.turnDirection == 0 && TurnGauge != 0)
                {
                    b_CheckBigImpactStep_One = false;
                    b_CheckBigImpactStep_Two = false;
                    BigImpactGauge = 0;
                }
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        #region
        if (carControl && !carControl.b_AccelerationImpact && 
            collision.transform.CompareTag("Car") &&
            !carControl.raceIsFinished)
            {
                if (carAI)
                {
                    carAI.b_Impact = true;
                }
            
            carControl.BrakeForce = 0;
            carControl.Coeff = 0;
            carControl.CoeffZ = 0;
           
           // Debug.Log(collision.gameObject.name);

           
            // Add opposite for when collision detected
            if (!collision.gameObject.GetComponent<CarController>().carAI.enabled &&
            collision.gameObject.GetComponent<impact>().b_CheckBigImpactStep_Two)
            {
                //Debug.Log("Big Collision");
                power = .75f;
                timerDisableAccelerationAndRotation = 1f;
                audioSource.volume = .1f;
            }
            else
            {
                //Debug.Log("Small Collision");
                power = .5f;
                timerDisableAccelerationAndRotation = .25f;
                audioSource.volume = .05f;
            }

            carControl.rb.AddForceAtPosition(-carControl.rb.velocity.normalized * power, transform.position, ForceMode.VelocityChange);
            audioSource.Play();

            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;
            StartCoroutine(StopAcceleration(dir));
        }
        #endregion
    }

   
    public IEnumerator StopAcceleration(Vector3 dir)
    {
        #region
        carControl.b_AccelerationImpact = true;
        /*float t = 0;

        while(t < timerDisableAccelerationAndRotation)
        {
            carControl.rb.AddForceAtPosition(dir * power, transform.position, ForceMode.Force);
            t = Mathf.MoveTowards(t, timerDisableAccelerationAndRotation, Time.deltaTime);
            yield return null;
        }*/


        yield return new WaitForSeconds(timerDisableAccelerationAndRotation);

        carControl.b_AccelerationImpact = false;
        if (carAI) carAI.b_Impact = false;
        carControl.BrakeForce = 35;
        carControl.Coeff = 0.2f;
        carControl.CoeffZ = 1.5f;

        yield return null;
        #endregion
    }
}

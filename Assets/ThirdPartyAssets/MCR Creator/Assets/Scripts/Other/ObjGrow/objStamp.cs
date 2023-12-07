using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class objStamp : MonoBehaviour {
    public AnimationCurve curve = new AnimationCurve();
    public float t = 0;
    public float speed = .5f;

    public Vector3 startScale = Vector3.zero;
    public AudioSource _Source;
    public AudioClip a_Boom;

    public Vector2 startScaleMovement = new Vector2(5, 5);

    public KeyCode _Letter = KeyCode.Y;

    public bool b_WaitBeforMoving = false;

    public bool b_AnimationEnded = false;

    private void Start()
    {
       // startScale = gameObject.transform.localScale;
       // transform.gameObject.SetActive(false);
       // transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    /* void Update () {
         if(Input.GetKeyDown(_Letter)){
            // transform.gameObject.SetActive(true);
             transform.localScale = new Vector3(startScaleMovement.x,startScaleMovement.y,1);
             StartCoroutine(AP_I_LogoAnimation());
         }
     }
     */

    public void AP_LogoAnimation()
    {
        startScale = gameObject.transform.localScale;

        StartCoroutine(AP_I_LogoAnimation());
    }

    public IEnumerator AP_I_LogoAnimation(){
        if(b_WaitBeforMoving)
            yield return new WaitForSeconds(.25f);
        transform.localScale = new Vector3(startScaleMovement.x, startScaleMovement.y, 1);
        GetComponent<Image>().enabled = true;
        t = 0;
        if(a_Boom && _Source){
            _Source.clip = a_Boom;
            _Source.Play();
        }
           

        while (transform.localScale != startScale)
        {
            // t = Mathf.MoveTowards(t,1,Time.deltaTime * speed) ;
            transform.localScale = Vector3.MoveTowards(transform.localScale, startScale, Time.deltaTime * speed);
            yield return null;
        }

        if (b_WaitBeforMoving)
            yield return new WaitForSeconds(1f);
        b_AnimationEnded = true;
        yield return null;
    }

    public bool MCR_CheckIfAnimationEnded(){
        return b_AnimationEnded;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objGrow : MonoBehaviour {
    public AnimationCurve curve = new AnimationCurve();
    public float t = 0;

    public float speed = .5f;

    public Vector3 startScale = Vector3.zero;
  
 


    private void Start()
    {
        startScale = gameObject.transform.localScale;
    }
   

    // Update is called once per frame
    void Update () {
        t += Time.deltaTime * speed;

        transform.localScale = new Vector3(startScale.x * curve.Evaluate(t % 1), startScale.y * curve.Evaluate(t % 1), 1);
	}



}

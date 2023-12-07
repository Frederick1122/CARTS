using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objTranslate : MonoBehaviour {
    public AnimationCurve curve = new AnimationCurve();
    public float t = 0;

    public float speed = .5f;

    public Vector3 startPosition = Vector3.zero;

    public bool AxisX = false;
    public bool AxisY = false;
    public bool AxisZ = false;

    private void Start()
    {
        startPosition = gameObject.transform.localPosition;
    }
   

    // Update is called once per frame
    void Update () {
        t += Time.deltaTime * speed;

        Vector3 newPosition = Vector3.zero;

        if (AxisX)
            newPosition = new Vector3(startPosition.x * curve.Evaluate(t % 1), newPosition.y, newPosition.z);
        else
            newPosition = new Vector3(transform.localPosition.x, newPosition.y, newPosition.z);

        if (AxisY)
            newPosition = new Vector3(newPosition.x, startPosition.y * curve.Evaluate(t % 1), newPosition.z);
        else
            newPosition = new Vector3(newPosition.x, transform.localPosition.y, newPosition.z);
        
        if (AxisZ)
            newPosition = new Vector3(newPosition.x, newPosition.y, startPosition.z * curve.Evaluate(t % 1));
        else
            newPosition = new Vector3(newPosition.x, newPosition.y, startPosition.z * transform.localPosition.z);

       // Debug.Log(newPosition);
        transform.localPosition = new Vector3(newPosition.x,newPosition.y,newPosition.z);
	}



}

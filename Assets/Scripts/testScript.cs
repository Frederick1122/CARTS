using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public RectTransform uiObj;
    public Transform threeDObj;
    public Camera uiCamera;
    public Canvas uiCanvas;

    private LineRenderer lineRenderer;

    bool start = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (!start)
            return;

        DrawLine();
    }

    public void DrawLine()
    {
        //Vector3 uiPos = uiObj.TransformPoint(uiObj.rect.center);
        var screen = Camera.main.WorldToScreenPoint(threeDObj.transform.position);
        screen.z = (uiCanvas.transform.position - uiCamera.transform.position).magnitude;
        var pos = uiCamera.ScreenToWorldPoint(screen);
        lineRenderer.SetPosition(0, pos);
        lineRenderer.SetPosition(1, uiObj.transform.position);
    }

    [ContextMenu("start")]
    public void tt() =>
         start = true;
}

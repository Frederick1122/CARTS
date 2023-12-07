using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MCR_ButtonTasks : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler, IDeselectHandler{

    public GameObject objArrow_01;
    public GameObject objArrow_02;
    public GameObject objArrow_03;

    public int slotNumber = 0;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(slotNumber == 0){
            objArrow_01.SetActive(true);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 1)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(true);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 2)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(true);
        }

        //Debug.Log("Pointer Enter");
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (slotNumber == 0)
        {
            objArrow_01.SetActive(true);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 1)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(true);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 2)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(true);
        }
        //Debug.Log("Select");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotNumber == 0)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 1)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 2)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        //Debug.Log("Pointer Exit");
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (slotNumber == 0)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 1)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        if (slotNumber == 2)
        {
            objArrow_01.SetActive(false);
            objArrow_02.SetActive(false);
            objArrow_03.SetActive(false);
        }
        //Debug.Log("Deselect ");
    }
}

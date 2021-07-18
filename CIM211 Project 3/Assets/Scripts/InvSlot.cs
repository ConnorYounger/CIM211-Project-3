using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlot : MonoBehaviour, IDropHandler
{
    public bool canDrop = true;
    public string slotType = "Arm";

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if(eventData.pointerDrag != null)
        {
            if (canDrop && eventData.pointerDrag.GetComponent<InvDragDrop>().invItem.itemType == slotType)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().localPosition;
                eventData.pointerDrag.GetComponent<InvDragDrop>().inSlot = true;
                eventData.pointerDrag.GetComponent<InvDragDrop>().startPos = gameObject.GetComponent<RectTransform>().localPosition;
            }
            else
            {
                if (eventData.pointerDrag.GetComponent<InvDragDrop>())
                {
                    eventData.pointerDrag.GetComponent<InvDragDrop>().ResetPos();
                }
            }
        }
    }
}

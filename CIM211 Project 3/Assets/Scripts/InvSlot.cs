using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlot : MonoBehaviour, IDropHandler
{
    public bool canDrop = true;
    public string slotType = "Arm";

    public GameObject currentItem;
    private Inventory inv;

    private void Start()
    {
        inv = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
        if(eventData.pointerDrag != null)
        {
            if (canDrop && eventData.pointerDrag.GetComponent<InvDragDrop>().invItem.itemType == slotType)
            {
                if (currentItem && currentItem != eventData.pointerDrag)
                {
                    inv.RemovePlayerItem(currentItem);
                    Destroy(currentItem);
                    currentItem = null;
                }

                Debug.Log("Add Item: " + eventData.pointerDrag.GetComponent<InvDragDrop>().invItem.itemType + " to: " + gameObject.name);

                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().localPosition;
                eventData.pointerDrag.GetComponent<InvDragDrop>().inSlot = true;
                eventData.pointerDrag.GetComponent<InvDragDrop>().startPos = gameObject.GetComponent<RectTransform>().localPosition;
                inv.playerItems.Add(eventData.pointerDrag.gameObject);
                inv.UpdatePlayerEnhancements();

                currentItem = eventData.pointerDrag.gameObject;

                if (eventData.pointerDrag.GetComponent<InvDragDrop>().isEnemy)
                {
                    inv.enemyItems.Remove(eventData.pointerDrag.GetComponent<InvDragDrop>().invItem);
                    inv.currentDeadBody.inv.Remove(eventData.pointerDrag.GetComponent<InvDragDrop>().invItem);
                    inv.currentDeadBody.RemovedItem();
                    eventData.pointerDrag.GetComponent<InvDragDrop>().isEnemy = false;
                }
            }
            else
            {
                Debug.Log("Inv Slot Reset pos");

                if (eventData.pointerDrag.GetComponent<InvDragDrop>())
                {
                    //eventData.pointerDrag.GetComponent<InvDragDrop>().ResetPos();
                }
            }
        }
    }
}

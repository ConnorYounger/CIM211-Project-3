using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Inventory inv;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public Vector2 startPos;

    public bool inSlot = true;
    public bool isEnemy = true;

    public InvItem invItem;

    private void Start()
    {
        canvas = GameObject.Find("InventoryCanvas").GetComponent<Canvas>();
        inv = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        startPos = gameObject.GetComponent<RectTransform>().localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        inSlot = false;
        inv.HideAllItems();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        inv.ShowAllItems();
        ResetPos();
    }

    IEnumerator ResetDragDrop()
    {
        ResetPos();
        inSlot = true;
        yield return new WaitForSeconds(0.05f);

        if(!inSlot)
        {
            ResetPos();
            inSlot = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");

    }

    public void ResetPos()
    {
        Debug.Log("Reset Pos");

        gameObject.GetComponent<RectTransform>().anchoredPosition = startPos;
        inSlot = true;
    }
}

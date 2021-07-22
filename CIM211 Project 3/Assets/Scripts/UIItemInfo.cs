using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItemInfo : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text itemLevelText;
    public TMP_Text itemBuffText;
    public TMP_Text itemFlavourText;

    private Canvas canvas;
    private RectTransform rectTransform;

    private void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (canvas.enabled)
        {
            Vector2 pos = new Vector2(Input.mousePosition.x + 250, Input.mousePosition.y + 150);
            rectTransform.anchoredPosition = pos / canvas.scaleFactor;
        }
    }
}

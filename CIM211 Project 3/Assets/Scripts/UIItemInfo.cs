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
            float posX = 0;
            float posY = 0;

            float xBuffer = 480;
            float yBuffer = 400;

            Debug.Log(Input.mousePosition);
            Debug.Log(Screen.height - yBuffer + ", " + Input.mousePosition.y);

            if (Input.mousePosition.x < Screen.width - xBuffer)
                posX = Input.mousePosition.x + 250;
            else
                posX = Screen.width - 250;

            if (Input.mousePosition.y < Screen.height - yBuffer)
                posY = Input.mousePosition.y + 150;
            else
                posY = Screen.height - 140;

            Vector2 pos = new Vector2(posX, posY);
            rectTransform.anchoredPosition = pos / canvas.scaleFactor;
        }
    }
}

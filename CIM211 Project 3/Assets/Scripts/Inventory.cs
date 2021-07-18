using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;


public class Inventory : MonoBehaviour
{
    public Canvas canvas;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;

    private void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        player = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    public void CloseInventory()
    {
        player.enabled = true;
        canvas.enabled = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenInventory()
    {
        player.enabled = false;
        canvas.enabled = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

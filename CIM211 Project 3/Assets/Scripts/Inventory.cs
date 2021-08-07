using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;


public class Inventory : MonoBehaviour
{
    public Canvas canvas;
    public GameObject invDragDropPrefab;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    public PlayerWeaponSystem playerWeapons;

    #region inventoryStuff
    public InvSlot[] enemyInvSlots;

    public List<InvItem> enemyItems;
    public List<GameObject> spawnedItems;
    public List<GameObject> playerItems;

    public InvDeadBody currentDeadBody;

    private PlayerEnhancements playerEnhancements;

    private bool inventoryIsOpen;
    [HideInInspector()] public int invCheck;

    [Header("Tutorial")]
    public bool firstTimeOpen;
    public GameObject tutorialEGO;
    public GameModeManager gameModeManager;
    #endregion

    private void Start()
    {
        spawnedItems = new List<GameObject>();
        canvas = gameObject.GetComponent<Canvas>();
        player = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        playerEnhancements = GameObject.Find("Player").GetComponent<PlayerEnhancements>();
    }

    private void Update()
    {
        if(inventoryIsOpen == true && Input.GetKeyDown(KeyCode.E))
        {
            if (invCheck < 1)
                invCheck++;
            else
                CloseInventory();
        }
    }

    public void CloseInventory()
    {
        inventoryIsOpen = false;
        invCheck = 0;
        player.enabled = true;
        canvas.enabled = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerWeapons.WeaponsCanFire();

        currentDeadBody = null;
        RemoveItems();

        if (tutorialEGO.activeSelf)
        {
            tutorialEGO.SetActive(false);
        }
    }

    public void OpenInventory()
    {
        player.enabled = false;
        canvas.enabled = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerWeapons.WeaponsCantFire();

        if (firstTimeOpen)
        {
            tutorialEGO.SetActive(true);
            firstTimeOpen = false;
            gameModeManager.ShowObjective(2);
        }

        inventoryIsOpen = true;
    }

    public void AddEnemyInventory(InvItem i)
    {
        enemyItems.Add(i);
    }

    public void RemoveItems()
    {
        Debug.Log("Destory Items");

        foreach(GameObject item in spawnedItems)
        {
            if (item.GetComponent<InvDragDrop>().isEnemy)
            {
                Destroy(item);
            }
        }

        enemyItems.Clear();
        spawnedItems.Clear();
    }

    public void UpdateEnemyInventory(InvDeadBody dead)
    {
        currentDeadBody = dead;
        foreach(InvItem i in enemyItems)
        {
            foreach(InvSlot s in enemyInvSlots)
            {
                if(i.itemType == s.slotType)
                {
                    AddEnemyItemIntoSlot(i, s);
                }
            }
        }
    }

    void AddEnemyItemIntoSlot(InvItem i, InvSlot s)
    {
        GameObject newItem = Instantiate(invDragDropPrefab, s.GetComponent<RectTransform>().localPosition, s.GetComponent<RectTransform>().localRotation);
        newItem.GetComponent<InvDragDrop>().invItem = i;
        newItem.transform.SetParent(gameObject.transform);
        newItem.transform.SetSiblingIndex(gameObject.transform.childCount - 2);
        newItem.GetComponent<RectTransform>().anchoredPosition = s.GetComponent<RectTransform>().localPosition;
        newItem.GetComponent<Image>().sprite = i.sprite;
        s.currentItem = newItem;
        spawnedItems.Add(newItem);
        newItem.GetComponent<Image>().color = i.color;
        newItem.name = "Item: " + i.name;
    }

    public void HideAllItems()
    {
        if(playerItems.Count > 0)
        {
            foreach(GameObject i in playerItems)
            {
                i.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void ShowAllItems()
    {
        if (playerItems.Count > 0)
        {
            foreach (GameObject i in playerItems)
            {
                i.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }

    public void RemovePlayerItem(GameObject i)
    {
        playerItems.Remove(i);
    }

    public void UpdatePlayerEnhancements()
    {
        playerEnhancements.UpdateStats();
    }
}

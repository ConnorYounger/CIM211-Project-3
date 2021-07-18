using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnhancements : MonoBehaviour
{
    public Inventory inventory;

    [Header("Health")]
    public float playerMaxHealth = 100;
    public float basePlayerMaxHealth = 100;
    public float autoHealMultiplier = 0;
    public float baseAutoHealMultiplier = 0;

    [Header("Movement")]
    public float maxStamina = 0;
    public float baseMaxStamina = 100;
    public float movementSpeedMultipluer = 0;
    public float baseMovementSpeedMultipluer = 1;
    public float jumpHeightMultiplier = 0;
    public float baseJumpHeightMultiplier = 1;

    [Header("Other")]
    public int vision = 0;
    public int visionMultiplier = 0;
    public int baseVisionMultiplier = 1;
    public bool brain;

    [Header("Refrences")]
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;
    public PlayerHealth player;

    private void Start()
    {
        inventory = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
        fPSController = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        UpdateStats();
    }

    public void UpdateStats()
    {
        List<InvItem> items = new List<InvItem>();

        Debug.Log("UpdatePlayerEnhancements pt 1");

        if (inventory.playerItems.Count > 0)
        {
            Debug.Log("inv count: " + inventory.playerItems.Count);

            foreach(GameObject item in inventory.playerItems)
            {
                items.Add(item.GetComponent<InvDragDrop>().invItem);
            }
        }

        Debug.Log("UpdatePlayerEnhancements pt 2");

        if (items.Count > 0)
        {
            foreach (InvItem item in items)
            {
                playerMaxHealth += item.maxHealthMultiplier;
                maxStamina += item.maxStaminaMultiplier;
                autoHealMultiplier += item.autoHealMultilpier;
                movementSpeedMultipluer += item.movementSpeedMultiplier;
                jumpHeightMultiplier += item.jumpHeightMultiliper;
                vision += item.vision;
                brain = item.brain;
            }
        }

        SetStats();
    }

    void SetStats()
    {
        fPSController.m_WalkSpeed = baseMovementSpeedMultipluer + movementSpeedMultipluer;
        fPSController.m_RunSpeed = baseMovementSpeedMultipluer + movementSpeedMultipluer;
        fPSController.m_JumpSpeed = baseJumpHeightMultiplier + jumpHeightMultiplier;

        player.maxHealth = basePlayerMaxHealth + playerMaxHealth;
        player.autoHealMultilpier = baseAutoHealMultiplier + autoHealMultiplier;
        player.maxStamina = baseMaxStamina + maxStamina;

        SetVision();
    }

    void SetVision()
    {
        vision = baseVisionMultiplier + visionMultiplier;

        switch (vision)
        {
            case 2:
                RenderSettings.fogDensity = 0.035f;
                break;
            case 3:
                RenderSettings.fogDensity = 0.02f; ;
                break;
            default:
                RenderSettings.fogDensity = 0.05f;
                break;
        }
    }
}

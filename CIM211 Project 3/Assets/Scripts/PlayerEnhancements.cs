using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnhancements : MonoBehaviour
{
    public Inventory inventory;

    [Header("Health")]
    public float playerMaxHealthH;
    public float playerMaxHealthB;
    public float basePlayerMaxHealth = 100;
    public float autoHealMultiplier = 0;
    public float baseAutoHealMultiplier = 0;

    [Header("Movement")]
    public float maxStamina = 0;
    public float baseMaxStamina = 100;
    public float movementSpeedMultipluerL = 0;
    public float movementSpeedMultipluerR = 0;
    public float baseMovementSpeedMultipluer = 1;
    public float jumpHeightMultiplierL = 0;
    public float jumpHeightMultiplierR = 0;
    public float baseJumpHeightMultiplier = 1;

    [Header("Weapons")]
    private PlayerWeaponSystem weaponSystem;
    public int leftArmWeaponCode = 0;
    public int rightArmWeaponCode = 0;

    [Header("Other")]
    public int vision = 0;
    public int visionMultiplier = 0;
    public int baseVisionMultiplier = 1;
    public float baseBrain = 0;
    public float brainMultiplier = 0;

    [Header("Refrences")]
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;
    public PlayerHealth player;
    public EnemySpawManager enemySpawManager;
    public List<Weapon> weapons;

    private void Start()
    {
        weaponSystem = player.GetComponent<PlayerWeaponSystem>();
        inventory = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
        fPSController = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        UpdateStats();

        GetWeapons();
    }

    void GetWeapons()
    {
        weapons = new List<Weapon>();

        for(int i = 0; i < gameObject.GetComponent<PlayerWeaponSystem>().leftArmWeapons.Length; i++)
        {
            weapons.Add(gameObject.GetComponent<PlayerWeaponSystem>().rightArmWeapons[i].GetComponent<Weapon>());
            weapons.Add(gameObject.GetComponent<PlayerWeaponSystem>().leftArmWeapons[i].GetComponent<Weapon>());
        }
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
                // Set Variables
                if(item.itemType == "Head")
                    playerMaxHealthH = item.maxHealthMultiplier;
                if(item.itemType == "Body")
                    playerMaxHealthB = item.maxHealthMultiplier;
                if(item.itemType == "Lungs")
                    maxStamina = item.maxStaminaMultiplier;
                if(item.itemType == "Heart")
                    autoHealMultiplier = item.autoHealMultilpier;

                if(item.itemType == "LeftLeg")
                {
                    movementSpeedMultipluerL = item.movementSpeedMultiplier;
                    jumpHeightMultiplierL = item.jumpHeightMultiliper;
                }
                if (item.itemType == "RightLeg")
                {
                    movementSpeedMultipluerR = item.movementSpeedMultiplier;
                    jumpHeightMultiplierR = item.jumpHeightMultiliper;
                }

                if(item.itemType == "Eyes")
                    visionMultiplier = item.vision;
                if (item.brain > 0)
                    brainMultiplier = item.brain;

                if(item.leftArmWeaponCode > 0)
                    leftArmWeaponCode = item.leftArmWeaponCode;
                if(item.rightArmWeaponCode > 0)
                    rightArmWeaponCode = item.rightArmWeaponCode;
            }
        }

        SetStats();
    }

    void SetStats()
    {
        fPSController.m_WalkSpeed = baseMovementSpeedMultipluer + movementSpeedMultipluerL + movementSpeedMultipluerR;
        fPSController.m_RunSpeed = 10 + movementSpeedMultipluerL + movementSpeedMultipluerR;
        fPSController.m_JumpSpeed = baseJumpHeightMultiplier + jumpHeightMultiplierL + jumpHeightMultiplierR;

        player.maxHealth = basePlayerMaxHealth + playerMaxHealthH + playerMaxHealthB;
        player.autoHealMultilpier = baseAutoHealMultiplier + autoHealMultiplier;
        player.maxStamina = baseMaxStamina + maxStamina;

        foreach(Weapon weapon in weapons)
        {
            weapon.SetBloom(baseBrain + brainMultiplier);
        }

        weaponSystem.SetWeapons(leftArmWeaponCode, rightArmWeaponCode);

        SetVision();
    }

    void SetVision()
    {
        vision = baseVisionMultiplier + visionMultiplier;

        enemySpawManager.VisionUpdate(vision);

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

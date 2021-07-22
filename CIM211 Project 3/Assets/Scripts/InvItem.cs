using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory/InvItem")]
public class InvItem : ScriptableObject
{
    public string itemType = "Arm";
    public Sprite sprite;
    public Color color;

    [Header("Stats")]
    public float maxHealthMultiplier = 0;
    public float maxStaminaMultiplier = 0;
    public float autoHealMultilpier = 0;
    public float movementSpeedMultiplier = 0;
    public float jumpHeightMultiliper = 0;
    public int vision = 0;
    public float brain = 0;

    [Header("Weapons")]
    public int leftArmWeaponCode;
    public int rightArmWeaponCode;

    [Header("Text Elements")]
    public string itemName;
    public int itemLevel;
    public string itemBuff;
    [TextArea] public string itemFlavourText;
}

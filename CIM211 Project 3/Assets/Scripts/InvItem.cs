using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory/InvItem")]
public class InvItem : ScriptableObject
{
    public string itemType = "Arm";
    public Sprite sprite;
}

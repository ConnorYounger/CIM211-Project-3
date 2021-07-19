using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootPool1", menuName = "Manager/LootPool")]
public class LootPool : ScriptableObject
{
    public InvItem[] head;
    public InvItem[] eyes;
    public InvItem[] brain;
    public InvItem[] lungs;
    public InvItem[] heart;
    public InvItem[] body;
    public InvItem[] leftArm;
    public InvItem[] rightArm;
    public InvItem[] leftLeg;
    public InvItem[] rightLeg;
}

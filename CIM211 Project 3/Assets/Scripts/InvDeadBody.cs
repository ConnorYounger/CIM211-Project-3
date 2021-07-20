using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvDeadBody : MonoBehaviour
{
    public bool showInv;

    public InvItem head;
    public InvItem eyes;
    public InvItem brain;
    public InvItem heart;
    public InvItem lungs;
    public InvItem body;
    public InvItem leftArm;
    public InvItem rightArm;
    public InvItem leftLeg;
    public InvItem rightLeg;

    public List<InvItem> inv;

    private void Start()
    {
        inv = new List<InvItem>();

        //UpdateInventory();
    }

    public void UpdateInventory()
    {
        inv.Clear();

        if (head != null)
            inv.Add(head);
        if (eyes != null)
            inv.Add(eyes);
        if (brain != null)
            inv.Add(brain);
        if (heart != null)
            inv.Add(heart);
        if (lungs != null)
            inv.Add(lungs);
        if (body != null)
            inv.Add(body);
        if (leftArm != null)
            inv.Add(leftArm);
        if (rightArm != null)
            inv.Add(rightArm);
        if (leftLeg != null)
            inv.Add(leftLeg);
        if (rightLeg != null)
            inv.Add(rightLeg);

        showInv = true;
    }

    public void InventoryPassThroughItems(Inventory i)
    {
        foreach(InvItem item in inv)
        {
            i.AddEnemyInventory(item);
        }

        i.UpdateEnemyInventory(gameObject.GetComponent<InvDeadBody>());
    }
}

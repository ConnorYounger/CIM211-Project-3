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

    public bool showOutline;
    public Outline outline;
    public Color unLootedColour;

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
        StartCoroutine("UpdateLootStatus");
    }

    IEnumerator UpdateLootStatus()
    {
        yield return new WaitForSeconds(0.1f);

        if (outline && showOutline)
        {
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineWidth = 3;
            outline.OutlineColor = Color.cyan;
            outline.enabled = true;
        }
    }

    public void RemovedItem()
    {
        if(inv.Count <= 0 && outline && showOutline)
        {
            HideOutline();

            if (gameObject.GetComponent<BoxCollider>())
                gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void HideOutline()
    {
        outline.enabled = false;
    }

    public void InventoryOpened()
    {
        if (outline && showOutline)
        {
            outline.OutlineWidth = 1.5f;
            outline.OutlineColor = unLootedColour;

            StartCoroutine("HideOutlineAfterSeconds");
        }
    }

    IEnumerator HideOutlineAfterSeconds()
    {
        yield return new WaitForSeconds(5);

        HideOutline();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
        inv = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        SearchForBody();
    }

    void SearchForBody()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 100);

        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);
            Debug.DrawLine(transform.position, hit.point, Color.blue);

            if (hit.collider.GetComponent<InvDeadBody>() && hit.collider.GetComponent<InvDeadBody>().showInv && Input.GetKeyDown(KeyCode.E) && !inv.canvas.enabled)
            {
                inv.OpenInventory();
                hit.collider.GetComponent<InvDeadBody>().InventoryPassThroughItems(inv);
            }
        }
    }
}

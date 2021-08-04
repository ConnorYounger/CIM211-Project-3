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
        LayerMask mask = ~(LayerMask.GetMask("Enemy") + LayerMask.GetMask("Vision"));
        Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 100, mask);

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

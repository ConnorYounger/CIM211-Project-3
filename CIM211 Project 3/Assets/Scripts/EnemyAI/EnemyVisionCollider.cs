using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;

public class EnemyVisionCollider : MonoBehaviour
{
    public Enemy enemy;

    private void Update()
    {
        RaycastHit hit = CalculateHit(enemy.gameObject.transform.position, enemy.player.transform.position, ~LayerMask.GetMask("Vision"));

        Debug.DrawLine(enemy.gameObject.transform.position, hit.point, Color.black);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            RaycastHit hit = CalculateHit(enemy.gameObject.transform.position, enemy.player.transform.position, ~LayerMask.GetMask("Vision"));
            //Physics.Raycast(enemy.gameObject.transform.position, enemy.player.transform.position, out hit, enemy.playerVisionDistance, ~LayerMask.GetMask("Vision"));

            Debug.DrawLine(enemy.gameObject.transform.position, hit.point, Color.black);

            if (hit.collider && hit.collider.gameObject.name == "Player")
            {
                Debug.Log("Has found the player");
                enemy.hasFoundPlayer = true;
            }
            else
                enemy.hasFoundPlayer = false;
        }
    }

    RaycastHit CalculateHit(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        Vector3 rayAngle = (end - start).normalized;

        Physics.Raycast(start, rayAngle, out hit);
        return hit;
    }

    RaycastHit CalculateHit(Vector3 start, Vector3 end, int layerMask)
    {
        RaycastHit hit;
        Vector3 rayAngle = (end - start).normalized;

        Physics.Raycast(start, rayAngle, out hit, 100 ,layerMask);
        return hit;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            enemy.hasFoundPlayer = false;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    private float damage;
    private int maxHit;
    private int currentHit;

    private float hitStartTime;
    private float hitEndTime;

    public void SetStats(float d, int multi, float s, float f)
    {
        damage = d;
        maxHit = multi;
        hitStartTime = s;
        hitEndTime = f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHealth>())
        {
            if(currentHit < maxHit)
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
                currentHit++;
            }
        }
    }

    public IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(hitStartTime);

        gameObject.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(hitEndTime);

        gameObject.GetComponent<BoxCollider>().enabled = false;

        currentHit = 0;
    }
}

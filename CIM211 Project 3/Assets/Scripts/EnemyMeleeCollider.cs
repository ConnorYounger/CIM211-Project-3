using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;

public class EnemyMeleeCollider : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            enemy.DealMeleeDamage();
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;

    public TMP_Text healthText;

    private InvDeadBody inv;
    public EnemySpawManager spawManager;

    public bool isDead;

    void Start()
    {
        inv = gameObject.GetComponent<InvDeadBody>();
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;

            if (health > 0)
            {
                if (healthText)
                    healthText.text = health.ToString();
            }
            else
                Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("EnemyDie");

        if (healthText)
            healthText.text = "Dead X(";

        if(inv != null)
        {
            inv.UpdateInventory();
        }

        if (spawManager)
        {
            spawManager.KilledEnemy(gameObject);
        }
    }
}

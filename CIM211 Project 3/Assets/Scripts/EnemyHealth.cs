using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using StatePattern;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;

    public TMP_Text healthText;

    private InvDeadBody inv;
    public EnemySpawManager spawManager;

    public Enemy enemy;

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
            healthText.text = "Dead";

        if(inv != null)
        {
            inv.UpdateInventory();
        }

        if (spawManager)
        {
            spawManager.KilledEnemy(gameObject);
        }

        if (enemy)
        {
            enemy.enabled = false;

            if (gameObject.GetComponent<NavMeshAgent>())
                gameObject.GetComponent<NavMeshAgent>().enabled = false;

            if (gameObject.GetComponent<Rigidbody>())
            {
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }

    public void ShowHealthUI()
    {
        healthText.enabled = true;
        healthText.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideHealthUI()
    {
        healthText.enabled = false;
        healthText.transform.GetChild(0).gameObject.SetActive(false);
    }
}

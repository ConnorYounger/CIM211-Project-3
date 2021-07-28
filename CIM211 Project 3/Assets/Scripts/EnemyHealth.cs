using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using StatePattern;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;
    public float healthRegenMultiplier;
    private bool canHeal = true;

    public TMP_Text healthText;

    private InvDeadBody inv;
    public EnemySpawManager spawManager;

    public Enemy enemy;

    public MeshRenderer meshRenderer;

    public bool isDead;

    void Start()
    {
        currentHealth = maxHealth;

        inv = gameObject.GetComponent<InvDeadBody>();
    }

    private void Update()
    {
        AutoHeal();
    }

    void AutoHeal()
    {
        if(canHeal && healthRegenMultiplier > 0 && !isDead)
        {
            if(currentHealth < maxHealth)
            {
                currentHealth += 10 * healthRegenMultiplier * Time.deltaTime;
                UpdateHealthUI();
            }
            else if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
                UpdateHealthUI();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;

            if (currentHealth > 0)
            {
                UpdateHealthUI();

                StopCoroutine("AutoHealCoolDown");
                StartCoroutine("AutoHealCoolDown");

                enemy.hasFoundPlayer = true;
            }
            else
                Die();
        }
    }

    public void UpdateHealthUI()
    {
        if (healthText)
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();
    }

    IEnumerator AutoHealCoolDown()
    {
        canHeal = false;

        yield return new WaitForSeconds(2);

        canHeal = true;
    }

    void Die()
    {
        isDead = true;

        Debug.Log("EnemyDie");

        if (healthText)
        {
            healthText.text = "Dead";
            HideHealthUI();
        }

        if (inv != null)
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

        if (meshRenderer)
        {
            meshRenderer.material.color = Color.red;
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

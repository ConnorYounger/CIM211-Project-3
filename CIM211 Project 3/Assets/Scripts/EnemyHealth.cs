using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Slider healthSlider;

    private InvDeadBody inv;
    public EnemySpawManager spawManager;

    public Enemy enemy;

    public MeshRenderer meshRenderer;

    public bool isDead;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip deathSound;

    [Space()]
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;

        inv = enemy.GetComponent<Enemy>().inventory;

        audioSource = gameObject.GetComponent<AudioSource>();
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

            PlaySound(damageSound);

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

        if (healthSlider)
            healthSlider.value = currentHealth / maxHealth;
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
                //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                //rb.constraints = RigidbodyConstraints.None;
            }

            if (gameObject.GetComponent<BoxCollider>())
                gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (meshRenderer)
        {
            meshRenderer.material.color = Color.red;
        }

        if (audioSource && deathSound)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
        }

        if (animator)
            animator.enabled = false;
    }

    public void PlaySound(AudioClip sound)
    {
        GameObject soundOb = Instantiate(new GameObject(), transform.position, transform.rotation);
        AudioSource aSource = soundOb.AddComponent<AudioSource>();

        aSource.volume = PlayerPrefs.GetFloat("audioVolume");
        aSource.spatialBlend = 1;
        aSource.maxDistance = 100;
        aSource.clip = sound;
        aSource.Play();

        Destroy(soundOb, sound.length);
    }

    public void ShowHealthUI()
    {
        //healthText.enabled = true;
        healthText.transform.GetChild(0).gameObject.SetActive(true);
        UpdateHealthUI();
    }

    public void HideHealthUI()
    {
        healthText.enabled = false;
        healthText.transform.GetChild(0).gameObject.SetActive(false);
    }
}

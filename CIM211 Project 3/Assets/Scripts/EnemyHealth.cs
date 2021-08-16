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
    public float currentHealth;
    public float healthRegenMultiplier;
    private bool canHeal = true;

    public TMP_Text healthText;
    public Slider healthSlider;

    private InvDeadBody inv;
    public EnemySpawManager spawManager;

    public Enemy enemy;

    public MeshRenderer meshRenderer;

    public ParticleSystem[] dmgeffects;

    public bool isDead;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip deathSound;

    [Space()]
    public Animator animator;
    public Rigidbody[] limbs;

    [Header("Tutorial")]
    public bool firstEnemy;
    public GameModeManager gameModeManager;

    [Header("Drone")]
    public RiotDrone drone;

    void Start()
    {
        currentHealth = maxHealth;

        if(enemy)
            inv = enemy.GetComponent<Enemy>().inventory;

        audioSource = gameObject.GetComponent<AudioSource>();

        if (limbs.Length > 0)
        {
            for (int i = 0; i < limbs.Length; i++)
            {
                limbs[i].constraints = RigidbodyConstraints.FreezeAll;
            }
        }
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

                if(enemy)
                    enemy.hasFoundPlayer = true;

                if(dmgeffects.Length > 0)
                {
                    foreach(ParticleSystem system in dmgeffects)
                    {
                        system.Play();
                    }
                }
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

    public void Die()
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

        if(limbs.Length > 0)
        {
            for(int i = 0; i < limbs.Length; i++)
            {
                limbs[i].constraints = RigidbodyConstraints.None;
            }
        }

        if (animator)
            animator.enabled = false;

        if (firstEnemy && gameModeManager)
        {
            gameModeManager.ShowObjective(1);
        }

        if (drone)
        {
            drone.droneSpawner.RemoveDrone(drone.gameObject, 10);
            drone.alive = false;

            if (drone.gameObject.GetComponent<Rigidbody>())
            {
                drone.gameObject.GetComponent<Rigidbody>().useGravity = true;
                drone.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                float rand = 45;
                float randx = Random.Range(-rand, rand);
                float randy = Random.Range(-rand, rand);
                float randz = Random.Range(-rand, rand);

                drone.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(randx, randy, randz));
            }

            if (drone.destroyFx)
            {
                GameObject fx = Instantiate(drone.destroyFx, transform.position, Quaternion.identity);
                Destroy(fx, 5);
            }

            if (drone.audioSource)
                drone.audioSource.enabled = false;

            if (drone.eyeGlow)
                drone.eyeGlow.gameObject.SetActive(false);

            drone.enabled = false;
        }
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

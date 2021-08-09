using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    public float autoHealMultilpier;
    public float damageCoolDownTime = 0.2f;
    private bool canTakeDamage = true;
    private bool canAutoHeal = true;
    public float autoHealCoolDown = 1;

    [Header("Sprinting")]
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;
    public float maxStamina;
    public float currentStamina;
    public float stamConsumption = 10000;
    public bool canSprint = true;
    public float restoreStaminaCoolDown = 1;
    private bool restoreStamina;
    public Color stamColour;
    public Color stamConsumptionColour;
    public Color stamRechargeColour;

    [Header("UI Refrences")]
    public Canvas playerHUD;
    public Canvas deathCanvas;
    public Slider staminaSlider;
    public Slider healthSlider;
    public TMP_Text healthText;
    public TMP_Text stamText;

    public Animator uIHitEffect;
    public Animator hitCamAni;
    public PostProcessVolume ppVolume;
    public PostProcessProfile defultProfile;
    public PostProcessProfile lowHealthProfile;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] hitSounds;
    private int currentHitSound;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        fPSController = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        StartCoroutine("SetCurrentHealth");
    }

    IEnumerator SetCurrentHealth()
    {
        yield return new WaitForSeconds(0.2f);

        currentHealth = maxHealth;
        UpdateSliders();
    }

    // Update is called once per frame
    void Update()
    {
        StaminaCoolDown();
        AutoHeal();
        LowHealth();
    }

    void LowHealth()
    {
        if(currentHealth <= maxHealth / 4 || currentHealth < 20)
        {
            uIHitEffect.SetBool("lowHealth", true);
            ppVolume.profile = lowHealthProfile;
        }
        else if (uIHitEffect.GetBool("lowHealth"))
        {
            uIHitEffect.SetBool("lowHealth", false);
            ppVolume.profile = defultProfile;
        }
    }
    
    void AutoHeal()
    {
        if (canAutoHeal && autoHealMultilpier > 0)
        {
            if(currentHealth < maxHealth)
            {
                currentHealth += 2 * autoHealMultilpier * Time.deltaTime;
                UpdateSliders();
            }
            else
            {
                currentHealth = maxHealth;
                canAutoHeal = false;
            }
        }
    }

    void StaminaCoolDown()
    {
        if(!canSprint && currentStamina > 25)
        {
            canSprint = true;
            fPSController.canSprint = true;
            staminaSlider.fillRect.GetComponent<Image>().color = stamColour;
        }

        if (restoreStamina)
        {
            if(currentStamina < maxStamina)
                currentStamina +=  40 * (maxStamina / 100) * Time.deltaTime;
            else
            {
                currentStamina = maxStamina;
                staminaSlider.fillRect.GetComponent<Image>().color = stamColour;
            }
            UpdateSliders();
        }
    }

    public void ConsumeStamina()
    {
        restoreStamina = false;
        currentStamina -= 30 * stamConsumption * Time.deltaTime;

        staminaSlider.fillRect.GetComponent<Image>().color = stamConsumptionColour;

        if (currentStamina <= 0 && canSprint)
        {
            fPSController.canSprint = false;
            canSprint = false;
        }

        UpdateSliders();
        StopCoroutine("RestoreStamina");
        StartCoroutine("RestoreStamina");
    }

    public void UpdateSliders()
    {
        staminaSlider.value = currentStamina / maxStamina;
        healthSlider.value = currentHealth / maxHealth;

        healthText.text = Mathf.Round(currentHealth) + "/" + maxHealth;
        stamText.text = Mathf.Round(currentStamina) + "/" + maxStamina;
    }

    IEnumerator RestoreStamina()
    {
        if(canSprint)
            staminaSlider.fillRect.GetComponent<Image>().color = stamColour;
        else
            staminaSlider.fillRect.GetComponent<Image>().color = stamRechargeColour;

        yield return new WaitForSeconds(restoreStaminaCoolDown);

        //staminaSlider.fillRect.GetComponent<Image>().color = stamRechargeColour;

        restoreStamina = true;
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
            StartCoroutine("TakeDamageCoolDown");

            if (uIHitEffect)
                uIHitEffect.Play("TakeDamage");

            StopCoroutine("AutoHealCoolDown");
            StartCoroutine("AutoHealCoolDown");

            if (hitCamAni)
                hitCamAni.Play("PlayerHitCam");
        }

        PlayHitSound();

        if (currentHealth <= 0)
        {
            Lose();
        }
    }

    void PlayHitSound()
    {
        if (audioSource)
        {
            if (currentHitSound < hitSounds.Length)
            {
                currentHitSound++;
            }
            else
            {
                currentHitSound = 1;
            }

            Debug.Log("Current Hit Sound: " + currentHitSound);

            audioSource.clip = hitSounds[currentHitSound - 1];
            audioSource.Play();
        }
    }

    IEnumerator TakeDamageCoolDown()
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(damageCoolDownTime);

        canTakeDamage = true;
    }

    public IEnumerator AutoHealCoolDown()
    {
        canAutoHeal = false;

        yield return new WaitForSeconds(autoHealCoolDown);

        canAutoHeal = true;
    }

    void Lose()
    {
        DeathMenu();

        Debug.Log("Player lose");
    }

    public void DeathMenu()
    {
        if (currentHealth <= 0)
        {
            Time.timeScale = 0;

            fPSController.enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (deathCanvas)
            {
                deathCanvas.enabled = true;
                deathCanvas.transform.GetChild(0).gameObject.SetActive(true);
            }

            if (playerHUD)
                playerHUD.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    public float autoHealMultilpier;

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
    public Slider staminaSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        fPSController = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        StaminaCoolDown();
    }

    void StaminaCoolDown()
    {
        if(!canSprint && currentStamina > 25)
        {
            canSprint = true;
            fPSController.canSprint = true;
            staminaSlider.fillRect.GetComponent<Image>().color = stamColour;
            // change bar color
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

    void UpdateSliders()
    {
        staminaSlider.value = currentStamina / maxStamina;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialEGO;
    public GameObject waveEGO;
    public GameObject[] slides;
    private int currentIndex;

    public TMP_Text buttonText;

    public EnemySpawManager spawnManager;

    [Header("Player Refs")]
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;
    public PlayerWeaponSystem playerWeaponSystem;
    public GameObject playerUI;

    public void ShowTutorial()
    {
        waveEGO.SetActive(false);
        tutorialEGO.SetActive(true);

        slides[0].SetActive(true);

        //Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextSlide()
    {
        currentIndex++;

        for(int i = 0; i < slides.Length; i++)
        {
            if(i == currentIndex)
            {
                slides[i].SetActive(true);
            }
            else
            {
                slides[i].SetActive(false);
            }
        }

        if (currentIndex == slides.Length)
        {
            FinishTutorial();
        }
        else if (currentIndex == slides.Length - 1)
        {
            buttonText.text = "Continue";
        }
        else
        {
            buttonText.text = "Next";
        }
    }

    public void FinishTutorial()
    {
        foreach(GameObject slide in slides)
        {
            slide.SetActive(false);
        }

        tutorialEGO.SetActive(false);

        //Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerUI.SetActive(true);
        waveEGO.SetActive(true);

        fPSController.enabled = true;
        playerWeaponSystem.enabled = true;

        spawnManager.NewWave();
    }
}

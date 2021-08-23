using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameModeManager : MonoBehaviour
{
    public int gameMode;

    public EnemySpawManager spawnManager;
    public Tutorial tutorial;

    [Header("Cutscene Refrences")]
    public GameObject playerUI;
    public GameObject waveUI;
    public GameObject startCutsceneEGO;
    public GameObject middleCutsceneEGO;
    public GameObject endCutsceneEGO;
    public GameObject levelStartZone;

    public TMP_Text startCutsceneButton;
    public TMP_Text middleCutsceneButton;
    public TMP_Text endCutsceneButton;

    [Header("Cutscene Audio")]
    public AudioManager audioManager;
    public AudioClip cutsceneTrack1;
    public AudioClip cutsceneTrack2;

    public GameObject[] objectives;

    [HideInInspector()] public GameObject firstEnemy;

    [Header("Player Refs")]
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;
    public PlayerWeaponSystem playerWeaponSystem;

    public Inventory playerInventory;

    public bool hasStarted;
    public bool inTutorial;
    public bool inCutscene;

    // Start is called before the first frame update
    void Start()
    {
        SetGameMode();

        //if(GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
        //    fPSController = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    void SetGameMode()
    {
        gameMode = PlayerPrefs.GetInt("gameMode");

        spawnManager.SetGameMode(gameMode);

        switch (gameMode)
        {
            case 0:
                StartCoroutine("PlayStartCutscene");
                inTutorial = true;
                break;
            case 1:
                waveUI.SetActive(true);
                hasStarted = true;
                break;
        }
    }

    IEnumerator PlayStartCutscene()
    {
        fPSController.enabled = false;
        playerWeaponSystem.WeaponsCantFire();

        playerUI.SetActive(false);
        startCutsceneEGO.SetActive(true);
        startCutsceneButton.text = "Skip";

        inCutscene = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (audioManager)
            audioManager.PlayMusicTrack(cutsceneTrack1);

        // Wait for cutscene legnth
        yield return new WaitForSeconds(5);

        startCutsceneButton.text = "Continue";
    }

    public void FinishStartCutscene()
    {
        startCutsceneEGO.SetActive(false);

        tutorial.ShowTutorial();
        playerInventory.firstTimeOpen = true;
        levelStartZone.SetActive(true);

        inCutscene = false;

        if (audioManager)
            audioManager.PlayMusicTrack();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public IEnumerator PlayMiddleCutscene()
    {
        fPSController.enabled = false;
        playerWeaponSystem.WeaponsCantFire();

        playerUI.SetActive(false);
        middleCutsceneEGO.SetActive(true);
        middleCutsceneButton.text = "Skip";

        inCutscene = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (audioManager)
            audioManager.PlayMusicTrack(cutsceneTrack1);

        yield return new WaitForSeconds(5);

        middleCutsceneButton.text = "Continue";
    }

    public void FinishMiddleSutScene()
    {
        middleCutsceneEGO.SetActive(false);
        playerUI.SetActive(true);

        fPSController.enabled = true;
        playerWeaponSystem.WeaponsCanFire();

        spawnManager.NewWave();

        inCutscene = false;

        if (audioManager)
            audioManager.PlayMusicTrack();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public IEnumerator PlayEndCutscene()
    {
        fPSController.enabled = false;
        playerWeaponSystem.WeaponsCantFire();

        playerUI.SetActive(false);
        endCutsceneEGO.SetActive(true);
        endCutsceneButton.text = "Skip";

        inCutscene = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (audioManager)
            audioManager.PlayMusicTrack(cutsceneTrack2);

        yield return new WaitForSeconds(5);

        endCutsceneButton.text = "Continue";
    }

    public void FinishEndCutscene()
    {
        endCutsceneEGO.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("Credits");
    }

    public void ShowObjective(int number)
    {
        if (!hasStarted)
        {
            if (number < objectives.Length)
            {
                for (int i = 0; i < objectives.Length; i++)
                {
                    if (i == number)
                        objectives[i].SetActive(true);
                    else
                        objectives[i].SetActive(false);
                }
            }
            else
            {
                foreach (GameObject objective in objectives)
                {
                    objective.SetActive(false);
                }
            }
        }

        if(number == 2 && !hasStarted)
        {
            waveUI.SetActive(true);
            spawnManager.NewWave();
            firstEnemy.GetComponent<Outline>().enabled = false;
            playerInventory.firstTimeOpen = false;
            playerInventory.invCheck = 5;

            if (firstEnemy.GetComponent<EnemyHealth>().currentHealth > 0)
            {
                firstEnemy.GetComponent<EnemyHealth>().firstEnemy = false;
                firstEnemy.GetComponent<EnemyHealth>().Die();
            }

            StartCoroutine("EndTutorial");
            hasStarted = true;
        }
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(5);

        ShowObjective(3);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    public int gameMode;

    public EnemySpawManager spawnManager;
    public Tutorial tutorial;

    public GameObject playerUI;
    public GameObject waveUI;
    public GameObject startCutsceneEGO;
    public GameObject middleCutsceneEGO;
    public GameObject endCutsceneEGO;
    public GameObject levelStartZone;

    public GameObject[] objectives;

    [HideInInspector()] public GameObject firstEnemy;

    [Header("Player Refs")]
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;
    public PlayerWeaponSystem playerWeaponSystem;

    public Inventory playerInventory;

    private bool hasStarted;

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
                break;
            case 1:
                waveUI.SetActive(true);
                break;
        }
    }

    IEnumerator PlayStartCutscene()
    {
        fPSController.enabled = false;
        playerWeaponSystem.enabled = false;

        playerUI.SetActive(false);
        startCutsceneEGO.SetActive(true);

        // Wait for cutscene legnth
        yield return new WaitForSeconds(5);

        startCutsceneEGO.SetActive(false);
        //playerUI.SetActive(true);

        //fPSController.enabled = true;
        //playerWeaponSystem.enabled = true;

        tutorial.ShowTutorial();
        playerInventory.firstTimeOpen = true;
        levelStartZone.SetActive(true);
        //spawnManager.NewWave();
    }

    public IEnumerator PlayMiddleCutscene()
    {
        fPSController.enabled = false;
        playerWeaponSystem.enabled = false;

        playerUI.SetActive(false);
        middleCutsceneEGO.SetActive(true);

        yield return new WaitForSeconds(5);

        middleCutsceneEGO.SetActive(false);
        playerUI.SetActive(true);

        fPSController.enabled = true;
        playerWeaponSystem.enabled = true;

        spawnManager.NewWave();
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

    public IEnumerator PlayEndCutscene()
    {
        fPSController.enabled = false;
        playerWeaponSystem.enabled = false;

        playerUI.SetActive(false);
        endCutsceneEGO.SetActive(true);

        yield return new WaitForSeconds(5);

        endCutsceneEGO.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        SceneManager.LoadScene("Credits");
    }
}

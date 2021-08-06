using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    public int gameMode;

    public EnemySpawManager spawnManager;

    public GameObject playerUI;
    public GameObject startCutsceneEGO;
    public GameObject middleCutsceneEGO;
    public GameObject endCutsceneEGO;

    [Header("Player Refs")]
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;
    public PlayerWeaponSystem playerWeaponSystem;

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
                //spawnManager.StartCoroutine("SpawnNewWave");
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
        playerUI.SetActive(true);

        fPSController.enabled = true;
        playerWeaponSystem.enabled = true;

        spawnManager.NewWave();
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

    public IEnumerator PlayEndCutscene()
    {
        fPSController.enabled = false;
        playerWeaponSystem.enabled = false;

        playerUI.SetActive(false);
        endCutsceneEGO.SetActive(true);

        yield return new WaitForSeconds(5);

        endCutsceneEGO.SetActive(false);
        SceneManager.LoadScene("Credits");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject[] gamePlayCanvas;
    public GameObject pauseCanvas;
    public Canvas inventoryCanvas;
    public GameObject optionsCanvas;

    public Options options;
    public AudioManager audioManager;

    public bool isPauseMenu = true;

    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;

    [Header("Player")]
    public PlayerHealth playerHealth;
    public PlayerWeaponSystem playerWeapons;

    public GameModeManager gameModeManager;

    // Start is called before the first frame update
    void Start()
    {
        if(isPauseMenu)
            fPSController = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPauseMenu)
        {
            PauseInput();
        }
    }

    void PauseInput()
    {
        if (!pauseCanvas.activeSelf)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        fPSController.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        pauseCanvas.SetActive(true);

        foreach(GameObject canvas in gamePlayCanvas)
        {
            canvas.SetActive(false);
        }

        if (optionsCanvas)
            optionsCanvas.SetActive(false);

        if (playerWeapons)
            playerWeapons.WeaponsCantFire();

        inventoryCanvas.enabled = false;
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);

        if (optionsCanvas)
            optionsCanvas.SetActive(false);

        foreach (GameObject canvas in gamePlayCanvas)
        {
            canvas.SetActive(true);
        }

        if (playerHealth)
            playerHealth.DeathMenu();

        Time.timeScale = 1;

        if (gameModeManager && gameModeManager.inTutorial)
            return;

        if (playerWeapons)
            playerWeapons.WeaponsCanFire();

        fPSController.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenOptionsMenu()
    {
        pauseCanvas.SetActive(false);

        options.UpdateVolumeSliders();

        if (optionsCanvas)
            optionsCanvas.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        options.UpdatePlayerPrefs();

        if (optionsCanvas)
            optionsCanvas.SetActive(false);

        if (audioManager)
            audioManager.UpdateVolume();

        pauseCanvas.SetActive(true);
    }

    public void LoadScene(string scene)
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(scene);
    }

    public void StartStoryMode()
    {
        PlayerPrefs.SetInt("gameMode", 0);
        SceneManager.LoadScene("Test Scene");

        Time.timeScale = 1;
    }

    public void StartArcadeMode()
    {
        PlayerPrefs.SetInt("gameMode", 1);
        SceneManager.LoadScene("Test Scene");

        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

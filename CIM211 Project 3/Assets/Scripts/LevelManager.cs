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

    public bool isPauseMenu = true;

    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;

    [Header("Player")]
    public PlayerHealth playerHealth;
    public PlayerWeaponSystem playerWeapons;

    // Start is called before the first frame update
    void Start()
    {
        if(isPauseMenu)
            fPSController = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
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
        Time.timeScale = 1;

        fPSController.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseCanvas.SetActive(false);

        if (optionsCanvas)
            optionsCanvas.SetActive(false);

        foreach (GameObject canvas in gamePlayCanvas)
        {
            canvas.SetActive(true);
        }

        if (playerHealth)
            playerHealth.DeathMenu();

        if (playerWeapons)
            playerWeapons.WeaponsCanFire();
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

        pauseCanvas.SetActive(true);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);

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

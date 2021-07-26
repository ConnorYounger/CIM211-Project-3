using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Canvas[] gamePlayCanvas;
    public Canvas pauseCanvas;
    public Canvas inventoryCanvas;

    public bool isPauseMenu = true;

    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;

    // Start is called before the first frame update
    void Start()
    {
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
        if (!pauseCanvas.enabled)
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

        pauseCanvas.enabled = true;

        foreach(Canvas canvas in gamePlayCanvas)
        {
            canvas.enabled = false;
        }

        inventoryCanvas.enabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        fPSController.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseCanvas.enabled = false;

        foreach (Canvas canvas in gamePlayCanvas)
        {
            canvas.enabled = true;
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
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

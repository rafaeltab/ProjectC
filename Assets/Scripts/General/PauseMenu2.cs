using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu2 : MonoBehaviour
{
    public static bool gamePaused = false;
    public static bool options = false;
    public static bool loading = true;

    public GameObject pauseMenuUI;
    public GameObject player;

    /// <summary>
    /// Listens for the pause button ESC, and pauses time if paused
    /// </summary>
    void Update()
    {
        if (gamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (!options && !loading)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gamePaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }      
    }

    /// <summary>
    /// Resumes the game
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (!InventoryManager.inventoryEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        gamePaused = false;
        player.GetComponentInChildren<CameraMovement>().enabled = true;
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gamePaused = true;
        player.GetComponentInChildren<CameraMovement>().enabled = false;
    }

    /// <summary>
    /// Loads the options menu
    /// </summary>
    public void LoadOptions()
    {
        OptionsLoader.GetInstance((c)=> { c.Open(pauseMenuUI); });
        pauseMenuUI.SetActive(false);
        options = true;
        Debug.Log("Loading options");
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Tooltip("Allow the tutorial to run on startup should be true if in production false if you don't want to run tutorial")]
    public bool AllowStartTutorial = true;

    public void Start()
    {
        //ResetTutorial();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Additive);

        //If player hasn't finished the tutorial before
        if (!PlayerPrefsX.GetBool("tutorialDone", false) && AllowStartTutorial)
        {
            Debug.Log("Not finished tutorial");
            Cursor.visible = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
        else
        {
            Debug.Log("Done tutorial");
        }
    }

    //Moves the scene up one level to the game scene
    public void StartGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Opens Options menu
    public void OptionsMenu()
    {
        OptionsLoader.GetInstance((c)=> { c.Open(); });
    }

    //Quits game
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    //Start tutorial
    public void StartTutorial()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("Tutorial");
    }

    //Sets playerpref tutorialDone to false
    public void ResetTutorial()
    {
        PlayerPrefsX.SetBool("tutorialDone", false);
    }
}

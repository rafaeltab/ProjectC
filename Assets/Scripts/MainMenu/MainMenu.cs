using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        //ResetTutorial();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Additive);

        if (!PlayerPrefsX.GetBool("tutorialDone", false))
        {
            Debug.Log("Not finished tutorial");
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

    public void ResetTutorial()
    {
        PlayerPrefsX.SetBool("tutorialDone", false);
    }
}

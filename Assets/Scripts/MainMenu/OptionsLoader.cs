using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for loading options
/// </summary>
public class OptionsLoader : MonoBehaviour
{
    private static OptionsLoader _instance;
    public static bool loaded = false;

    /// <summary>
    /// Loads the options scene into the current one
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
        Debug.Log("ran");
        loaded = true;
        gameObject.SetActive(false);

        if (doWhenReady != null)
        {
            doWhenReady(this);
        }
    }

    /// <summary>
    /// Activate options screen
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Activates options screen in esc menu and saves gameobject for enabling later
    /// </summary>
    GameObject obj;
    public void Open(GameObject obj)
    {
        gameObject.SetActive(true);
        this.obj = obj;
    }

    /// <summary>
    /// Closes options menu and activates saved gameobject
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
        if (obj != null)
        {
            obj.SetActive(true);
        }
        PauseMenu2.options = false;
    }

    private static Action<OptionsLoader> doWhenReady;

    /// <summary>
    /// Get instance of options loader
    /// </summary>
    public static void GetInstance(Action<OptionsLoader> DoWhenReady)
    {
        if(_instance == null)
        {
            doWhenReady = DoWhenReady;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
        }
        else
        {
            doWhenReady = null;
            DoWhenReady(_instance);
        }
    }
}

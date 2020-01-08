using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsLoader : MonoBehaviour
{
    private static OptionsLoader _instance;
    public static bool loaded = false;

    // Start is called before the first frame update
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

    public void Open()
    {
        gameObject.SetActive(true);
    }
    GameObject obj;
    public void Open(GameObject obj)
    {
        gameObject.SetActive(true);
        this.obj = obj;
    }

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

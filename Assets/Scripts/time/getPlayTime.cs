using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getPlayTime : MonoBehaviour
{
    float timeOld;
    float timeRunning;
    float rounded;
    TimeSpan actualTime;
    void Start()
    {
        //turns time into a readable format (just here to remember wehn it should be used)
        //timeOld = PlayerPrefs.GetFloat("timeOld");
        //print(timeOld);
        //timeRunning = Time.time;
        //rounded = UnityEngine.Mathf.Round(timeRunning);
        //actualTime = TimeSpan.FromSeconds(rounded);
    }

    void OnApplicationQuit()
    {
        timeRunning = Time.time;
        rounded = UnityEngine.Mathf.Round(timeRunning);
        PlayerPrefs.SetFloat("timeOld", rounded+timeOld);
    }
}

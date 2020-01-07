using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public static musicLoop musicLoopInstance;

    /// <summary>
    /// Get the instance of musicLoop
    /// </summary>
    void Start()
    {
        musicLoopInstance = musicLoop.instance;
    }

    /// <summary>
    /// Changes audio by pressing right mouse button
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (musicLoopInstance.clipNumber == musicLoopInstance.audioClipArray.Length - 1)
            {
                musicLoopInstance.clipNumber = 0;
            }
            else
            {
                musicLoopInstance.clipNumber += 1;
            }

            musicLoopInstance.PlayMusic();
        }
    }
}

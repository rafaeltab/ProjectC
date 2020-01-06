using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicLoop : MonoBehaviour
{

    public static musicLoop instance;
    public AudioSource _as;
    public AudioClip[] audioClipArray;
    public int clipNumber = 0;

    void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    void Start()
    {
        instance = this;
    }

    /// <summary>
    /// If not playing, play music
    /// </summary>
    void Update()
    {
        if (!_as.isPlaying)
        {
            PlayMusic();
        }
    }

    /// <summary>
    /// Stops and then plays an audioclip with the clipNumber
    /// </summary>
    public void PlayMusic()
    {
        _as.Stop();
        _as.clip = audioClipArray[clipNumber];
        _as.Play();
    }
}

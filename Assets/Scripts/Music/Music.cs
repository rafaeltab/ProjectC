using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music instance;
    public AudioSource _as;
    public AudioClip[] audioClipArray;
    public int clipNumber;

    void Awake()
    {
        instance = this;
        _as = GetComponent<AudioSource>();
    }

    void Start()
    {
        clipNumber = Random.Range(0, audioClipArray.Length - 1);
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
        if (_as.isPlaying) { _as.Stop(); }
        _as.clip = audioClipArray[clipNumber];
        _as.Play();
    }
}

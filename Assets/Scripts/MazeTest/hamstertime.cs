using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class hamstertime : MonoBehaviour
{
    public GameObject hamster;
    public GameObject subtitles;
    void Start()
    {
        subtitles.SetActive(true);
        Invoke("Hidehamster", 35.0f);
    }

    void Hidehamster()
    {
        hamster.SetActive(false);
        subtitles.SetActive(false);

    }
}
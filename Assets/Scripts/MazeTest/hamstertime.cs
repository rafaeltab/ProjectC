using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class hamstertime : MonoBehaviour
{
    public GameObject hamster;
    void Start()
    {
        Invoke("Hidehamster", 35.0f);
    }

    void Hidehamster()
    {
        hamster.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnOnAudio : MonoBehaviour
{
    public static Sound scriptInstance;
    public static GameObject Hamster;
    public GameObject subtitles;
    public TextMeshProUGUI textBoxText;

    private bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        Hamster = GameObject.Find("jesus (1)");
        scriptInstance = Hamster.GetComponent<Sound>();
    }

    //Start audio if touching collider
    private void OnTriggerEnter(Collider other)
    {
        if (!done && other.gameObject.name == "Player")
        {
            done = true;
            textBoxText.text = "";
            subtitles.SetActive(true);
            scriptInstance.MazeFinish();
        }
    }
}

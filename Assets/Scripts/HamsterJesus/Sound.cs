using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource myAudio;
    public GameObject player;
    public Transform playerTransform;
    string currentClip;

    public AudioClip fransisco3;
    public AudioClip fransisco4;
    public AudioClip fransisco5;
    public AudioClip fransisco6;
    public AudioClip fransisco7;
    public AudioClip kelly1;

    Vector3 targetPosition;
    public float speed = 15.0f;
    float step;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("audioFinished", myAudio.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        step =  speed * Time.deltaTime;
        transform.LookAt(playerTransform);
        moveTowardsPoint();
    }

    void audioFinished()
    {
        Debug.Log("Audio finished");
    }
    public void switchAudio()
    {
        currentClip = myAudio.clip.name;
        switch (currentClip)
        {
            case "fransisco1":
                myAudio.clip = fransisco3;
                myAudio.Play();
                break;
            case "fransisco3":
                myAudio.clip = fransisco4;
                myAudio.Play();
                break;
            case "fransisco4":
                myAudio.clip = fransisco5;
                myAudio.Play();
                break;
            case "fransisco5":
                myAudio.clip = fransisco6;
                myAudio.Play();
                break;
            case "fransisco6":
                myAudio.clip = fransisco7;
                myAudio.Play();
                break;
            default:
                break;
        }
    }

    public void moveTowardsPoint()
    {
        currentClip = myAudio.clip.name;
        switch (currentClip)
        {
            case "fransisco4":
                targetPosition = new Vector3(294.45f, 3.12f, 193.72f);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                break;
            case "fransisco5":
                targetPosition = new Vector3(294.45f, 3.12f, 199.54f);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                break;
            case "fransisco6":
                targetPosition = new Vector3(296.75f, 3.11f, 203.22f);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                break;
            case "fransisco7":
                targetPosition = new Vector3(294.45f, 3.12f, 252f);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step * 4);
                break;
            default:
                break;
        }
    }
    
}

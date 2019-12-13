using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound : MonoBehaviour
{
    public AudioSource myAudio;
    public GameObject player;
    public Transform playerTransform;
    public TextMeshProUGUI textBoxText;
    string currentClip;

    public AudioClip fransisco3;
    public AudioClip fransisco4;
    public AudioClip fransisco5;
    public AudioClip fransisco6;
    public AudioClip fransisco7;
    public AudioClip kelly1;

    public static List<string> textList = new List<string>();
    public static List<float> waitTimeList = new List<float>();

    Vector3 targetPosition;
    public float speed = 15.0f;
    float step;

    // Start is called before the first frame update
    /// <summary>
    /// Starts the first textbox text
    /// </summary>
    void Start()
    {
        Invoke("audioFinished", myAudio.clip.length);

        StopText();
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Tutorial")
        {
            textList.Add("Welcome to the game my child, I am Hamster Jesus the one and only.");
            waitTimeList.Add(0);

            textList.Add("I died for all hamsters sins, but that isn't what we're going to talk about today.");
            waitTimeList.Add(5);

            textList.Add("We are here, because you wanted to learn about this epic 4-dimensional game!");
            waitTimeList.Add(6);

            textList.Add("First, let's go over the controls.");
            waitTimeList.Add(5);

            textList.Add("Press W to walk forward,\nA to walk to the left,\nS to walk backwards\nand D to walk to the right.");
            waitTimeList.Add(2.5f);

            textList.Add("Don't forget you can jump with the spacebar.");
            waitTimeList.Add(6);

            textList.Add("When you've pressed all the buttons, you will start your first challenge!");
            waitTimeList.Add(2.5f);
        }
        else if (sceneName == "Maze")
        {
            textList.Add("Hello my child, today we will learn how to move in the 4th dimension.");
            waitTimeList.Add(0);

            textList.Add("As you can see, we are surrounded by walls.");
            waitTimeList.Add(4);

            textList.Add("This is perfect to test out your 4th dimension jumping ability.");
            waitTimeList.Add(2.5f);

            textList.Add("To jump to another slice of this 4-dimensional world, we simply press the \"I\" or the \"J\" button.");
            waitTimeList.Add(4.5f);

            textList.Add("In this particular world, the black walls are 'constant' that exist on all slices of this 4-dimensional space.");
            waitTimeList.Add(5.5f);

            textList.Add("The colored walls do not and only exist in one of them.");
            waitTimeList.Add(6.5f);

            textList.Add("Try jumping back and forth between slices while moving around, and reach the goal. See you there!");
            waitTimeList.Add(4);
        }


        StartCoroutine(TextGiver());
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

    /// <summary>
    /// plays the correct audio clip, when you reach a new checkpoint in the tutorial
    /// and also makes it so that the text changes
    /// </summary>
    public void switchAudio()
    {
        Debug.Log("Next Audio");
        Debug.Log(TutorialManager.currentControlsTutId);
        currentClip = myAudio.clip.name;
        switch (currentClip)
        {
            case "fransisco1":
                myAudio.clip = fransisco3;
                myAudio.Play();

                StopText();
                textList.Add("Make your way up these stairs quickly by sprinting with the shift button.");
                waitTimeList.Add(0);
                StartCoroutine(TextGiver());
                break;
            case "fransisco3":
                myAudio.clip = fransisco4;
                myAudio.Play();

                StopText();
                textList.Add("Jump over this gap by sprinting and jumping at the same time.");
                waitTimeList.Add(0);
                StartCoroutine(TextGiver());
                break;
            case "fransisco4":
                myAudio.clip = fransisco5;
                myAudio.Play();

                StopText();
                textList.Add("You can squeeze through here by crouching.");
                waitTimeList.Add(0);
                StartCoroutine(TextGiver());
                break;
            case "fransisco5":
                myAudio.clip = fransisco6;
                myAudio.Play();

                StopText();
                textList.Add("Look in your inventory by pressing the E button.");
                waitTimeList.Add(0);

                textList.Add("In your inventory you'll find a grappling hook.");
                waitTimeList.Add(2.5f);

                textList.Add("You can move this to your hotbar by clicking and dragging it to your hotbar.");
                waitTimeList.Add(3);

                textList.Add("Equip your hookshot by using the mouse wheel to get to the appropriate hotbar slot.");
                waitTimeList.Add(4.5f);

                textList.Add("And then launch the hook by pressing the left mouse button.");
                waitTimeList.Add(4.5f);
                StartCoroutine(TextGiver());
                break;
            case "fransisco6":
                myAudio.clip = fransisco7;
                myAudio.Play();

                StopText();
                textList.Add("Now, the 4th dimension will start to play a serious aspect.");
                waitTimeList.Add(0);
                textList.Add("Press the I or J button to cycle through slices of reality.");
                waitTimeList.Add(3.5f);
                StartCoroutine(TextGiver());
                break;
            default:
                break;
        }
    }

/// <summary>
/// makes the hamster move to the correct point
/// </summary>
    public void moveTowardsPoint()
    {
        currentClip = myAudio.clip.name;
        switch (currentClip)
        {
            case "fransisco3":
                transform.position = new Vector3(298, 1.91f, 183.86f);
                break;
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

    /// <summary>
    /// Stops all the corountines and clears the textList and waitTimeList
    /// </summary>
    public void StopText()
    {
        StopAllCoroutines();
        textList.Clear();
        waitTimeList.Clear();
    }

    /// <summary>
    /// Waits for x amount of seconds before starting the text coroutine with the given text
    /// </summary>
    IEnumerator TextGiver()
    {
        for (int i = 0; i < textList.Count; i++)
        {
            yield return new WaitForSeconds(waitTimeList[i]);
            StartCoroutine(TextTyping(textList[i]));
        }
    }

    /// <summary>
    /// Types the text
    /// </summary>
    /// <param name="text">Dialogue</param>
    IEnumerator TextTyping(string text)
    {
        for (int i = 0; i <= text.Length; i++)
        {
            yield return new WaitForSeconds(0.02f);
            textBoxText.text = new string(text.Take(i).ToArray());
        }
    }
    
}

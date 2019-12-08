using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image blackScreen;
    public GameObject controlsTutorial;
    public GameObject subtitles;
    public static GameObject currentControlsTutObj;
    public static int currentControlsTutId;
    public bool notComplete = true;

    //The 2 dimensions (switchable with the i and j keys)
    public GameObject dimension1;
    public GameObject dimension2;

    public static List<bool> taskList;

    public GameObject inventoryAndHotbar;
    public GameObject player;
    public float lightRotY = 0;
    public static bool cutsceneLock = false;

    public static Sound scriptInstance;
    public static GameObject Hamster;

    void Start()
    {
        cutsceneLock = true;

        currentControlsTutObj = controlsTutorial.transform.Find("Controls Tutorial 1").gameObject;
        taskList = new List<bool>() { false, false, false, false , false};

        inventoryAndHotbar.SetActive(false);
        subtitles.SetActive(true);
        StartCoroutine(ScreenFade());

        Hamster = GameObject.Find("jesus");
        scriptInstance = Hamster.GetComponent<Sound>();
        
    }

    /// <summary>
    /// Controls tutorials
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.J))
        {
            dimension1.SetActive(!dimension1.activeSelf);
            dimension2.SetActive(!dimension2.activeSelf);
        }


        if (!cutsceneLock && currentControlsTutId == 1 && notComplete) //WASD Move and Space Jump
        {
            if (Input.GetKey(KeyCode.W))
            {
                currentControlsTutObj.transform.Find("W").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                currentControlsTutObj.transform.Find("W").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKey(KeyCode.A))
            {
                currentControlsTutObj.transform.Find("A").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[1] = true;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                currentControlsTutObj.transform.Find("A").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKey(KeyCode.S))
            {
                currentControlsTutObj.transform.Find("S").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[2] = true;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                currentControlsTutObj.transform.Find("S").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKey(KeyCode.D))
            {
                currentControlsTutObj.transform.Find("D").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[3] = true;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                currentControlsTutObj.transform.Find("D").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                currentControlsTutObj.transform.Find("Space").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[4] = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                currentControlsTutObj.transform.Find("Space").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }

            notComplete = false;
            foreach (bool task in taskList) //Check if all tasks are done
            {
                if (!task)
                {
                    notComplete = true;
                    break;
                }
            }

            if (!notComplete) //If all tasks are done
            {
                currentControlsTutObj.SetActive(false);
                MovementController.TeleportPlayer(300, 1, 180); //Move to Area 2
            }
        }

        if (currentControlsTutId == 2) //Shift Sprint
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentControlsTutObj.transform.Find("Shift").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentControlsTutObj.transform.Find("Shift").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }

        if (currentControlsTutId == 3) //Shift Sprint Jump
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentControlsTutObj.transform.Find("Shift").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentControlsTutObj.transform.Find("Shift").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                currentControlsTutObj.transform.Find("Space").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[1] = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                currentControlsTutObj.transform.Find("Space").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }

        if (currentControlsTutId == 4) //Crouch Ctrl
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                currentControlsTutObj.transform.Find("Control").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                currentControlsTutObj.transform.Find("Control").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }

        if (currentControlsTutId == 5) //Inventory E
        {
            if (Input.GetKey(KeyCode.E))
            {
                currentControlsTutObj.transform.Find("E").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                currentControlsTutObj.transform.Find("E").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }

        if (currentControlsTutId == 6) //Change Dimensions I and J
        {
            if (Input.GetKey(KeyCode.I))
            {
                currentControlsTutObj.transform.Find("I").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.I))
            {
                currentControlsTutObj.transform.Find("I").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKey(KeyCode.J))
            {
                currentControlsTutObj.transform.Find("J").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[1] = true;
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                currentControlsTutObj.transform.Find("J").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }
    }

    /// <summary>
    /// Gives the screen a black fade in
    /// </summary>
    IEnumerator ScreenFade()
    {
        byte opacity = 255;
        for (int i = 0; i < 255; i++)
        {
            opacity -= 1;

            blackScreen.color = new Color32(0, 0, 0, opacity);
            yield return new WaitForSeconds(0.01f);
        }
        inventoryAndHotbar.SetActive(true);
        cutsceneLock = false;

        //yield return new WaitForSeconds(15); //Wait for HJ to mention controls    (Disabled for now)
        currentControlsTutId = 1;
        currentControlsTutObj.SetActive(true);
    }

    /// <summary>
    /// Used to continue to the next controls tutorial
    /// </summary>
    /// <param name="amountTasks">The amount of tasks the controls tutorial has</param>
    public static void NextTutorial(int amountTasks)
    {
        currentControlsTutObj.SetActive(false);
        currentControlsTutId += 1;
        scriptInstance.switchAudio();

        if (currentControlsTutId < 7) //If no more control tutorials
        {
            taskList = new List<bool>();
            for (int i = 0; i < amountTasks; i++)
            {
                taskList.Add(false);
            }

            currentControlsTutObj = currentControlsTutObj.transform.parent.Find("Controls Tutorial " + currentControlsTutId).gameObject;
            currentControlsTutObj.SetActive(true);
        }
    }
}

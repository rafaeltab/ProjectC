using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image blackScreen;
    public GameObject controlsTutorial;
    public static GameObject currentControlsTutObj;
    public static int currentControlsTutId = 1;
    public bool notComplete = true;

    public static List<bool> taskList;

    public GameObject inventoryCanvas;
    public GameObject player;
    public float lightRotY = 0;
    public static bool cutsceneLock = true;

    void Start()
    {
        Debug.Log("start");

        currentControlsTutObj = controlsTutorial.transform.Find("Controls Tutorial 1").gameObject;
        taskList = new List<bool>() { false, false, false, false , false};

        inventoryCanvas.SetActive(false);
        StartCoroutine(ScreenFade());
    }

    void Update()
    {
        int rest = 0;
        if (Input.GetKeyDown(KeyCode.B))
        {
            rest = InventoryManager.PickUpItem(ItemDatabase.FetchItemByID(3), 1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            rest = InventoryManager.PickUpItem(ItemDatabase.FetchItemByID(2), 5);
        }
        
        if (rest > 0)
        {
            Debug.Log("Rest " + rest);
        }


        if (!cutsceneLock && currentControlsTutId == 1 && notComplete)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                currentControlsTutObj.transform.Find("W").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                currentControlsTutObj.transform.Find("W").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentControlsTutObj.transform.Find("A").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[1] = true;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                currentControlsTutObj.transform.Find("A").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentControlsTutObj.transform.Find("S").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[2] = true;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                currentControlsTutObj.transform.Find("S").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentControlsTutObj.transform.Find("D").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[3] = true;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                currentControlsTutObj.transform.Find("D").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentControlsTutObj.transform.Find("Space").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                taskList[4] = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                currentControlsTutObj.transform.Find("Space").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }

            notComplete = false;
            foreach (bool task in taskList)
            {
                if (!task)
                {
                    notComplete = true;
                    break;
                }
            }

            if (!notComplete) //Check if completed
            {
                currentControlsTutObj.SetActive(false);
                MovementController.TeleportPlayer(300, 1, 180); //Move to Area 2
            }
        }

        if (currentControlsTutId == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                currentControlsTutObj.transform.Find("Shift").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentControlsTutObj.transform.Find("Shift").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }

        if (currentControlsTutId == 3)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                currentControlsTutObj.transform.Find("Control").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                currentControlsTutObj.transform.Find("Control").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }

        if (currentControlsTutId == 4)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentControlsTutObj.transform.Find("E").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                currentControlsTutObj.transform.Find("E").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }

        if (currentControlsTutId == 5)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                currentControlsTutObj.transform.Find("I").GetComponent<Image>().color = new Color32(170, 170, 170, 255);
                taskList[0] = true;
            }
            else if (Input.GetKeyUp(KeyCode.I))
            {
                currentControlsTutObj.transform.Find("I").GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            if (Input.GetKeyDown(KeyCode.J))
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

    IEnumerator ScreenFade()
    {
        byte opacity = 255;
        for (int i = 0; i < 255; i++)
        {
            opacity -= 1;

            blackScreen.color = new Color32(0, 0, 0, opacity);
            yield return new WaitForSeconds(0.01f);
        }
        inventoryCanvas.SetActive(true);
        cutsceneLock = false;
        currentControlsTutObj.SetActive(true);
    }

    public static void NextTutorial(int amountTasks)
    {
        currentControlsTutObj.SetActive(false);
        currentControlsTutId += 1;

        if (currentControlsTutId < 6) //If no more control tutorials
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

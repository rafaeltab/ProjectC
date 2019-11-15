using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image blackScreen;
    public GameObject inventoryCanvas;
    public byte opaque = 255;
    public static bool cutsceneLock = false;

    void Start()
    {
        Debug.Log("start");
        cutsceneLock = true;
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
    }

    IEnumerator ScreenFade()
    {
        for (int i = 0; i < 255; i++)
        {
            opaque -= 1;

            blackScreen.GetComponent<Image>().color = new Color32(0, 0, 0, opaque);
            yield return new WaitForSeconds(0.01f);
        }
        inventoryCanvas.SetActive(true);
        cutsceneLock = false;
    }

}

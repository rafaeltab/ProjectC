using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public static List<InventoryManager.ItemSlot> hotbarList = new List<InventoryManager.ItemSlot>();

    public GameObject selectHighlight;
    public int selectPos = 0;
    public static InventoryManager.ItemSlot selectedItemSlot;

    public static GameObject obj;

    public static void getHotbarItemSlots(List<InventoryManager.ItemSlot> inventoryList)
    {

        for (int i = 0; i < 5; i++)
        {
            hotbarList.Add(inventoryList[i]);
        }

        selectedItemSlot = hotbarList[0];

    }

    public void Start()
    {
        obj = new GameObject();
        obj.SetActive(false);
    }

    public void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {

            if (Input.mouseScrollDelta.y < 0) //Scroll down
            {
                if (selectPos == 4)
                {
                    selectPos = 0;
                }
                else
                {
                    selectPos += 1;
                }

            }
            else if (Input.mouseScrollDelta.y > 0) //Scroll up
            {
                if (selectPos == 0)
                {
                    selectPos = 4;
                }
                else
                {
                    selectPos -= 1;
                }
                
            }
            
            selectHighlight.transform.position = new Vector3(selectPos * 60 + 25, 25, 0) + InventoryManager.offsetHotbar;
            selectedItemSlot = hotbarList[selectPos];

        }
    }




}

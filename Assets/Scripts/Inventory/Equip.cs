using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static InventoryManager;
using static ItemDatabase;

public class Equip : MonoBehaviour
{
    public static List<InventoryManager.ItemSlot> hotbarList = new List<InventoryManager.ItemSlot>();
    public GameObject selectHighlight;
    public int selectPos = 0;
    public int oldSelectPos = 0;
    public static InventoryManager.ItemSlot selectedItemSlot;

    public static GameObject ItemGameObjects;
    
    public static List<ItemEquip> itemEquips = new List<ItemEquip>();

    /// <summary>
    /// Fixes some switching scene bugs.
    /// </summary>
    public void Awake()
    {
        hotbarList.Clear();
        selectedItemSlot = null;
        ItemGameObjects = null;
        itemEquips.Clear();
    }

    /// <summary>
    /// Get the hotbar slots from inventoryList
    /// </summary>
    /// <param name="inventoryList">The list of the inventory with all the item slots</param>
    public static void GetHotbarItemSlots(List<InventoryManager.ItemSlot> inventoryList)
    {

        for (int i = 4; i >= 0; i--)
        {
            hotbarList.Add(inventoryList[i]);
        }

        selectedItemSlot = hotbarList[0];

    }

    /// <summary>
    /// 
    /// </summary>
    public static void SetupItemEquips()
    {
        ItemGameObjects = GameObject.Find("ItemGameObjects");

        foreach (var item in ItemDatabase.database)
        {
            itemEquips.Add(new ItemEquip(item.id, ItemGameObjects.transform.Find(item.title).gameObject));
        }
    }


    /// <summary>
    /// <para>If the player is scrolling up, equip the hotbar slot to the left</para>
    /// If the player is scrolling down, equip the hotbar slot to the right
    /// </summary>
    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                if (Input.mouseScrollDelta.y < 0) //Scroll down
                {
                    if (selectPos == 4) { selectPos = 0; }
                    else { selectPos += 1; }
                }

                else if (Input.mouseScrollDelta.y > 0) //Scroll up
                {
                    if (selectPos == 0) { selectPos = 4; }
                    else { selectPos -= 1; }

                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectPos = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectPos = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectPos = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selectPos = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selectPos = 4;
            }

            //If new select position is not the same as the old select position
            if (selectPos != oldSelectPos)
            {
                oldSelectPos = selectPos;
                selectHighlight.transform.position = new Vector3(selectPos * 60 + 25, 25, 0) + InventoryManager.offsetHotbar;
                selectedItemSlot = hotbarList[selectPos];
            }
        }

        DoItemEquips();
    }


    public static GameObject oldSelected;
    Item oldItem;
    public void DoItemEquips()
    {
        if (oldItem != selectedItemSlot.item)
        {
            foreach (var item in itemEquips)
            {
                if (selectedItemSlot.item.id == item.itemId)
                {
                    if (oldSelected != null)
                    {
                        oldSelected.SetActive(false);
                    }
                    oldSelected = item.go;
                    oldSelected.SetActive(true);
                    break;
                }
            }

            oldItem = selectedItemSlot.item;
        }
    }
}

public class ItemEquip
{
    public int itemId;
    public GameObject go;

    public ItemEquip(int itemId, GameObject go)
    {
        this.itemId = itemId;
        this.go = go;
    }
}

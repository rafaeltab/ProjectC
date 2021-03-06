﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryManager.ItemSlot itemSlot;

    public bool hovering = false;
    public GameObject itemInfoBox;
    public Vector3 offset;

    public static GameObject grabbedItemObj;
    public static ItemDatabase.Item grabbedItem;
    public static int grabbedAmount;

    public bool addOneDisabled = false;

    /// <summary>
    /// Fixes some switching scene bugs.
    /// </summary>
    public void Awake()
    {
        grabbedItemObj = null;
        grabbedItem = null;
        grabbedAmount = 0;
    }

    /// <summary>
    /// Get the itemInfoBox and grabbedItemObj GameObjects
    /// </summary>
    private void Start()
    {
        itemInfoBox = itemSlot.slotObj.transform.parent.parent.Find("Item Info Box").gameObject;
        grabbedItemObj = itemSlot.slotObj.transform.parent.parent.Find("Grabbed Item").gameObject;
    }


    /// <summary>
    /// If the mouse hovers over the item slot, activate Hover, and if not grabbing anything also show item info
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!PauseMenu2.gamePaused)
        {
            itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(true);
            hovering = true;
            if (itemSlot.amount != 0 && grabbedAmount == 0)
            {
                itemInfoBox.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = itemSlot.item.title;
                itemInfoBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = itemSlot.item.description;
                itemInfoBox.SetActive(true);
            }
        }
    }


    /// <summary>
    /// <para>While hovering, move the item info to the mouse position</para>
    /// <para>Moves the grabbed item to the mouse position if there is one</para>
    /// <para>Picks up item if left mouse button is pressed above item</para>
    /// <para>Picks up half of item if right mouse button is pressed above item</para>
    /// <para>Press right mouse button while grabbing an item to move 1 amount of that item to an item slot</para>
    /// <para>Drop an item by moving the mouse out of the inventory and pressing right mouse button</para>
    /// </summary>
    public void Update()
    {
        if (!PauseMenu2.gamePaused)
        {
            // Move itemInfoBox to the mouse position
            offset = new Vector3(itemInfoBox.GetComponent<RectTransform>().rect.width / 2 + 10, itemInfoBox.GetComponent<RectTransform>().rect.height / -2 + 10, 0);
            itemInfoBox.transform.position = Input.mousePosition + offset;

            // Move the grabbed item to the mouse position
            if (grabbedItem != null)
            {
                grabbedItemObj.transform.position = Input.mousePosition;
            }
        }

        if (hovering)
        {
            if (Input.GetMouseButtonDown(0)) //If left mouse button is down
            {
                if (grabbedAmount == 0 && itemSlot.item.id != 0) //If there is no grabbed item and the clicked item slot is not empty
                {
                    UpdateGrabbedItemObj(itemSlot.item, itemSlot.amount);

                    itemSlot.EmptyItemSlot();

                    grabbedItemObj.SetActive(true);
                    grabbedItemObj.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                else
                {
                    if (itemSlot.item != grabbedItem && itemSlot.item.id != 0)
                    {
                        //Switch items
                        itemSlot.SwitchItems(grabbedItem, grabbedAmount);
                    }

                    else if (grabbedAmount != 0)
                    {
                        //Sum items
                        int rest = itemSlot.SumItems(grabbedItem, grabbedAmount);

                        UpdateGrabbedItemObj(grabbedItem, rest);
                    }

                }

            }
            else if (Input.GetMouseButtonDown(1)) //If right mouse button is down
            {
                if (grabbedAmount == 0 && itemSlot.item.id != 0) //If there is no grabbed item and the clicked item slot is not empty
                {
                    int amount;
                    if (itemSlot.amount % 2 == 0)
                    {
                        amount = itemSlot.amount / 2;
                    }
                    else
                    {
                        amount = itemSlot.amount / 2 + 1;
                    }

                    UpdateGrabbedItemObj(itemSlot.item, amount);

                    itemSlot.amount = itemSlot.amount - amount;
                    itemSlot.UpdateAmount();

                    addOneDisabled = true;

                    grabbedItemObj.SetActive(true);
                    grabbedItemObj.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }

                //For adding 1 to itemSlot by clicking right mouse button
                else if (grabbedAmount != 0 && (itemSlot.item == grabbedItem || itemSlot.item.id == 0))
                {
                    addOneDisabled = true;
                    itemSlot.AddOne();
                }

            }

            if (Input.GetMouseButton(1)) //While right mouse button is down
            {
                //For adding 1 to itemSlot by hovering with right mouse button
                if (grabbedAmount != 0 && !addOneDisabled && (itemSlot.item == grabbedItem || itemSlot.item.id == 0))
                {
                    addOneDisabled = true;
                    itemSlot.AddOne();
                }
            }


            if (grabbedAmount == 0) //Check if there is a grabbed item
            {
                grabbedItemObj.SetActive(false);
                grabbedItemObj.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        //Check if mouse is outside inventory (For dropping items) ## Disabled for now ##
        //if (!CheckInventoryHover.hoverInventory)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        UpdateGrabbedItemObj(ItemDatabase.FetchItemByID(0), 0);
        //        grabbedItemObj.SetActive(false);
        //        grabbedItemObj.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //    }
        //}
    }

    /// <summary>
    /// Update the grabbedItemObj and values grabbedItem and grabbedAmount
    /// </summary>
    /// <param name="item">The grabbedItem</param>
    /// <param name="amount">The amount of the grabbed item</param>
    public static void UpdateGrabbedItemObj(ItemDatabase.Item item, int amount)
    {
        grabbedItem = item;
        grabbedAmount = amount;
        grabbedItemObj.transform.GetComponent<Image>().sprite = grabbedItem.sprite;
        grabbedItemObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = grabbedAmount.ToString();
    }


    /// <summary>
    /// If the mouse stops hovering, disable Hover, and if not grabbing anything, disable item info
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        addOneDisabled = false;
        itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(false);

        if (grabbedAmount == 0) { itemInfoBox.SetActive(false); }
    }

    /// <summary>
    /// If the inventory is disabled, disable Hover
    /// </summary>
    public void OnDisable()
    {
        hovering = false;
        addOneDisabled = false;
        if (itemSlot != null)
        {
            itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(false);
            if (grabbedAmount == 0) { itemInfoBox.SetActive(false); }
        }
        
    }
}

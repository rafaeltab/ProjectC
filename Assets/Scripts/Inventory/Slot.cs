using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryManager.ItemSlot itemSlot;

    public void OnDrop(PointerEventData eventData)
    {

        ItemDrag droppedItem = eventData.pointerDrag.GetComponent<ItemDrag>();


        if (itemSlot.slotID != droppedItem.itemSlot.slotID)
        {
            if (itemSlot.item == droppedItem.itemSlot.item)
            {
                //original slot + new slot = new amount in new slot, remove item in original slot (add item stack limit later)
                droppedItem.itemSlot.sumItems(itemSlot);
            }

            else if (droppedItem.itemSlot.item.id != 0)
            {
                //Switch items from slots
                droppedItem.itemSlot.switchItems(itemSlot);
            }
        }

    }

    //Hover over item slot
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter");
        itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        Debug.Log("Call Disable");
        itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(false);
    }
}

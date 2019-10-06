using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public InventoryManager.ItemSlot itemSlot;

    public void OnDrop(PointerEventData eventData)
    {
        ItemDrag droppedItem = eventData.pointerDrag.GetComponent<ItemDrag>();

        if (itemSlot.slotID != droppedItem.itemSlot.slotID)
        {
            if (itemSlot.amount == 0)
            {
                //Replace item with new slot, and remove item from original slot
            }
            else if (itemSlot.item == droppedItem.itemSlot.item)
            {
                //original slot + new slot = new amount in new slot, remove item in original slot (add item stack limit later)
            }
            else if (itemSlot.item != droppedItem.itemSlot.item)
            {
                //Switch items from slots
            }
        }

    }
}

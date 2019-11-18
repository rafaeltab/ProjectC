using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    public int itemId;
    public int amount;
    public bool specificItemSlot;
    public int itemSlotIndex;
    private bool done = false;

    //Give player item if touching collider
    private void OnTriggerEnter(Collider other)
    {
        if (!done && other.gameObject.name == "Player")
        {
            if (specificItemSlot) //Moves item to the given itemslot (Overwrites current item in the itemslot)
            {
                InventoryManager.FetchItemSlotByID(itemSlotIndex).InitiateItem(ItemDatabase.FetchItemByID(itemId), amount);
            }
            else //Gives the player the item
            {
                InventoryManager.PickUpItem(ItemDatabase.FetchItemByID(itemId), amount);
                done = true;
            }
        }
    }
}
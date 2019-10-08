using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryManager.ItemSlot itemSlot;

    public bool hovering = false;
    public GameObject itemInfoBox;
    public Vector3 offset;

    public void OnDrop(PointerEventData eventData)
    {

        ItemDrag droppedItem = eventData.pointerDrag.GetComponent<ItemDrag>();


        if (itemSlot.slotID != droppedItem.itemSlot.slotID && !droppedItem.disabled)
        {
            if (itemSlot.item == droppedItem.itemSlot.item)
            {
                //original slot + new slot = new amount in new slot, remove item in original slot.
                //If new amount is more than stackLimit, new slot amount = stackLimit, original slot = rest
                droppedItem.itemSlot.sumItems(itemSlot);
            }

            else if (droppedItem.itemSlot.item.id != 0)
            {
                //Switch items from item slots
                droppedItem.itemSlot.switchItems(itemSlot);
            }
        }

    }

    private void Start()
    {
        itemInfoBox = itemSlot.slotObj.transform.parent.parent.GetChild(1).gameObject;
    }


    //Hover over item slot
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(true);
        hovering = true;
        if (itemSlot.amount != 0)
        {
            itemInfoBox.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = itemSlot.item.title;
            itemInfoBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = itemSlot.item.description;
            itemInfoBox.SetActive(true);
        }
    }

    public void Update()
    {
        if (hovering)
        {
            offset = new Vector3(itemInfoBox.GetComponent<RectTransform>().rect.width / 2 + 10, itemInfoBox.GetComponent<RectTransform>().rect.height / -2 + 10, 0);
            itemInfoBox.transform.position = Input.mousePosition + offset;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        hovering = false;
        itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(false);
        itemInfoBox.SetActive(false);
    }

    public void OnDisable()
    {
        hovering = false;
        if (itemSlot != null)
        {
            Debug.Log("Disable");
            itemSlot.slotObj.transform.Find("Hover").gameObject.SetActive(false);
            itemInfoBox.SetActive(false);
        }
        
    }
}

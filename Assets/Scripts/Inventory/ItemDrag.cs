using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryManager.ItemSlot itemSlot;

    public Transform originalParent;

    public bool dragging = false;

    public bool disabled = false;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot.amount != 0)
        {
            Debug.Log("Begin dragging item");
            disabled = false;
            dragging = true;
            this.originalParent = this.transform.parent;
            this.transform.parent.transform.SetAsLastSibling();
            this.transform.SetAsLastSibling();
            this.transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot.amount != 0 && dragging)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endDrag();
    }

    public void OnDisable()
    {
        if (dragging) { disabled = true; endDrag(); }
    }

    public void endDrag()
    {
        Debug.Log("End dragging item");
        dragging = false;
        this.transform.SetAsFirstSibling();
        this.transform.position = this.originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (!CheckInventoryHover.hoverInventory && !disabled)
        {
            InventoryManager.dropItem(itemSlot);
        }
    }
}

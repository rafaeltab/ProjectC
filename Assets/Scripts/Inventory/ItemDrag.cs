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

    /// <summary>
    /// If item gets dragged
    /// </summary>
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

    /// <summary>
    /// While item gets dragged, mouse the item to the mouse position
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot.amount != 0 && dragging)
        {
            this.transform.position = eventData.position;
        }
    }

    /// <summary>
    /// If items stops getting dragged
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        endDrag();
    }

    /// <summary>
    /// If the inventory gets disabled
    /// </summary>
    public void OnDisable()
    {
        if (dragging) { disabled = true; endDrag(); }
    }

    /// <summary>
    /// Ends the dragging and drop the item if it's dragged outside the inventory
    /// </summary>
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

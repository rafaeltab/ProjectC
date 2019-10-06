using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryManager.ItemSlot itemSlot;

    public Transform originalParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot.amount != 0)
        {
            this.originalParent = this.transform.parent;
            this.transform.parent.transform.SetAsLastSibling();
            this.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot.amount != 0)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = this.originalParent.position;
    }
}

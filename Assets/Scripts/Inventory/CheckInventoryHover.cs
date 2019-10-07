using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckInventoryHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool hoverInventory = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverInventory = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverInventory = false;
    }

    public void OnDisable()
    {
        hoverInventory = false;
    }
}

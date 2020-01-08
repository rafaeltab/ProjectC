using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckInventoryHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool hoverInventory = false;

    /// <summary>
    /// Fixes some switching scene bugs.
    /// </summary>
    public void Awake()
    {
        hoverInventory = false;
    }

    /// <summary>
    /// Checks if mouse is in the inventory
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverInventory = true;
    }

    /// <summary>
    /// Checks if the mouse is outside the inventory
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverInventory = false;
    }

    /// <summary>
    /// Checks if the inventory is disabled
    /// </summary>
    public void OnDisable()
    {
        hoverInventory = false;
    }
}

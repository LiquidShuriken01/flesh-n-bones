using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /* TODO: World-to-Inventory item movement */
        GameObject item = eventData.pointerDrag;
        DraggableItem draggableItem = item.GetComponent<DraggableItem>();
        if (transform.childCount == 0)
        {
            draggableItem.endParent = transform;
        }
        else
        {
            // Swap item
            transform.GetChild(0).SetParent(draggableItem.endParent);
            draggableItem.endParent = transform;
        }
    }

    /* TODO: Create an adjacency checking function, probably using GameObject reference variables.
     * Make another function for calculating stat bonuses from item held and its adjacencies.
     * Report total bonus to a character stats Scriptable Object.
     */
}

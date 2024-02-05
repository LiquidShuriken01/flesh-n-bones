using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler, IEndDragHandler
{
    public bool has_item = false;
    int slot_index = -1;

    void Start()
    {
        has_item = (transform.childCount > 0);
        slot_index = transform.GetSiblingIndex();
    }

    public void OnDrop(PointerEventData eventData)
    {
        /* TODO: World-to-Inventory item movement */
        GameObject item = eventData.pointerDrag;
        DraggableItem draggableItem = item.GetComponent<DraggableItem>();
        if (!has_item)
        {
            draggableItem.endParent = transform;
            has_item= true;

            UpdateSummary();
        }
        else
        {
            // Swap item
            transform.GetChild(0).SetParent(draggableItem.endParent);
            draggableItem.endParent.GetComponent<ItemSlot>().UpdateSummary();
            draggableItem.endParent = transform;
            UpdateSummary();
        }
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        if (has_item && transform.childCount == 0)
        {
            has_item = false;

            GameObject item = eventData.pointerDrag;
            GameObject summary_block = transform.parent.GetChild(transform.parent.childCount).gameObject;
            DraggableItem draggableItem = item.GetComponent<DraggableItem>();
            DataManager dm = DataManager._instance;
            summary_block.GetComponent<SummaryTextBlock>().RemoveEntry(draggableItem.this_organ.name);
        }
    }

    /* TODO: Create an adjacency checking function, probably using GameObject reference variables.
     * Make another function for calculating stat bonuses from item held and its adjacencies.
     * Report total bonus to a character stats Scriptable Object.
     */
    public void UpdateSummary()
    {
        GameObject summary_block = transform.parent.GetChild(transform.parent.childCount).gameObject;
        DataManager dm = DataManager._instance;
        DraggableItem item = transform.GetChild(0).GetComponent<DraggableItem>();
        string summary = dm.organ_list[(int)item.item_id].name + ":\n" + item.this_organ.ModString();
        summary_block.GetComponent<SummaryTextBlock>().AddEntry(item.this_organ.name, summary);
    }

    public void ApplyModifiers()
    {
        CharacterInfo player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInfo>();
        DraggableItem item = transform.GetChild(0).GetComponent<DraggableItem>();
        item.this_organ.ApplyModifiers(player);
    }
}

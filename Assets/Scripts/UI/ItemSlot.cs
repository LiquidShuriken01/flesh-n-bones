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
        if(has_item)
        {
            UpdateSummary(transform.GetChild(0).GetComponent<DraggableItem>());
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        /* TODO: World-to-Inventory item movement */
        GameObject item = eventData.pointerDrag;
        DraggableItem draggableItem = item.GetComponent<DraggableItem>();
        if(!has_item)
        {
            draggableItem.endParent = transform;
            has_item= true;

            UpdateSummary(draggableItem);
        }
        else
        {
            // Swap item
            DraggableItem prev_item = transform.GetChild(0).GetComponent<DraggableItem>();
            transform.GetChild(0).SetParent(draggableItem.endParent);
            draggableItem.endParent = transform;
            UpdateSummary(prev_item);
            UpdateSummary(draggableItem);
        }
    }


    public void OnEndDrag(PointerEventData eventData) 
    {
        Debug.Log("End drag. Checking organ status...");

        if (has_item && transform.childCount == 0)
        {
            Debug.Log("Organ lost...");
            has_item = false;

            GameObject item = eventData.pointerDrag;
            GameObject summary_block = transform.parent.GetChild(transform.parent.childCount-1).gameObject;
            DraggableItem draggableItem = item.GetComponent<DraggableItem>();
            DataManager dm = DataManager._instance;
            summary_block.GetComponent<SummaryTextBlock>().RemoveEntry(draggableItem.this_organ.name);
        }
    }

    /* TODO: Create an adjacency checking function, probably using GameObject reference variables.
     * Make another function for calculating stat bonuses from item held and its adjacencies.
     * Report total bonus to a character stats Scriptable Object.
     */
    public void UpdateSummary(DraggableItem item)
    {
        GameObject summary_block = transform.parent.GetChild(transform.parent.childCount-1).gameObject;
        DataManager dm = DataManager._instance;
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

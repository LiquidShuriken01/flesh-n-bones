using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public bool has_item = false;
    int slot_index = -1;
    public string current_organ_name = string.Empty;

    void Start()
    {
        has_item = (transform.childCount > 0);
        slot_index = transform.GetSiblingIndex();
        if (has_item)
        {
            DraggableItem draggableItem = transform.GetChild(0).GetComponent<DraggableItem>();
            UpdateSummary(draggableItem);
        }
    }

    void Update()
    {
        if (has_item && transform.childCount == 0)
        {
            Debug.Log("Organ lost...");
            has_item = false;

            SummaryTextBlock summary_block = transform.parent.GetChild(transform.parent.childCount - 1).gameObject.GetComponent<SummaryTextBlock>();
            summary_block.RemoveEntry(current_organ_name);
            current_organ_name = string.Empty;
        }
        else if (!has_item && transform.childCount > 0)
        {
            has_item = true;
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
            has_item = true;

            UpdateSummary(draggableItem);
        }
        else
        {
            // Swap items
            DraggableItem prev_item = transform.GetChild(0).GetComponent<DraggableItem>();
            transform.GetChild(0).SetParent(draggableItem.endParent);
            draggableItem.endParent.gameObject.GetComponent<ItemSlot>().UpdateSummary(prev_item);
            draggableItem.endParent = transform;
            
            UpdateSummary(draggableItem);
        }
    }

    /* TODO: Create an adjacency checking function, probably using GameObject reference variables.
     * Make another function for calculating stat bonuses from item held and its adjacencies.
     * Report total bonus to a character stats Scriptable Object.
     */
    public void UpdateSummary(DraggableItem item)
    {
        current_organ_name = item.this_organ.name;
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

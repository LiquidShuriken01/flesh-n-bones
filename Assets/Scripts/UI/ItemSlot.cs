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
    SummaryTextBlock summaryBlock;

    void Start()
    {
        has_item = (transform.childCount > 0);
        slot_index = transform.GetSiblingIndex();
        summaryBlock = transform.parent.GetChild(transform.parent.childCount - 1).gameObject.GetComponent<SummaryTextBlock>();
        if (has_item)
        {
            DraggableItem draggableItem = transform.GetChild(0).GetComponent<DraggableItem>();
            UpdateSummary(draggableItem);
            ApplyOrganModifiers(draggableItem);
        }
    }

    void Update()
    {
        if (has_item && transform.childCount == 0)
        {
            Debug.Log("Organ missing...");
            has_item = false;
            RemoveOrganModifiers();
            summaryBlock.RemoveEntry(current_organ_name);
            
            current_organ_name = string.Empty;
        }
        else if (!has_item && transform.childCount > 0) { has_item = true; }
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
            ApplyOrganModifiers(draggableItem);
        }
        else
        {
            // Swap items
            DraggableItem prevItem = transform.GetChild(0).GetComponent<DraggableItem>();
            transform.GetChild(0).SetParent(draggableItem.endParent);
            ItemSlot otherSlot = draggableItem.endParent.gameObject.GetComponent<ItemSlot>();
            otherSlot.UpdateSummary(prevItem);
            otherSlot.ApplyOrganModifiers(prevItem);
            draggableItem.endParent = transform;
            UpdateSummary(draggableItem);
            ApplyOrganModifiers(draggableItem);
        }
    }

    
    public void UpdateSummary(DraggableItem item)
    {
        current_organ_name = item.this_organ.name;
        DataManager dm = DataManager._instance;
        string summary = dm.organ_list[(int)item.item_id].name + ":\n" + item.this_organ.ModString();
        summaryBlock.AddEntry(item.this_organ.name, summary);
    }

    public void ApplyOrganModifiers(DraggableItem item)
    {
        CharacterInfo player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataHandler>().player_info;

        /* TODO: Create an adjacency checking function, probably using GameObject reference variables.
         * Make another function for calculating stat bonuses from item held and its adjacencies.
         * Report total bonus to a character stats Scriptable Object.
         */

        item.this_organ.ApplyModifiers(player); // Apply only the base modifiers for now
    }

    public void RemoveOrganModifiers()
    {
        CharacterInfo player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataHandler>().player_info;
        player.RemoveBuff(current_organ_name);
    }
}

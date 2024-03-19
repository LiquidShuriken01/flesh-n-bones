using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform endParent;
    public GameObject InWorldItem;
    public uint item_id = 0;
    public int organ_session_id = -1;
    public Organ this_organ = null;

    public bool in_invt = true;

    void Awake()
    {
        DataManager dm = DataManager._instance;
        this_organ = new Organ(dm.organ_list[(int)item_id]);
        organ_session_id = ++dm.organs_in_session;
        this_organ.name += organ_session_id.ToSafeString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        endParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(endParent);
        gameObject.GetComponent<Image>().raycastTarget = true;

        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 spawnPos = playerTransform.position + .75f * playerTransform.forward;
            var worldItem = Instantiate(InWorldItem, spawnPos, playerTransform.rotation);
            worldItem.GetComponent<WorldItem>().item_id = this.item_id;
            Destroy(gameObject);
            return;
        }

        ItemSlot slot = endParent.gameObject.GetComponent<ItemSlot>();
        if (slot != null)
        {
            if (!slot.has_item)
            {
                slot.has_item = true;
                slot.UpdateSummary(this);
            }
        }
    }
}

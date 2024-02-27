using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public GameObject Container;
    public uint item_id = 0;

    private GameObject _container;
    private GameObject _canvas;

    void Start()
    {
        _canvas = GameObject.FindGameObjectWithTag("UICanvas");
    }

    public void ShowContainer()
    {
        _container = Instantiate(Container);
        _container.transform.SetParent(_canvas.transform, false);
        _container.GetComponent<RectTransform>().anchoredPosition = new Vector2(40f, -40f);
        var item = _container.transform.GetChild(0).GetChild(0).GetComponent<DraggableItem>();
        item.item_id = this.item_id;
    }
}

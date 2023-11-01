using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    GameObject inventory;
    GameObject reticle;
    GameObject pause_text;
    bool inv_is_active = false;
    bool show_reticle = true;
    bool paused = false;

    void Start()
    {
        inventory = transform.GetChild(0).gameObject;
        reticle = transform.GetChild(2).gameObject;
        pause_text = transform.GetChild(3).gameObject;
        inventory.SetActive(inv_is_active);
        reticle.SetActive(show_reticle);
        pause_text.SetActive(paused);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            show_reticle = inv_is_active;
            inv_is_active = !inv_is_active;
            inventory.SetActive(inv_is_active);
            reticle.SetActive(show_reticle);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            show_reticle = paused;
            paused = !paused;
            pause_text.SetActive(paused);
            reticle.SetActive(show_reticle);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    GameObject inventory;
    GameObject health_bar;
    GameObject reticle;
    GameObject pause_text;
    PlayerInfo player_data;
    Slider hp_slider;
    bool inv_is_active = false;
    bool show_reticle = true;
    bool paused = false;

    void Start()
    {
        inventory = transform.GetChild(0).gameObject;
        health_bar = transform.GetChild(1).gameObject;
        reticle = transform.GetChild(2).gameObject;
        pause_text = transform.GetChild(3).gameObject;
        hp_slider = health_bar.GetComponent<Slider>();
        inventory.SetActive(inv_is_active);
        reticle.SetActive(show_reticle);
        pause_text.SetActive(paused);

        player_data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataHandler>().player_info;
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

        hp_slider.maxValue = player_data.max_health;
        hp_slider.value = player_data.health;
    }
}

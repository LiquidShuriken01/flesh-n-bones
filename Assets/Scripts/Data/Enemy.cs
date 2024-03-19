using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public CharacterInfo template;
    [System.NonSerialized]
    public CharacterInfo character_info;
    [System.NonSerialized]
    public bool dead;
    public GameObject health_bar;

    private Slider hp_slider;
    private bool loot_dropped;
    [SerializeField] private int bones = 0;

    private void Awake()
    {
        character_info = Instantiate(template);
        character_info.gm = GameMaster._instance;
        hp_slider = health_bar.GetComponent<Slider>();
        loot_dropped = false;
    }

    private void Update()
    {
        dead = character_info.dead;
        if (dead)
        {
            health_bar.SetActive(false);
            if (!loot_dropped)
            {
                GameMaster._instance.GiveBones(bones);
                loot_dropped = true;
            }
        }

        hp_slider.maxValue = character_info.max_health;
        hp_slider.value = character_info.health;
    }
}

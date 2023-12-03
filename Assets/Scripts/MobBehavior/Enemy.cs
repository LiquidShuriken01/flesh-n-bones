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

    private void Awake()
    {
        character_info = Instantiate(template);
        character_info.gm = GameObject.FindWithTag("Ruleset").GetComponent<GameMaster>();
        GetComponent<LungfishBehavior>().character_info = this.character_info;
        hp_slider = health_bar.GetComponent<Slider>();
    }

    private void Update()
    {
        dead = character_info.dead;

        hp_slider.maxValue = character_info.max_health;
        hp_slider.value = character_info.health;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
    private List<int> organ_drop = new List<int>();

    private void Awake()
    {
        character_info = Instantiate(template);
        character_info.gm = GameMaster._instance;
        hp_slider = health_bar.GetComponent<Slider>();
        loot_dropped = false;
        foreach (int i in character_info.organ_drop)
        {
            organ_drop.Add(i);
        }
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
                Debug.Log("Trying to load Item Prefab from file...");
                GameObject worldItem = (GameObject)Resources.Load("Itemization/World Item");
                if (worldItem == null)
                {
                    throw new FileNotFoundException("...no file found - please check the configuration");
                }
                foreach (int i in organ_drop)
                {
                    var randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    var newItem = Instantiate(worldItem, transform.position,randomRotation);
                    WorldItem witm = newItem.GetComponent<WorldItem>();
                    Rigidbody irb = newItem.GetComponent<Rigidbody>();
                    irb.AddForce(newItem.transform.right * 0.1f);
                }
            }
        }

        hp_slider.maxValue = character_info.max_health;
        hp_slider.value = character_info.health;
    }
}

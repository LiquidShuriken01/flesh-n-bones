using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerInfo : CharacterInfo
{
    void OnEnable()
    {
        this.char_name = "Player";
        this.health = this.max_health = this.GetStatValue("max_health");
        this.nerve = this.max_nerve = this.GetStatValue("max_nerve");
    }

    public void Interact(GameObject target)
    {
        if (target.CompareTag("Enemy"))
        {
            CharacterInfo enemy_info = target.GetComponent<Enemy>().character_info;
            this.gm.AttackRoll(enemy_info, this.char_name, this.GetStatValueInt("base_atk_bonus"), 10, AtkType.Carapace);
        }
        else if (target.CompareTag("Interactable"))
        {
            Debug.Log("Not yet implemented");
        }
    }

    //void Update() {
        //Debug.Log(this.GetStatValue("Health"));
        //if (this.GetStatValue("Health") == 0):
     //}
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (target.tag == "Enemy")
        {
            this.Attack(target, this.GetStatValueInt("base_atk_bonus"), 10, AtkType.Carapace);
        }
    }
}

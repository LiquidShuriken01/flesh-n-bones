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
    }

    public void Attack(GameObject target)
    {
        if (target.tag == "Enemy")
        {
            CharacterInfo enemy = target.GetComponent<Enemy>().character_info;
            enemy.TakeDamage(10);
        }
    }
}

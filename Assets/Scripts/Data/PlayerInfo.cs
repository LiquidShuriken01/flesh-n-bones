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
        this.name = "Player";
    }

    public void Attack(GameObject target)
    {
        if (target.tag == "Enemy")
        {
            //target.GetComponent<>
        }
    }
}

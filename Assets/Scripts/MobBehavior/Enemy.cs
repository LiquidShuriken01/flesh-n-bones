using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CharacterInfo template;
    [System.NonSerialized]
    public CharacterInfo character_info;
    [System.NonSerialized]
    bool moving;
    [System.NonSerialized]
    bool dead;
    public Animator animator;

    private Pathing pathing_ai;

    private void Awake()
    {
        character_info = Instantiate(template);
        pathing_ai = gameObject.GetComponent<Pathing>();
    }

    private void Update()
    {
        moving = pathing_ai.is_moving;
        // Set animator bool "moving" to moving
        animator.SetBool("isMoving", moving);
        //Debug.Log(moving);
        dead = character_info.dead;
        animator.SetBool("isDead", dead);
        //Debug.Log(dead);
        // Set animator bool "dead" to dead
    }
}

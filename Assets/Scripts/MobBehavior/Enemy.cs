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
        dead = character_info.dead;
    }
}

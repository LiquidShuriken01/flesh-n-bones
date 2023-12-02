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
    public Animator animator;

    private Pathing pathing_ai;
    private Slider hp_slider;
    private bool moving;

    private void Awake()
    {
        character_info = Instantiate(template);
        character_info.gm = GameObject.FindWithTag("Ruleset").GetComponent<GameMaster>();
        pathing_ai = gameObject.GetComponent<Pathing>();
        hp_slider = health_bar.GetComponent<Slider>();
    }

    private void Update()
    {
        moving = pathing_ai.is_moving;
        animator.SetBool("isMoving", moving);
        dead = character_info.dead;
        if (dead) { animator.SetTrigger("isDead"); }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fish_Attack") && pathing_ai.can_attack)
        {
            pathing_ai.attacking = true;
            animator.SetTrigger("attack");
        }
        else
        {
            pathing_ai.attacking = false;
        }

        hp_slider.maxValue = character_info.max_health;
        hp_slider.value = character_info.health;

    }

    public void ShootPhlegm()
    {
        Instantiate(template);
    }
}

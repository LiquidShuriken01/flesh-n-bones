using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungfishBehavior : MonoBehaviour
{
    public Animator animator;
    public GameObject phlegm_glob;
    [System.NonSerialized] public CharacterInfo character_info;

    private Pathing pathing_ai;

    private void Start()
    {
        pathing_ai = gameObject.GetComponent<Pathing>();
    }

    private void Update()
    {
        animator.SetBool("isMoving", pathing_ai.is_moving);
        if (character_info.dead) { animator.SetTrigger("isDead"); }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fish_Attack") && pathing_ai.can_attack)
        {
            pathing_ai.attacking = true;
            animator.SetTrigger("attack");
        }
        else
        {
            pathing_ai.attacking = false;
        }
    }

    // Attacks & Skills
    public void ShootPhlegm()
    {
        var glob = Instantiate(phlegm_glob);
        glob.transform.position = this.transform.position - 0.2f * this.transform.forward;
        var proj_ai = glob.GetComponent<Projectile>();
        proj_ai.accuracy = character_info.GetStatValueInt("base_atk_bonus");
        proj_ai.damage = 7.5f;
        proj_ai.atk_type = AtkType.Mucus;
    }
}

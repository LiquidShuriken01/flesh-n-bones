using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBehavior : MonoBehaviour
{
    public Animator animator;
    public LayerMask layersToHit;

    private CharacterInfo character_info;
    private Pathing pathing_ai;
    private GameMaster gm;
    

    private void Start()
    {
        pathing_ai = gameObject.GetComponent<Pathing>();
        character_info = gameObject.GetComponent<Enemy>().character_info;
        gm = GameMaster._instance;
    }

    private void Update()
    {
        animator.SetBool("isMoving", pathing_ai.is_moving);
        if (character_info.dead) 
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("isDead");
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Worker_Attack") && pathing_ai.can_attack)
        {
            pathing_ai.attacking = true;
            animator.SetTrigger("attack");
        }
        else
        {
            animator.ResetTrigger("attack");
            pathing_ai.attacking = false;
        }
    }

    public void Slice()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - 0.5f * transform.forward, 1.25f, layersToHit);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                CharacterInfo target_info = collider.gameObject.GetComponent<FPSControl>().player_info;
                if (target_info != null)
                {
                    int accuracy = character_info.GetStatValueInt("base_atk_bonus");
                    float damage = 12f;
                    gm.AttackRoll(target_info, character_info.char_name, accuracy, damage, AtkType.Carapace);
                    break;
                }
            }
        }
    }

    public void Hew()
    {

    }
}

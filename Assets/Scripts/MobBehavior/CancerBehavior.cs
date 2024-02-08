using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancerBehavior : MonoBehaviour
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
        if (character_info.dead) { animator.SetTrigger("isDead"); }
        if (pathing_ai.can_attack) { Boom(); }
    }

    public void Boom()
    {
        character_info.dead = true;
        pathing_ai.can_attack = false;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f, layersToHit);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                CharacterInfo target_info = collider.gameObject.GetComponent<PlayerDataHandler>().player_info;
                if (target_info != null)
                {
                    int accuracy = -10;
                    float damage = 15f;
                    gm.AttackRoll(target_info, character_info.char_name, accuracy, damage, AtkType.Mucus);
                    break;
                }
            }
        }
        
    }

}

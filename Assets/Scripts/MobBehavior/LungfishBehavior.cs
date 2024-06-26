using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LungfishBehavior : MonoBehaviour
{
    public Animator animator;
    public GameObject phlegm_glob;
    
    private CharacterInfo character_info;
    private Pathing pathing_ai;

    private void Start()
    {
        pathing_ai = gameObject.GetComponent<Pathing>();
        character_info = gameObject.GetComponent<Enemy>().character_info;
    }

    private void Update()
    {
        animator.SetBool("isMoving", pathing_ai.is_moving);
        if (character_info.dead)
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("isDead");
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fish_Attack") && pathing_ai.can_attack)
        {
            animator.SetTrigger("attack");
        }
        else
        {
            animator.ResetTrigger("attack");
        }

        pathing_ai.attacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Fish_Attack");
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

    public void CreateAcidPool()
    {
        Debug.Log("Trying to load Acid Pool Pellet Prefab from file...");
        GameObject pellet = (GameObject)Resources.Load("Projectiles/Acid Pool Pellet");
        if (pellet == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }

        var launchPos = this.transform.position - 0.4f * this.transform.forward;
        var launchRot = Quaternion.Euler(0, this.transform.rotation.y, 0);

        var newProj0 = Instantiate(pellet, launchPos, Quaternion.Euler(0, this.transform.rotation.y-60f, 0));
        var newProj1 = Instantiate(pellet, launchPos, Quaternion.Euler(0, this.transform.rotation.y, 0));
        var newProj2 = Instantiate(pellet, launchPos, Quaternion.Euler(0, this.transform.rotation.y+60f, 0));
        Rigidbody prb0 = newProj0.GetComponent<Rigidbody>();
        Rigidbody prb1 = newProj1.GetComponent<Rigidbody>();
        Rigidbody prb2 = newProj2.GetComponent<Rigidbody>();
        prb0.velocity = newProj0.transform.forward * 0.25f + new Vector3(0, 0.25f, 0);
        prb1.velocity = newProj1.transform.forward * 0.25f + new Vector3(0, 0.25f, 0);
        prb2.velocity = newProj2.transform.forward * 0.25f + new Vector3(0, 0.25f, 0);
    }
}

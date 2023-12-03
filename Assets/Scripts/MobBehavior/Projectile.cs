using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public string origin_name;
    public int accuracy = 0;
    public float damage;
    public AtkType atk_type;

    private GameMaster gm;
    private Rigidbody rb;
    [SerializeField] private bool homing;
    [SerializeField] private float speed;
    [SerializeField] private float rotation_speed;
    [SerializeField] private float life_span = 10f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        gm = GameObject.FindGameObjectWithTag("Ruleset").GetComponent<GameMaster>();
        rb = GetComponent<Rigidbody>();

        Vector3 dir = target.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = rot;
    }

    private void FixedUpdate()
    {
        if (life_span <= 0) { Destroy(gameObject); }

        if (homing)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rot, rotation_speed * Time.deltaTime));
        }

        rb.velocity = transform.forward * speed;

        life_span -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            CharacterInfo target_info = other.GetComponent<FPSControl>().player_info;
            if (target_info != null)
            {
                gm.AttackRoll(target_info, origin_name, accuracy, damage, atk_type);
            }
        }
        Destroy(gameObject);
    }
}

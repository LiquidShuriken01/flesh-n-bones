using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathing : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float detectDist = 7.5f;
    [SerializeField] private float pursueDist = 5.0f;
    [SerializeField] private float evadeDist = 0f;
    
    [NonSerialized] public bool is_moving;
    [NonSerialized] public bool can_attack;
    [NonSerialized] public bool attacking;

    private bool pursue;
    private float distance;

    private CharacterInfo character_info;

    private NavMeshAgent agent;
    private List<Vector3> path = new List<Vector3>();

    //[SerializeField] Transform waypoints;
    //public LayerMask layersToHit;

    // Start is called before the first frame update
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        path.Clear();
        pursue = true;
        /*if (waypoints.childCount > 0)
        {
            foreach (Transform waypoint in waypoints)
            {
                path.Add(waypoint.position);
            }
        }
        */
    }

    private void Start()
    {
        character_info = gameObject.GetComponent<Enemy>().character_info;

        pursue = true;
        is_moving = false;
        can_attack = false;
        attacking = false;
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 startPosition = waypoints.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
    }*/

    void Seek(Vector3 location)
    {
        agent.isStopped = false;
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        agent.isStopped = false;
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector.normalized);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = 0.5f;
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = 0.5f;
        Flee(target.transform.position + targetDir * lookAhead);
    }


    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        agent.isStopped = false;
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        UnityEngine.Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    /*void FollowPath(List<Vector3> path)
    {
        int index = 0;
        int closest = 0;
        float minDist = 999.9f;
        foreach (Vector3 waypoint in path)
        {
            Vector3 dir = waypoint - this.transform.position;
            float dist = dir.magnitude;
            if (minDist > dist)
            {
                minDist = dist;
                closest = index;
            }
            index++;
        }

        if (closest + 1 < path.Count)
        {
            Seek(path[closest + 1]);
        }
        else
        {
            Seek(path[0]);
        }
            
    }*/

    private void Stop()
    {
        agent.isStopped = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!character_info.dead && !attacking)
        {
            distance = (this.transform.position - target.transform.position).magnitude;
            if (distance > pursueDist) { pursue = true; }
            if (distance <= evadeDist) { pursue = false; }
            if (pursue)
            {
                if (distance <= detectDist && distance > pursueDist + 0.5f)
                {
                    Pursue();
                }
                //else { Stop(); }
            }
            else
            {
                if (distance < evadeDist - 0.5f)
                {
                    Evade();
                }
                else { Stop(); }
            }

            can_attack = (distance <= pursueDist) && (distance > evadeDist);
        }
        else { can_attack = false; }

        is_moving = (agent.velocity.magnitude > 0.01f);
    }
}

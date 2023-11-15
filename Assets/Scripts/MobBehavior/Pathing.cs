using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathing : MonoBehaviour
{
    [NonSerialized]
    public CharacterInfo character_info;

    private NavMeshAgent agent;
    private List<Vector3> path = new List<Vector3>();

    [SerializeField] GameObject target;
    //[SerializeField] Transform waypoints;
    [SerializeField] bool pursue;
    [NonSerialized] public bool is_moving;
    [SerializeField] float pursueDist = 5.0f;
    [NonSerialized] public float distance;
    [NonSerialized] public bool attacking;

    //public LayerMask layersToHit;

    // Start is called before the first frame update
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        path.Clear();
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
        is_moving = false;
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
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));

        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20))
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = 0.5f;
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / agent.speed;
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }


    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!character_info.dead && !attacking)
        {
            if (pursue)
            {
                distance = (this.transform.position - target.transform.position).magnitude;
                if (distance <= pursueDist) { Pursue(); }
            }
            else { Evade(); }
        }

        Debug.Log($"Current Speed is {agent.velocity.magnitude}");
        is_moving = (agent.velocity.magnitude > 0.01f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Sprite : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 reversed_target = 3 * transform.position - 2 * target.position;
        transform.LookAt(reversed_target);
    }
}

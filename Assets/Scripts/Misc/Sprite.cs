using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum TrackingStyle
{
    Precise,
    Reduced,
    Upright,
    None
}

public class Sprite : MonoBehaviour
{
    private Transform target;
    public TrackingStyle tracking_style;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 reversed_target = 3 * transform.position - 2 * target.position;
        switch (tracking_style)
        {
            case TrackingStyle.Precise:
                break;
            case TrackingStyle.Reduced:
                reversed_target.x /= 2;
                break;
            case TrackingStyle.Upright:
                reversed_target.x = 0;
                break;
            default: break;
        }
        transform.LookAt(reversed_target);
    }
}

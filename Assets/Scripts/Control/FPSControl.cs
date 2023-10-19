using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControl : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public float moveSpeed = 2.0f;
    public float minTurnAngle = -80.0f;
    public float maxTurnAngle = 80.0f;
    public float interactDist = 1.5f;
    public LayerMask layersToHit;

    GameObject cam;
    Ray contextRay;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Move Camera
        float new_y = Input.GetAxis("Mouse X") * turnSpeed + transform.eulerAngles.y;
        float new_x = Input.GetAxis("Mouse Y") * turnSpeed * -1f + cam.transform.eulerAngles.x;
        transform.eulerAngles = new Vector3(0, new_y, 0);
        cam.transform.eulerAngles = new Vector3(new_x, new_y, 0);

        // Move Character
        Vector3 dir = new Vector3(0, 0, 0);
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        // Draw New Context Ray
        contextRay = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawLine(cam.transform.position, cam.transform.position + interactDist * cam.transform.forward, Color.blue);
        if (Physics.Raycast(contextRay, out RaycastHit hit, interactDist, layersToHit))
        {
            Debug.Log($"Looking at {hit.collider.gameObject.tag}");
        }
    }
}

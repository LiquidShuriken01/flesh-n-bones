using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FPSControl : MonoBehaviour
{
    public PlayerInfo player_info;

    public float turnSpeed = 4.0f;
    public float moveSpeed = 2.0f;
    public float minTurnAngle = -80.0f;
    public float maxTurnAngle = 80.0f;
    public float interactDist = 1.5f;
    public LayerMask layersToHit;

    GameObject cam;
    Ray contextRay;

    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        player_info.gm = GameObject.FindWithTag("Ruleset").GetComponent<GameMaster>();
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
        Vector3 dir = new Vector3(0, 0, 0)
        {
            x = Input.GetAxis("Horizontal"),
            z = Input.GetAxis("Vertical")
        };
        transform.Translate(moveSpeed * Time.deltaTime * dir);

        // Draw New Context Ray
        contextRay = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawLine(cam.transform.position, cam.transform.position + interactDist * cam.transform.forward, Color.blue);
        if (!paused && Physics.Raycast(contextRay, out RaycastHit hit, interactDist, layersToHit))
        {
            Debug.Log($"Looking at {hit.collider.gameObject.tag}");
            if (Input.GetMouseButtonDown(0) && !paused)
            {
                player_info.Interact(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1.0f;
        }

        /*if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(player_info.GetStatValueInt("base_atk_bonus"));
            Debug.Log(player_info.GetStatValueInt("carapace"));
            Debug.Log(player_info.GetStatValueInt("mucus"));
            Debug.Log(player_info.GetStatValueInt("ectoplasm"));
        }*/
    }
}

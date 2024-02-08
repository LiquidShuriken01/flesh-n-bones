using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{
    public PlayerInfo player_info;

    // Start is called before the first frame update
    void Start()
    {
        player_info.gm = GameMaster._instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(player_info.GetStatValueInt("base_atk_bonus"));
            Debug.Log(player_info.GetStatValue("speed"));
            Debug.Log(player_info.GetStatValueInt("carapace"));
            Debug.Log(player_info.GetStatValueInt("mucus"));
            Debug.Log(player_info.GetStatValueInt("ectoplasm"));
        }
    }

    public void PlayerInteract(GameObject target)
    {
        if (target == null) return;
        player_info.Interact(target);
    }
}

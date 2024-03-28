using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        player_info.StatTick();

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
        if (target.CompareTag("Enemy"))
        {
            CharacterInfo enemy_info = target.GetComponent<Enemy>().character_info;
            player_info.gm.AttackRoll(enemy_info, player_info.char_name, player_info.GetStatValueInt("base_atk_bonus"), 10, AtkType.Carapace);
        }
        else if (target.CompareTag("Interactable"))
        {
            Debug.Log("Not yet implemented");
        }
        else if (target.CompareTag("Item"))
        {
            target.GetComponent<WorldItem>().ShowContainer();
        }
    }

    public void UseSkill(int index)
    {
        int skillID = player_info.GetMemorizedSkillIndex(index);
        if (skillID == -1) { return; }
        if (DataManager._instance.IsInCD(skillID)) 
        {
            Debug.Log($"Skill {DataManager._instance.skill_list[skillID].name} is in cooldown.");
            return;
        }
        DataManager._instance.skill_list[skillID].Activate();
        DataManager._instance.SetSkillCDTimer(skillID);
    }
}

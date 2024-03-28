using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerInfo : CharacterInfo
{
    public int bones;
    private int skill_slots;
    private int filled_slots;
    private List<int> skills_memorized = new List<int>();
    private List<bool> unlocked_skills = new List<bool>(/*new bool[DataManager._instance.skill_list.Count]*/);

    void OnEnable()
    {
        this.char_name = "Player";
        this.health = this.max_health = this.GetStatValue("max_health");
        this.nerve = this.max_nerve = this.GetStatValue("max_nerve");
        skill_slots = 1; // TODO: add skill slots formula
        filled_slots = 0;
    }

    public void MemorizeSkill(int index)
    {
        if (filled_slots+DataManager._instance.skill_list[index].slot_n > skill_slots) { return; }
        if (skills_memorized.Contains(index)) { return; }
        skills_memorized.Add(index);
        filled_slots += DataManager._instance.skill_list[index].slot_n;
    }
    
    public void RemoveSkill(int index)
    {
        if (!skills_memorized.Remove(index)) { return; }
        filled_slots -= DataManager._instance.skill_list[index].slot_n;
    }

    public int GetMemorizedSkillIndex(int i)
    {
        if (i < 0 || i >= skills_memorized.Count) { return -1; }
        return skills_memorized[i];
    }
}

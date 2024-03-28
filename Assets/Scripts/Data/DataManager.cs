using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public PlayerInfo player_info;
    public CharacterInfo fishhead_info;
    public CharacterInfo worker_info;
    public CharacterInfo cancer_info;

    [System.Serializable]
    private class SkillList
    {
        public List<Skill> _list;
    }

    [System.Serializable]
    private class OrganList
    {
        public List<Organ> _list;
    }

    /*[SerializeField]
    private List<string> loading_list_skills;
    [SerializeField]
    private List<string> loading_list_organs;
    */
    public int organs_in_session = 0;
    public List<Skill> skill_list = new List<Skill>();
    public List<Organ> organ_list = new List<Organ>();
    public List<Effect> effect_list = new List<Effect>();
    /* The effect_list should be a set of base effects, e.g. lifeleech, stunned */

    private List<float> cd_timers = new List<float>();

    public static DataManager _instance { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            LoadSkills();
            LoadOrgans();
            LoadPlayerInfo();
            LoadMobInfo();

            Effect slowed = new Effect(0, "slowed", EffectType.Debuff, 2.5f);
            slowed.AddModifier("speed", -10f, ModType.Flat);
            effect_list.Add(slowed);

            _instance = this;
        }

        
        foreach (Skill skill in skill_list)
        {
            Debug.Log($"{skill.name}");
            cd_timers.Add(0f);
        }
        
    }
    
    private void LoadSkills()
    {
        Debug.Log("Loading skill data...");

        /*string path = $"{Application.dataPath}/Data/skills.json";
        string json = File.ReadAllText(path);
        SkillList skillList = JsonUtility.FromJson<SkillList>(json);
        skill_list = skillList._list;

        Debug.Log($"... success! {skill_list.Count} skills loaded");*/

        // Test case. Will remove later
        Skill adrenaline_rush = new Skill(0, "Adrenaline Rush", (SkillFlags.TargetSelf & SkillFlags.HasEffect), 1, 15f);
        skill_list.Add(adrenaline_rush);
    }

    private void LoadOrgans()
    {
        Debug.Log("Loading organ data...");
        
        int count = 0;
        /*foreach (string s in loading_list_skills)
        {
            string path = $"{Application.dataPath}/Data/Skills/{s}.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(Organ));
            StreamReader reader = new StreamReader(path);
            Organ deserialized = (Organ)serializer.Deserialize(reader.BaseStream);
            reader.Close();
            if (deserialized == null) { continue; }
            deserialized.LoadModifiers();
            organ_list.Add(deserialized);
            count++;
        }
        */
        organ_list.Add(new Organ(0, "muscle"));
        organ_list.Add(new Organ(1, "chitin_gland"));
        organ_list.Add(new Organ(2, "fat"));
        Debug.Log($"... success! {count} organs loaded");
    }

    private void LoadPlayerInfo()
    {
        player_info.ClearStats();
        player_info.bones = 0;
        player_info.AddStat("max_health", 100f);
        player_info.AddStat("max_nerve", 20f);
        player_info.AddStat("base_atk_bonus", 5f, true);
        player_info.AddStat("speed", 5f, true, false);
        player_info.AddStat("carapace", 50f, true);
        player_info.AddStat("mucus", 50f, true);
        player_info.AddStat("ectoplasm", 50f, true);
        player_info.RestoreStatus();

        //player_info.MemorizeSkill(0);
    }

    private void LoadMobInfo()
    {
        fishhead_info.ClearStats();
        fishhead_info.AddStat("max_health", 50f);
        fishhead_info.AddStat("base_atk_bonus", 5f, true);
        fishhead_info.AddStat("carapace", 30f, true);
        fishhead_info.AddStat("mucus", 30f, true);
        fishhead_info.AddStat("ectoplasm", 30f, true);
        fishhead_info.RestoreStatus();
    }

    public void SetSkillCDTimer(int index)
    {
        cd_timers[index] = skill_list[index].cd;
    }

    public bool IsInCD(int index)
    {
        return cd_timers[index] > 0f;
    }

    void Update()
    {
        /*if (player_info.health <= 0)
        {
            SceneManager.LoadScene("IntroSlides");
        }*/

        for (int i=0; i<skill_list.Count; i++)
        {
            if (cd_timers[i] > 0f)
            {
                cd_timers[i] -= Time.deltaTime;
            }
        }
    }
}

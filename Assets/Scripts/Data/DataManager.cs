using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

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

    public static DataManager _instance { get; private set; }

    /*[SerializeField]
    private List<string> loading_list_skills;
    [SerializeField]
    private List<string> loading_list_organs;
    */
    private List<Skill> skill_list = new List<Skill>();
    private List<Organ> organ_list = new List<Organ>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            //LoadSkills();
            //LoadOrgans();
            LoadPlayerInfo();
            LoadMobInfo();
            _instance = this;
        }

        foreach (Skill skill in skill_list)
        {
            Debug.Log($"{skill.name}");
        }
    }
    
    private void LoadSkills()
    {
        Debug.Log("Loading skill data...");

        string path = $"{Application.dataPath}/Data/skills.json";
        string json = File.ReadAllText(path);
        SkillList skillList = JsonUtility.FromJson<SkillList>(json);
        skill_list = skillList._list;

        Debug.Log($"... success! {skill_list.Count} skills loaded");
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
        Debug.Log($"... success! {count} organs loaded");
    }

    private void LoadPlayerInfo()
    {
        player_info.AddStat("max_health", 100f, false);
        player_info.AddStat("max_nerve", 20f, false);
        player_info.AddStat("base_atk_bonus", 5f, true);
        player_info.AddStat("carapace", 50f, true);
        player_info.AddStat("mucus", 50f, true);
        player_info.AddStat("ectoplasm", 50f, true);
    }

    private void LoadMobInfo()
    {
        fishhead_info.AddStat("max_health", 50f, false);
        fishhead_info.AddStat("base_atk_bonus", 5f, true);
        fishhead_info.AddStat("carapace", 30f, true);
        fishhead_info.AddStat("mucus", 30f, true);
        fishhead_info.AddStat("ectoplasm", 30f, true);
    }
}

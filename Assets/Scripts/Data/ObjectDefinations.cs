using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Stat
{
    public float base_value { get; private set; }
    public float total_value { get; private set; }
    private string name;
    private Dictionary<string,float> modifiers = new Dictionary<string,float>();
    public void AddModifier(string source, float mod)
    {
        if (mod != 0)
        {
            modifiers.Add(source, mod);
            total_value += mod;
        }
    }

    public void RemoveModifier(string source, float mod)
    {
        if (!modifiers.ContainsKey(source)) { return; }
        if (mod != 0)
        {
            modifiers.Remove(source, out mod);
            total_value -= mod;
        }
    }

    public void ClearModifiers()
    { 
        modifiers.Clear();
        total_value = base_value;
    }

    public bool IsStatName(string statName)
    {
        return (statName == name);
    }
}

[System.Serializable]
[XmlRoot("Skill")]
public class Skill
{
    [XmlAttribute("id")]
    public uint id;
    [XmlAttribute("name")]
    public string name;
    public float dmg;
    public float cd;
    [XmlElement("melee")]
    public bool is_melee;
    [XmlElement("self")]
    public bool self_target;
}

[System.Serializable]
[XmlRoot("Organ")]
public class Organ
{
    [XmlAttribute("id")]
    public uint id;
    [XmlAttribute("name")]
    public string name;
    private string buff_json;
    private Dictionary<string, int> modifiers = new Dictionary<string, int>();

    public void LoadModifiers()
    {
        modifiers = JsonConvert.DeserializeObject<Dictionary<string, int>>(buff_json);
    }


}
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public enum ModType
{
    Flat,
    Percentile
}

public enum AtkType
{
    Carapace,
    Mucus,
    Ectoplasm
}

/* TODO:
 *      Implement a Damage class that contains a list of damages by types.
 *      Some functions ideas are: ToString() for log purposes, ApplyResistances() reduce each damage according to
 *      target's resistances.
 *      TakeDamage() will be modified to take in Damage as param.
 */

[System.Serializable]
public class Modifier
{
    public float value { get; private set; }
    public float duration { get; private set; }
    public ModType mod_type { get; private set; }

    public Modifier(float value, float duration, ModType modType)
    {
        this.value = value;
        this.duration = duration;
        this.mod_type = modType;
    }
    
    public bool Tick()
    {
        if (this.duration > 0)
        {
            duration -= Time.deltaTime;
            if (duration < 0) { return true; }
        }
        return false;
    }
}

[System.Serializable]
public class Stat
{
    public float base_value { get; private set; }
    public float total_value 
    {
        get {
            if (is_dirty)
            {
                value = CalculateValue();    
            }
            return value;
        }

        private set { }
    }
    private string name;
    private bool is_dirty = false;
    private bool rounding;
    private float value;
    private Dictionary<string, Modifier> modifiers;
    
    public Stat(string n, float baseValue, bool rounding) 
    {
        this.name = n;
        this.base_value = baseValue;
        this.rounding = rounding;
        this.modifiers = new Dictionary<string,Modifier>();
    }

    public void AddModifier(string source, Modifier mod)
    {
        if (mod.value != 0)
        {
            is_dirty = true;
            modifiers.Add(source, mod);
        }
    }

    public bool RemoveModifier(string source, Modifier mod)
    {
        is_dirty = true;
        return modifiers.Remove(source, out mod);
    }

    private float CalculateValue()
    {
        float _value = base_value;
        foreach (KeyValuePair<string,Modifier> kvp in modifiers)
        {
            if (kvp.Value.mod_type == ModType.Flat)
            {
                _value += kvp.Value.value;
            }
            else if (kvp.Value.mod_type == ModType.Percentile)
            {
                _value += kvp.Value.value * base_value;
            }
        }
        if (rounding)  { return (float)System.Math.Round(_value, 0); }
        else  { return (float)System.Math.Round(_value, 3); }
    }

    public void ClearModifiers()
    { 
        modifiers.Clear();
        value = base_value;
        is_dirty = false;
    }

    public bool IsStatName(string statName)
    {
        return (statName == name);
    }

    /* progresses time for each Modifier affecting .this Stat
     * call this in Update() of PlayerInfo for each Stat in its list
     */
    public void ModifiersTick()
    {
        List<string> srcToRemove = new List<string>();
        List<Modifier> modToRemove = new List<Modifier>();

        foreach (KeyValuePair<string, Modifier> kvp in modifiers)
        {
            if (kvp.Value.Tick())
            {
                srcToRemove.Add(kvp.Key);
                modToRemove.Add(kvp.Value);
            }
        }
        for (int i=0; i<srcToRemove.Count; i++) 
        {
            RemoveModifier(srcToRemove[i], modToRemove[i]);
        }
    }
}

[System.Serializable]
public class Skill
{
    public uint id;
    public string name;
    public float dmg;
    public float cd;
    public List<string> tags;
    public List<uint> effects;
}

[System.Serializable]
public class Organ
{
    public uint id;
    public string name;
    private string buff_json;
    private Dictionary<string, int> modifiers = new Dictionary<string, int>();

    public void LoadModifiers()
    {
        modifiers = JsonConvert.DeserializeObject<Dictionary<string, int>>(buff_json);
    }


}
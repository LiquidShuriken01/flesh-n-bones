using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using UnityEditor;

public enum DmgType
{
    Physical,
    Corrosive,
    Psi
}

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

public enum EffectType
{
    DamageOverTime,
    Buff,
    Debuff
}

[System.Flags] public enum SkillFlags
{
    None = 0,
    Attack = 1 << 0,
    CreatesProjectile = 1 << 1,
    TargetSelf = 1 << 2,
    HasEffect = 1 << 3,
}

/* TODO:
 *      Implement a Damage class that contains a list of damages by types.
 *      Some function ideas are: ToString() for log purposes, ApplyResistances() reduce each damage according to
 *      target's resistances.
 *      TakeDamage() will be modified to take in Damage as param.
 */
public class Damage
{
    private float[] dmg_table;

    public Damage()
    {
        dmg_table = new float[System.Enum.GetNames(typeof(DmgType)).Length];
    }

    public Damage(float dmgVal, DmgType dmgType)
    {
        dmg_table = new float[System.Enum.GetNames(typeof(DmgType)).Length];
        dmg_table[(int)dmgType] = dmgVal;
    }

    public void SetDamage(float dmgVal, DmgType dmgType)
    {
        dmg_table[(int)dmgType] = dmgVal;
    }

    public void GetDamage(float dmgVal, DmgType dmgType)
    {
        dmg_table[(int)dmgType] = dmgVal;
    }
}

[System.Serializable]
public class Modifier
{
    public string stat_name { get; private set; }
    public float value { get; private set; }
    public float duration { get; private set; }
    public ModType mod_type { get; private set; }

    public Modifier(string name, float value, float duration, ModType modType)
    {
        this.stat_name = name;
        this.value = value;
        this.duration = duration;
        this.mod_type = modType;
    }

    public override string ToString()
    {
        return stat_name + " -- " + ((value > 0) ? "+" : "") + value.ToString() + ((mod_type == ModType.Percentile) ? "%" : "");
    }

    public bool Tick()
    {
        if (this.duration > 0)
        {
            Debug.Log($"{this.stat_name} modifier ticks...");
            duration -= Time.deltaTime;
            if (duration <= 0) { return true; }
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
    private readonly string name;
    private bool is_dirty = false;
    private readonly bool rounding;
    private readonly bool can_be_neg;
    private float value;
    private Dictionary<string, Modifier> modifiers;

    public Stat(string n, float baseValue, bool rounding=false, bool canBeNeg=true)
    {
        this.name = n;
        this.base_value = baseValue;
        this.value = baseValue;
        this.rounding = rounding;
        this.can_be_neg = canBeNeg;
        this.modifiers = new Dictionary<string, Modifier>();
    }

    public void AddModifier(string source, Modifier mod)
    {
        if (mod.value != 0)
        {
            is_dirty = true;
            modifiers[source] = mod;
        }
    }

    public bool RemoveModifier(string source)
    {
        is_dirty = true;
        return modifiers.Remove(source);
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
        if (!can_be_neg && _value < 0)  { return 0; }
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
        return (statName.Equals(name));
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
                Debug.Log($"Effect {kvp.Key}: {this.name} modifier wears off");
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
public class Effect
{
    public uint id;
    public string name;
    public EffectType type;
    public float duration;

    private List<Modifier> ModTable = new List<Modifier>();

    public Effect(uint i, string n, EffectType t, float d)
    {
        id = i;
        name = n;
        type = t;
        duration = d;
    }

    public void AddModifier(string targetStat, float modVal, ModType modType)
    {
        Modifier new_mod = new Modifier(targetStat, modVal, duration, modType);
        ModTable.Add(new_mod);
    }

    public void ApplyModifiers(CharacterInfo character)
    {
        foreach (Modifier mod in ModTable)
        {
            character.BuffStat(mod, name);
        }
    }
}


[System.Serializable]
public class Skill
{
    public uint id;
    public string name;
    public SkillFlags flags;
    public int slot_n = 1;
    public float cd;

    private float dmg = 0;
    private List<int> effects;

    public Skill(uint id, string name, SkillFlags flags, int slot_n, float cd)
    {
        this.id = id;
        this.name = name;
        this.flags = flags;
        this.slot_n = slot_n;
        this.cd = cd;
        effects = new List<int>();
    }

    public void AddEffect(int effectID)
    {
        effects.Add(effectID);
    }

    public void SetDamage(float d)
    {
        dmg = d;
    }

    public void Activate(PlayerInfo player)
    {
        Debug.Log($"{flags.HasFlag(SkillFlags.Attack)}, {flags.HasFlag(SkillFlags.CreatesProjectile)}, {flags.HasFlag(SkillFlags.TargetSelf)}, {flags.HasFlag(SkillFlags.HasEffect)}");
        if (flags.HasFlag(SkillFlags.Attack))
        {
            if (flags.HasFlag(SkillFlags.HasEffect))
            {

            }
        }
        else if (flags.HasFlag(SkillFlags.CreatesProjectile)) 
        {

        }
        else if (flags.HasFlag(SkillFlags.TargetSelf)) 
        {
            Debug.Log("Buffing Self...");
            foreach(int i in effects)
            {
                DataManager._instance.effect_list[i].ApplyModifiers(player);
            }
        }
    }
}

[System.Serializable]
public class Organ
{
    public uint id;
    public string name;
    private readonly string buff_json;
    private List<Modifier> modifiers = new List<Modifier>();

    public Organ(uint i, string n) 
    {
        id = i;
        name = n;
        switch (i)
        {
            case 0:
                modifiers.Add(new Modifier("speed", 1f, 0, ModType.Flat));
                modifiers.Add(new Modifier("base_atk_bonus", 8f, 0, ModType.Flat));
                break;
            case 1:
                modifiers.Add(new Modifier("carapace", 10f, 0, ModType.Flat));
                break;
            case 2:
                modifiers.Add(new Modifier("max_health", 0.1f, 0, ModType.Percentile));
                break;
            default:
                break;
        }
    }
    
    public Organ(Organ other) 
    {
        this.id = other.id;
        this.name = other.name;
        this.buff_json = other.buff_json;
        this.modifiers = new List<Modifier>();
        foreach(Modifier mod in other.modifiers)
        {
            this.modifiers.Add(mod);
        }
    }

    public void LoadModifiers()
    {
        //modifiers = JsonConvert.DeserializeObject<Dictionary<string, int>>(buff_json);
    }

    public List<Modifier> GetModifiers()
    {
        List<Modifier> res = new List<Modifier>();
        foreach(Modifier mod in modifiers) 
        {
            res.Add(mod);
        }

        return res;
    }

    public void ApplyModifiers(CharacterInfo character)
    {
        foreach (Modifier mod in modifiers)
        {
            character.BuffStat(mod, name);
        }
    }


    public string ModString()
    {
        string res = string.Empty;
        foreach(Modifier mod in modifiers)
        {
            res += mod.ToString() + "\n";
        }
        return res;
    }
}
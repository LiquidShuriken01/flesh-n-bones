using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CharacterInfo : ScriptableObject
{
    public string char_name;
    public float health;
    public float max_health;
    public float nerve;
    public float max_nerve;

    public bool dead = false;

    // These info will be loaded from external files in the future
    [SerializeField]
    private List<Stat> stat_block = new();

    public GameMaster gm;

    public Stat AddStat(string statName, float baseValue, bool rounding)
    {
        Stat new_stat = new Stat(statName, baseValue, rounding);
        stat_block.Add(new_stat);
        if (statName == "max_health") { max_health = baseValue; }
        if (statName == "max_nerve") { max_nerve = baseValue; }
        return new_stat;
    }

    public void ClearStats()
    {
        stat_block.Clear();
    }

    public float GetStatValue(string statName)
    {
        foreach (Stat stat in stat_block)
        {
            if (stat.IsStatName(statName))
            {
                return stat.total_value;
            }
        }
        return 0;
    }

    public int GetStatValueInt(string statName)
    {
        foreach (Stat stat in stat_block)
        {
            if (stat.IsStatName(statName))
            {
                return Mathf.RoundToInt(stat.total_value);
            }
        }
        return 0;
    }

    public void RestoreStatus()
    {
        dead = false;
        health = max_health;
        nerve = max_nerve;
    }

    public void BuffStat(string statName, float value, float duration, ModType modType, string source)
    {
        Modifier new_mod = new Modifier(value, duration, modType);

        foreach (Stat stat in stat_block)
        {
            if (stat.IsStatName(statName))
            {
                stat.AddModifier(source, new_mod);
                if (statName == "max_health")
                {
                    max_health = stat.total_value;
                }
                if (statName == "max_nerve")
                {
                    max_nerve = stat.total_value;
                }
                return;
            }
        }

        bool rounding = (modType == ModType.Flat);
        Stat new_stat = AddStat(statName, 0f, rounding);
        new_stat.AddModifier(source, new_mod);
    }

    public void TakeDamage(int dmg)
    {
        if (dmg < 0)
        {
            Debug.LogError("Damage can't be negative");
            return;
        }

        this.health = (dmg <= this.health) ? (this.health-dmg) : 0;

        if (this.health == 0) 
        {
            Death();
        }
    }

    private void Death()
    {
        dead = true;
    }

}

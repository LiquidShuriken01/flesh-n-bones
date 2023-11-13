using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : ScriptableObject
{
    public string name;
    public int health;
    public int max_health;
    public int nerve;
    public int max_nerve;

    public bool dead = false;

    private List<Stat> stat_block = new List<Stat>();

    public Stat AddStat(string statName, float baseValue, bool rounding)
    {
        Stat new_stat = new Stat(statName, baseValue, rounding);
        stat_block.Add(new_stat);
        return new_stat;
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

    public void BuffStat(string statName, float value, float duration, ModType modType, string source)
    {
        Modifier new_mod = new Modifier(value, duration, modType);

        foreach (Stat stat in stat_block)
        {
            if (stat.IsStatName(statName))
            {
                stat.AddModifier(source, new_mod);
                return;
            }
        }

        bool rounding = (modType == ModType.Flat) ? true : false;
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
